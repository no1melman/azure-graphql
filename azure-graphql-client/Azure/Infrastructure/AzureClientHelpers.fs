namespace DevOpsCentre.Bff.Azure

open System
open System.Text.Json
open System.Threading.Tasks

open Microsoft.Extensions.Logging.Abstractions
open Microsoft.Extensions.Logging
open System.IO
open System.Net.Http
open System.Net

open DevOpsCentre.Bff

type WrappedResponse<'T> = { Value: 'T; Count: int }

module HttpHelpers =
  let writeResponseToFile (name: string) (data: string) = task {
      let tempRandomString = Guid.NewGuid().ToString("N")
      use fs = File.Create($"AzureResponse-{name}-{tempRandomString}.json")
      use writer = new StreamWriter(fs)
      do! writer.WriteAsync(data)
    }

  let enumeratorAsSeq<'T> (enumerator: Collections.Generic.IEnumerator<'T>) =
    Seq.unfold(fun _ -> 
      if enumerator.MoveNext() then 
        Some(enumerator.Current, ())
      else 
        enumerator.Dispose()
        Option.None)


  let getResposonse (res: HttpResponseMessage) = res.Content.ReadAsStringAsync()

  let getHeaders (header: string) (res: HttpResponseMessage) =
    let mutable empty = Linq.Enumerable.Empty<string>()
    res.Headers.TryGetValues(header, &empty) |> ignore
    match empty with
    | null -> Option.None
    | a -> a |> Seq.cast<string> |> Seq.tryHead

  let prettyPrintHeaders (res: HttpResponseMessage) =
    let joinWith = String.join "\n"
    let joinWithComma = String.join ", "
    res.Headers |> Seq.map (fun a -> 
        $"{a.Key}={joinWithComma a.Value}"
    ) |> joinWith

  let getContinuationToken = getHeaders "x-ms-continuationtoken"
  let getContentType = getHeaders "Content-Type"
  let deserialiseData<'a> (data: Stream) (logger: ILogger) = 
    try 
        JsonSerializer.DeserializeAsync<'a>(data, JsonConfiguration.options logger)
    with e ->
        logger.LogCritical(e, "Json Deserialisation has failed") |> ignore
        invalidOp "Unable to deserialise the response recieved from the Azure Client"

  let deserialiseResponse<'T> (logger: ILogger) (res: HttpResponseMessage) = task {       
      let continuationToken = getContinuationToken res

      let! stream = res.Content.ReadAsStreamAsync()
      let! deserialisedData = deserialiseData<'T> stream logger

      // do! writeResponseToFile "data" (JsonSerializer.Serialize(deserialisedData))
      // do! writeResponseToFile "headers" (JsonSerializer.Serialize(enumeratorAsSeq (res.Headers.GetEnumerator()) ()))

      //let! toWrite = 
      //  if stream.CanSeek then 
      //      stream.Position <- 0L
      //      Stream.readAsString stream |> Task.map Some
      //  else Task.lift Option.None
              
      //toWrite |> Option.iter (fun data -> logger.LogInformation($"Data: %A{data}") |> ignore)

      return deserialisedData, continuationToken
    }

  let createQueryParams paramList =
    String.Join(
      "&",
      paramList |> List.filter (String.IsNullOrWhiteSpace >> not))

  let createJsonContent data : HttpContent = 
    let serialiseContent = JsonSerializer.Serialize(data, (JsonConfiguration.options NullLogger.Instance))
    new StringContent(serialiseContent, Text.Encoding.UTF8, "application/json") :> HttpContent


module HttpClient =
  type DeserialisedResponse<'a> =
    | JsonResult of 'a
    | StringResult of string
    | Unknown of string

  type HttpResponse<'a> =
    | Success of DeserialisedResponse<'a>
    | SuccessNoContent
    | Error of HttpStatusCode * DeserialisedResponse<'a>

  let deserialiseHttpResponse<'a> (logger: ILogger) (res: HttpResponseMessage) = task {
        let maybeContentType = HttpHelpers.getContentType res
        logger.LogDebug($"Found headers: %A{HttpHelpers.prettyPrintHeaders res}")
        
        let getJson res = HttpHelpers.deserialiseResponse<'a> logger res |> Task.map (fst >> JsonResult)
        let getText (res: HttpResponseMessage) = res.Content.ReadAsStringAsync() |> Task.map StringResult
        let getUnknownText (res: HttpResponseMessage) a = res.Content.ReadAsStringAsync() |> Task.map (fun data -> Unknown $"Can't deserialise {a}, \n{data}")

        return! match maybeContentType with
                | Option.None ->
                    match int(res.StatusCode) with
                    | s when s >= 200 && s < 300 ->
                        // try deserialising json first...
                        logger.LogInformation("No Content Type, trying json")
                        try getJson res
                        with _ -> 
                          // then try just text...
                          logger.LogInformation("JSON failed, trying text")
                          try getText res
                          with _ -> invalidOp "Unable to deserialise content, no content type provided..."
                    | _ -> 
                        logger.LogInformation("No Content Type, bad status code, trying text")
                        getText res
                | Some contentType ->
                    match contentType with
                    | "application/json" ->  getJson res
                    | "text/plain" -> getText res
                    | a -> getUnknownText res a
    }

  let fireRequest<'a> (request: HttpClient -> Task<HttpResponseMessage>)  (client: HttpClient)  (logger: ILogger) =
    let deserialiser = fun m -> deserialiseHttpResponse<'a> logger m |> Task.map (fun d -> d,m)
    request(client)
    |> Task.bind deserialiser
    |> Task.map (fun (d, res) -> 
        let data = match int(res.StatusCode) with
                   | s when s >= 200 && s < 300 -> 
                       match s with
                       | 204 -> SuccessNoContent
                       | s -> Success d
                   | s when s >= 300 && s < 400 ->
                       Error (res.StatusCode, Unknown "Client redirect...")
                   | _ -> Error (res.StatusCode, d)
        res,data
    )

  let get<'a> (url: string) = 
    fireRequest<'a> (fun c -> c.GetAsync(url))
   
  let getWrapped<'a> = get<WrappedResponse<'a>>
  let post<'a, 'b> (url: string) (data: 'b) = 
    fireRequest<'a> (fun c ->  c.PostAsync(url, HttpHelpers.createJsonContent data))

  let pullJson = 
    function 
    | Success innerRes ->
        match innerRes with
        | JsonResult a -> a
        | b -> invalidOp $"Got a success reponse but not json data\n%A{b}"
    | b -> invalidOp $"Request Failed!\n%A{b}"
  let getJson<'a> (url: string)  (client: HttpClient) (logger: ILogger) = task {
    let! (res, data) = get<'a> url client logger

    let continuationToken = HttpHelpers.getContinuationToken res
    
    continuationToken |> Option.iter (fun ct -> logger.LogInformation("Has continuation token :: {ct}", ct))
    
    return pullJson data, continuationToken
  }
  let postJson<'a, 'b> (url: string) data (client: HttpClient) (logger: ILogger) =  task {
    let! (res, resData) = post<'a, 'b> url data client logger 

    let continuationToken = HttpHelpers.getContinuationToken res
    
    continuationToken |> Option.iter (fun ct -> logger.LogInformation("Has continuation token :: {ct}", ct))

    return pullJson resData, continuationToken
  }
  let pullText = 
      function 
      | Success innerRes ->
          match innerRes with
          | StringResult a -> a
          | b -> invalidOp $"Got a success reponse but not json data\n%A{b}"
      | b -> invalidOp $"Request Failed!\n%A{b}"
  let getText<'a> (url: string) (client: HttpClient) (logger: ILogger) = task {
      let! (res, data) = get<'a> url client logger
  
      let continuationToken = HttpHelpers.getContinuationToken res
      
      continuationToken |> Option.iter (fun ct -> logger.LogInformation("Has continuation token :: {ct}", ct))
      
      return pullText data, continuationToken
  }
  let postText<'a, 'b> (logger: ILogger) (client: HttpClient) (url: string) data =  task {
      let! (res, resData) = post<'a, 'b> url data client logger 
  
      let continuationToken = HttpHelpers.getContinuationToken res
      
      continuationToken |> Option.iter (fun ct -> logger.LogInformation("Has continuation token :: {ct}", ct))
  
      return pullText resData, continuationToken
  }
  let rec enumerateCodeSearch<'response, 'result, 'request> term (mapper: 'response -> 'result list) (results: 'result list) skip (requestCreator: int -> 'request) (getTop: 'request -> int) url (client: HttpClient) (logger: ILogger) = task {
      let request = (requestCreator skip)
      let! codeResponse, _ = 
        postJson<'response, 'request> url request client logger
      
      let listRes = mapper codeResponse
      let top = getTop(request)

      match listRes.Length with
      | resultLength when resultLength >= top ->
        return! enumerateCodeSearch<'response, 'result, 'request> term mapper listRes (top + skip) requestCreator getTop url client logger
      | _ -> // if the results is less than the top, that means we've come to the end
        return results @ listRes
}