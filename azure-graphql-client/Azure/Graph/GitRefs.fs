namespace DevOpsCentre.Bff.Azure.Graph

open DevOpsCentre.Shared.Azure.Domain

open GraphQL.Types
open GraphQL.Utilities.Federation

type GitRefGraphType() =
  inherit ObjectGraphType<GitRef>()
  
  do
    base.Field<AnyScalarGraphType>("Links", resolve = (fun r -> r.Source.Links :> obj)) |> ignore
    base.Field(fun f -> f.Creator) |> ignore
    base.Field(fun f -> f.IsLocked) |> ignore
    base.Field(fun f -> f.IsLockedBy) |> ignore
    base.Field(fun f -> f.Name) |> ignore
    base.Field(fun f -> f.ObjectId) |> ignore
    base.Field(fun f -> f.PeeledObjectId) |> ignore
    base.Field(fun f -> f.Statuses) |> ignore
    base.Field(fun f -> f.Url) |> ignore

type GitStatusContextGraphType() =
  inherit ObjectGraphType<GitStatusContext>()
  
  do
    base.Field(fun f -> f.Genre) |> ignore
    base.Field(fun f -> f.Name) |> ignore