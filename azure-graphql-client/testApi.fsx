
open System
open System.Threading.Tasks
open System.Net.Http
open System.Net.Http.Headers

let httpClient =
  let httpClient = HttpClient()
  httpClient.BaseAddress <- Uri("https://dev.azure.com/cbinfrastructure/cbi/_apis/")
  httpClient.DefaultRequestHeaders.Authorization <- AuthenticationHeaderValue("Basic", "{{ BASE64_TOKEN }}")
  httpClient

let makeRequest (url: string) = async {
    let! res = httpClient.GetAsync(url) |> Async.AwaitTask
    let! data = res.Content.ReadAsStringAsync() |> Async.AwaitTask

    return {| data = data; statusCode = res.StatusCode |}
  }

makeRequest "" |> Async.RunSynchronously |> printfn "%A"

