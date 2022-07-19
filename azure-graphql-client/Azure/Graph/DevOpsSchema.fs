namespace DevOpsCentre.Bff.Azure.Graph

open DevOpsCentre.Bff
open DevOpsCentre.Bff.Azure.Client.Application
open DevOpsCentre.Bff.Azure.Graph

open DevOpsCentre.Shared.Azure.Domain

open System

open GraphQL
open GraphQL.Types
open GraphQL.Utilities.Federation

open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging

type DevOpsQuery() =
  inherit ObjectGraphType<Object>()
  let getClient (ctx: GraphQL.IResolveFieldContext<Object>) =
    ctx.RequestServices.GetRequiredService<IAzureClient>()
  let getLogger (ctx: GraphQL.IResolveFieldContext<Object>) name = 
    let factory = ctx.RequestServices.GetRequiredService<ILoggerFactory>()
    factory.CreateLogger(name)
  do
    base.Field<ListGraphType<GitRepositoryGraphType>, GitRepository list>()
      .Name("Repositories")
      .Argument<StringGraphType, String>("repositoryName", "Provide a filter or leave blank for all repositories")
      .ResolveAsync(fun ctx ->
        let client = getClient ctx
        client.Repositories () |> Task.map (fun repos -> 
            match ctx.GetArgument<String>("repositoryName", "") with
            | s when isNull s -> repos
            | s when not (String.IsNullOrWhiteSpace(s)) -> 
                List.filter (fun r -> r.Name.ToLowerInvariant().Contains(s.ToLowerInvariant())) repos
            | _ -> repos
        )
      ) |> ignore
    base.Field<ListGraphType<PipelineGraphType>, Pipeline list>()
      .Name("Pipelines")
      .ResolveAsync(fun ctx ->
        let client = getClient ctx
        client.Pipelines()
      ) |> ignore
    base.Field<ListGraphType<BuildGraphType>, Build list>()
      .Name("Builds")
      .Description("Get's all the builds for a give repository and branch name, branch name is optional.")
      .Argument<StringGraphType, String>("repositoryId", "The id of repository you wish to query builds for")
      .Argument<StringGraphType, String>("branchName", "The specific branch in the repository that you wish to query builds for", String.Empty)
      .Argument<ListGraphType<StringGraphType>, String list>("buildIds", "Specify a list of specific builds to retrieve", [])
      .ResolveAsync(fun ctx -> 
        let client = getClient ctx
        let repositoryId = ctx.GetArgument<String>("repositoryId")
        let branchName = ctx.GetArgument<String>("branchName")
        let buildIds = ctx.GetArgument<System.Collections.Generic.List<String>>("buildIds")
        client.Builds repositoryId branchName (buildIds |> Seq.toList)
      ) |> ignore
    base.Field<ListGraphType<PullRequestGraphType>, PullRequest list>()
      .Name("PullRequests")
      .Argument<StringGraphType, String>("repositoryId", "The id of repository you wish to query builds for")
      .ResolveAsync(fun ctx -> 
        let client = getClient ctx
        let repositoryId = ctx.GetArgument<String>("repositoryId")
        client.PullRequests repositoryId
      ) |> ignore
    base.Field<CommitGraphType, Commit>()
      .Name("Commit")
      .Argument<StringGraphType, String>("commitId", "The id of the commit you wish to retrieve")
      .Argument<StringGraphType, String>("repositoryId", "The id of repository that contains the commit")
      .ResolveAsync(fun ctx ->
        let client = getClient ctx
        let commitId = ctx.GetArgument<String>("commitId")
        let repositoryId = ctx.GetArgument<String>("repositoryId")
        client.Commit commitId repositoryId
      ) |> ignore
    base.Field<ListGraphType<CommitGraphType>, Commit list>()
      .Name("CommitsFrom")
      .Argument<StringGraphType, String>("repositoryId", "The id of repository that contains the commit")
      .Argument<DateTimeGraphType, DateTime>("dateFrom", "All the commits from a specific point in time")
      .ResolveAsync(fun ctx ->
        let client = getClient ctx
        let repositoryId = ctx.GetArgument<String>("repositoryId")
        let dateFrom = ctx.GetArgument<DateTime>("dateFrom")
        client.CommitsFrom repositoryId dateFrom
      ) |> ignore
    base.Field<ListGraphType<GitRefGraphType>, GitRef list>()
      .Name("Refs")
      .Argument<StringGraphType, String>("repositoryId", "The id of repository that contains the commit")
      .Argument<StringGraphType, String>("filter", "Allows you to filter the results, defaults to startsWith")
      .Argument<BooleanGraphType, bool>("contains", "This changes the filter behaviour, when true uses contains")
      .Argument<BooleanGraphType, bool>("includeStatus", "Allows you to include the status for the refs")
      .ResolveAsync(fun ctx ->
        let client = getClient ctx
        let repositoryId = ctx.GetArgument<String>("repositoryId")
        let filter = ctx.GetArgument<String>("filter")
        let contains = ctx.GetArgument<bool>("contains")
        let includeStatus = ctx.GetArgument<bool>("includeStatus")
        client.Refs repositoryId filter contains includeStatus
      ) |> ignore
    base.Field<CodeSearchResponseGraphType, CodeSearchResponse>()
      .Name("CodeSearch")
      .Argument<StringGraphType, String>("searchTerm", "The term you wish to search for")
      .ResolveAsync(fun ctx ->
          let client = getClient ctx
          let searchTerm = ctx.GetArgument<String>("searchTerm")
          client.CodeSearch searchTerm
      ) |> ignore
    base.Field<AllCodeSearchResultGraphType, AllCodeSearchResult>()
      .Name("AllCodeSearches")
      .Argument<StringGraphType, String>("searchTerm", "The term you wish to search for")
      .ResolveAsync(fun ctx ->
          let client = getClient ctx
          let searchTerm = ctx.GetArgument<String>("searchTerm")
          client.AllCodeSearches searchTerm
      ) |> ignore

type DevOpsSchema (services: IServiceProvider)  =
  inherit Schema(services)
  do 
    base.Query <- services.GetRequiredService<DevOpsQuery>()