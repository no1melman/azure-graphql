namespace DevOpsCentre.Bff.Azure

open System

open System.Net.Http
open System.Net.Http.Headers

open Microsoft.Extensions.Logging

open DevOpsCentre.Bff.Azure.Client.Application

open DevOpsCentre.Shared.Azure.Domain

module Infrastructure =

  type AzureClient(httpClientFactory: IHttpClientFactory, logger: ILogger<AzureClient>) =
    let httpClient = httpClientFactory.CreateClient("azure")
    let searchHttpClient = httpClientFactory.CreateClient()

    do
      httpClient.BaseAddress <- Uri("https://dev.azure.com/cbinfrastructure/cbi/_apis/")
      httpClient.DefaultRequestHeaders.Authorization <- AuthenticationHeaderValue("Basic", "{{ DEVOPS_TOKEN }}")
      httpClient.DefaultRequestHeaders.Add("Accept", seq { "text/html"; "application/json"; "text/plain" })
      searchHttpClient.BaseAddress <- Uri("https://almsearch.dev.azure.com/cbinfrastructure/cbi/_apis/")
      searchHttpClient.DefaultRequestHeaders.Authorization <- AuthenticationHeaderValue("Basic", "{{ DEVOPS_TOKEN }}")

    member private __.GetJson<'a> url = HttpClient.getJson<'a> url httpClient logger
    member private __.GetText<'a> url = HttpClient.getText<'a> url httpClient logger
    member private __.GetJsonWrapped<'a> url = HttpClient.getJson<WrappedResponse<'a>> url httpClient logger
    member private __.SearchGetJson<'a> url = HttpClient.getJson<'a> url searchHttpClient logger
    member private __.SearchPost<'a, 'b> url data = HttpClient.post<'a,' b> url data searchHttpClient logger 
    member private __.SearchPostJson<'a, 'b> url data = HttpClient.postJson<'a, 'b> url data searchHttpClient logger 
    member private __.SearchGetJsonWrapped<'a> url = HttpClient.getJson<WrappedResponse<'a>> url searchHttpClient logger 

    interface IAzureClient with
      member __.Repositories () = task {
          let! devopsRes, _ = __.GetJsonWrapped<GitRepository[]> "git/repositories?includeLinks=true&includeAllUrls=true&api-version=6.1-preview.1"

          let respositories = devopsRes.Value |> Array.toList

          return respositories // |> List.filter(fun f -> Team.allRepos.ContainsKey f.Name)
        }
      member __.Repository repositoryId = task {
          let! data, _ = __.GetJson<GitRepository> $"git/repositories/%s{repositoryId}?api-version=6.1-preview.1"

          return data
        }
      member __.RepositoryFromPath path = task {
          let! data, _ = __.GetJson<GitRepository> path

          return data
        }
      member __.Commits href = task {
          let! data, _ = __.GetJsonWrapped<Commit[]> $"{href}?searchCriteria.includeLinks=true"

          return data.Value |> Array.toList
        }
      member __.CommitsFrom repositoryId date = task {
          let isoDate = date.ToString("o")
          let! data, _ = __.GetJsonWrapped<Commit[]> $"git/repositories/%s{repositoryId}/commits?searchCriteria.fromDate={isoDate}&searchCriteria.$top=10000&api-version=6.1-preview.1"

          return data.Value |> Array.toList
      }
      member __.Builds repositoryId branchName buildIds = task {
          let queryList = HttpHelpers.createQueryParams [
              // without statusFilter, this query doesn't return anything, strangely enough
              //statusFilter=all&
              "api-version=6.1-preview.6"
              if String.IsNullOrWhiteSpace(branchName) then "" else $"branchName={branchName}"
              if buildIds.Length > 0 then $"buildIds={String.Join(',', buildIds)}" 
              else $"repositoryId={repositoryId}&statusFilter=all&deletedFilter=includeDeleted&repositoryType=TfsGit" // can't filter and do build ids
            ]

          let! data, _ = __.GetJsonWrapped<Build[]> $"build/builds?{queryList}"

          return data.Value |> Array.toList
        }
      member __.Commit commitId repositoryId = task {
          let! data, _ = __.GetJson<Commit> $"git/repositories/{repositoryId}/commits/{commitId}"

          return data
        }
      member __.Refs repositoryId filter contains includeStatus = task {
          let queryList = HttpHelpers.createQueryParams [
                if String.IsNullOrWhiteSpace(filter) then "" 
                else 
                    if contains then $"filterContains={filter}"
                    else $"filter={filter}"
                if includeStatus then $"includeStatuses=true"
                "api-version=6.1-preview.1"
            ]

          let! data, _ = __.GetJsonWrapped<GitRef[]> $"git/repositories/{repositoryId}/refs?{queryList}"

          return data.Value |> Array.toList
        }  
      member __.Plan planUrl = task {
          let! data, _ = __.GetText planUrl
          
          do! HttpHelpers.writeResponseToFile "Plan" data
        }
      member __.Pipelines () = task {
          let! data, _ = __.GetJsonWrapped<Pipeline[]> "pipelines?api-version=6.1-preview.1"

          return data.Value |> Array.toList
        }
      member __.PullRequests repositoryId = task {
          let! data, _ = __.GetJsonWrapped<PullRequest[]> $"git/repositories/{repositoryId}/pullrequests?api-version=6.1-preview.1"

          return data.Value |> Array.toList
        }
      member __.CodeSearch term = task {
          let! codeResponse, _ = __.SearchPostJson "search/codesearchresults?api-version=6.1-preview.1" (CodeSearchRequest.Default term)

          return codeResponse
      }
      member __.AllCodeSearches term = task {
          let! results = 
            let withTerm = HttpClient.enumerateCodeSearch<CodeSearchResponse, CodeResult, CodeSearchRequest> term 
            let withExtractor = withTerm (fun res -> res.Results |> List.ofArray) 
            let withSetup = withExtractor [] 0 
            let withRequest = withSetup (fun skip -> { (CodeSearchRequest.Default term) with Skip = skip })
            let withTopExtractor = withRequest (fun req -> req.Top)
            let withUrl = withTopExtractor "search/codesearchresults?api-version=6.1-preview.1" 
            withUrl searchHttpClient logger
          
          return {
            Results = results
            RepositoryCount = results |> List.map(fun f -> f.Repository.Name, f) |> Map.ofList |> Map.count
          }
      }

