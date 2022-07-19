namespace DevOpsCentre.Bff.Azure.Graph

open DevOpsCentre.Bff.Azure.Client.Application
open DevOpsCentre.Shared.Azure.Domain

open GraphQL
open GraphQL.Types
open GraphQL.Utilities.Federation

open Microsoft.Extensions.DependencyInjection
open System.Threading.Tasks

type GitUserDateGraphType() =
  inherit ObjectGraphType<GitUserDate>()

  do
    base.Field(fun f -> f.Date) |> ignore
    base.Field(fun f -> f.Email) |> ignore
    base.Field(fun f -> f.ImageUrl) |> ignore
    base.Field(fun f -> f.Name) |> ignore

type GitPushRefGraphType() =
  inherit ObjectGraphType<GitPushRef>()
  do
    base.Field<AnyScalarGraphType>("Links", resolve = (fun r -> r.Source.Links :> obj)) |> ignore
    base.Field(fun r -> r.Date) |> ignore
    base.Field(fun r -> r.PushId) |> ignore
    base.Field(fun r -> r.PushedBy) |> ignore
    base.Field(fun r -> r.Url) |> ignore

// todo: MISSING: GitRepositoryRef

type CommitGraphType() =
  inherit ObjectGraphType<Commit>()
  do 
    base.Field<AnyScalarGraphType>().Name("Links").Resolve(fun r -> r.Source.Links :> obj) |> ignore
    base.Field(fun f -> f.Author) |> ignore
    base.Field<AnyScalarGraphType>(name = "ChangeCounts", resolve = (fun f -> f.Source.ChangeCounts :> obj)) |> ignore
    base.Field(fun f -> f.Changes) |> ignore
    base.Field(fun f -> f.Comment) |> ignore
    base.Field(fun f -> f.CommentTruncated) |> ignore
    base.Field(fun f -> f.CommitId) |> ignore
    base.Field(fun f -> f.Committer) |> ignore
    base.Field(fun f -> f.Parents) |> ignore
    base.Field(fun f -> f.Push) |> ignore
    base.Field(fun f -> f.RemoteUrl) |> ignore
    base.Field(expression = (fun f -> f.Statuses), nullable = true) |> ignore
    base.Field(fun f -> f.Url) |> ignore
    base.Field(fun f -> f.WorkItems) |> ignore
    base.FieldAsync<GitRepositoryGraphType, GitRepository>("Repository", resolve = (fun ctx -> 
      let client = ctx.RequestServices.GetRequiredService<IAzureClient>()

      let res = ctx.Source.Links.["repository"]
      client.RepositoryFromPath(res.Href)
    )) |> ignore
        
and GitRepositoryGraphType() =
  inherit ObjectGraphType<GitRepository>()
  do
    base.Field(fun r -> r.Id) |> ignore
    base.Field(fun r -> r.Name) |> ignore
    base.Field(expression = (fun r -> r.DefaultBranch), nullable = true) |> ignore
    base.Field(fun r -> r.Url) |> ignore
    base.Field<AnyScalarGraphType>("Links", resolve = (fun ctx -> ctx.Source.Links :> obj)) |> ignore
    base.Field<ListGraphType<CommitGraphType>, Commit list>()
      .Name("Commits")
      .ResolveAsync(fun ctx -> 
        let client = ctx.RequestServices.GetRequiredService<IAzureClient>()
        if ctx.Source.Links.ContainsKey("commits") then
          client.Commits ctx.Source.Links.["commits"].Href
        else
          null
      ) |> ignore
    base.Field<ListGraphType<PullRequestGraphType>, PullRequest list>()
      .Name("PullRequests")
      .ResolveAsync(fun ctx -> 
        let client = ctx.RequestServices.GetRequiredService<IAzureClient>()
        client.PullRequests ctx.Source.Id
      ) |> ignore

type GitStatusGraphType() =
  inherit ObjectGraphType<GitStatus>()
  do 
    base.Field(fun r -> r.Id) |> ignore
    base.Field(fun r -> r.CreationDate) |> ignore
    base.Field(fun r -> r.Description) |> ignore
    base.Field<AnyScalarGraphType>("Links", resolve = (fun r -> r.Source.Links :> obj)) |> ignore
    base.Field<StringGraphType, string>().Name("State").Resolve(fun ctx -> ctx.Source.State |> DevOpsCentre.Bff.UnionHelpers.toString) |> ignore

type GitTemplateGraphType() =
  inherit ObjectGraphType<GitTemplate>()

  do
    base.Field(fun f -> f.Name) |> ignore
    base.Field(fun f -> f.Type) |> ignore

type GitChangeGraphType() =
  inherit ObjectGraphType<GitChange>()

  do
    base.Field(fun f -> f.ChangeId) |> ignore
    base.Field<StringGraphType, string>("ChangeType").Resolve(fun f -> f.Source.ChangeType |> DevOpsCentre.Bff.UnionHelpers.toString) |> ignore
    base.Field(fun f -> f.Item) |> ignore
    base.Field(fun f -> f.NewContent) |> ignore
    base.Field(fun f -> f.NewContentTemplate) |> ignore
    base.Field(fun f -> f.OriginalPath) |> ignore
    base.Field(fun f -> f.SourceServerItem) |> ignore
    base.Field(fun f -> f.Url) |> ignore

type GitCommitRefGraphType() =
  inherit ObjectGraphType<GitCommitRef>()

  do
    base.Field<AnyScalarGraphType>().Name("Links").Resolve(fun r -> r.Source.Links :> obj) |> ignore
    base.Field(fun f -> f.Author) |> ignore
    base.Field<AnyScalarGraphType>(name = "ChangeCounts", resolve = (fun f -> f.Source.ChangeCounts :> obj)) |> ignore
    base.Field(fun f -> f.Changes) |> ignore
    base.Field(fun f -> f.Comment) |> ignore
    base.Field(fun f -> f.CommentTruncated) |> ignore
    base.Field(fun f -> f.CommitId) |> ignore
    base.Field(fun f -> f.Committer) |> ignore
    base.Field(fun f -> f.Parents) |> ignore
    base.Field(fun f -> f.Push) |> ignore
    base.Field(fun f -> f.RemoteUrl) |> ignore
    base.Field(expression = (fun f -> f.Statuses), nullable = true) |> ignore
    base.Field(fun f -> f.Url) |> ignore
    base.Field(fun f -> f.WorkItems) |> ignore