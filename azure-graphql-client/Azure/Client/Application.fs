namespace DevOpsCentre.Bff.Azure.Client

open System
open System.Threading.Tasks

open DevOpsCentre.Shared.Azure.Domain

module Application =

  type IAzureClient =
    abstract member Repositories: unit -> Task<GitRepository list>
    abstract member Repository: repositoryId:string -> Task<GitRepository>
    abstract member RepositoryFromPath: path:string -> Task<GitRepository>
    abstract member Commits: href:string -> Task<Commit list>
    abstract member CommitsFrom: repositoryId:string -> toDate:DateTime -> Task<Commit list>
    abstract member Builds: repositoryId:string -> branchName:string -> buildIds:string list -> Task<Build list>
    abstract member Commit: commitId:string -> repositoryId:string -> Task<Commit>
    abstract member Refs: repositoryId:string -> filter:string -> contains:bool -> includeStatus:bool -> Task<GitRef list>
    abstract member Plan: planUrl:string -> Task<unit>
    abstract member Pipelines: unit -> Task<Pipeline list>
    abstract member PullRequests: repositoryId:string -> Task<PullRequest list>
    abstract member CodeSearch: term:string -> Task<CodeSearchResponse>
    abstract member AllCodeSearches: term:string -> Task<AllCodeSearchResult>