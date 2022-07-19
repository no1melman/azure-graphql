namespace DevOpsCentre.Bff.Azure.Graph

open DevOpsCentre.Shared.Azure.Domain

open GraphQL
open GraphQL.Types
open GraphQL.Utilities.Federation

type IdentityRefGraphType() =
  inherit ObjectGraphType<IdentityRef>() 
  do
    base.Field<AnyScalarGraphType>("Links", resolve = (fun r -> r.Source.Links :> obj)) |> ignore
    base.Field(fun r -> r.Descriptor) |> ignore
    base.Field(fun r -> r.DisplayName) |> ignore
    base.Field(fun r -> r.Id) |> ignore
    base.Field(fun r -> r.IsDeletedInOrigin) |> ignore
    base.Field(fun r -> r.Url) |> ignore

type PipelineGraphType() =
  inherit ObjectGraphType<Pipeline>()
  do
    base.Field(fun r -> r.Id) |> ignore
    base.Field(fun r -> r.Name) |> ignore
    base.Field(fun r -> r.Revision) |> ignore
    base.Field(fun r -> r.Url) |> ignore
    base.Field<AnyScalarGraphType>("Links", resolve = (fun ctx -> ctx.Source.Links :> obj)) |> ignore