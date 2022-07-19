namespace DevOpsCentre.Bff.Azure.Graph

open DevOpsCentre.Shared.Azure.Domain

open GraphQL
open GraphQL.Types

open GraphQL.Utilities.Federation
open GraphQL.Utilities

type CodeSearchCollectionGraphType() =
  inherit ObjectGraphType<CodeSearchCollection>()
  do
    base.Field(fun r -> r.Name) |> ignore

type CodeSearchProjectRepositoryRefGraphType() =
  inherit ObjectGraphType<CodeSearchProjectRepositoryRef>()
  do
    base.Field(fun r -> r.Name) |> ignore
    base.Field(fun r -> r.Id) |> ignore

type VersionControlTypeGraphType() =
  inherit ObjectGraphType<VersionControlType>()
  do
    base.Field(fun r -> r.Custom) |> ignore
    base.Field(fun r -> r.Git) |> ignore
    base.Field(fun r -> r.Tfvc) |> ignore

type CodeResultGraphType() =
  inherit ObjectGraphType<CodeResult>()
  do
    base.Field(fun r -> r.FileName) |> ignore
    base.Field(fun r -> r.Path) |> ignore
    base.Field(fun r -> r.ContentId) |> ignore
    base.Field<AnyScalarGraphType>("Matches",  resolve = (fun r -> r.Source.Matches)) |> ignore
    base.Field(fun r -> r.Project) |> ignore
    base.Field(fun r -> r.Collection) |> ignore
    base.Field(fun r -> r.Repository) |> ignore

type CodeSearchResponseGraphType() =
  inherit ObjectGraphType<CodeSearchResponse>()
  do 
    base.Field(fun r -> r.Count) |> ignore // Expression<Func<Commit, string>> resolver, resolver.Compile().Invoke(ctx.Source)
    base.Field(fun r -> r.Results) |> ignore
    base.Field(fun f -> f.InfoCode) |> ignore

type AllCodeSearchResultGraphType() =
  inherit ObjectGraphType<AllCodeSearchResult>()
  do
    base.Field<ListGraphType<CodeResultGraphType>>("Results", resolve = (fun r -> r.Source.Results :> obj)) |> ignore
    base.Field(fun r -> r.RepositoryCount) |> ignore