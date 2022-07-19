namespace DevOpsCentre.Bff.Azure.Graph

open DevOpsCentre.Shared.Azure.Domain

open GraphQL
open GraphQL.Types
open GraphQL.Utilities.Federation

type ResourceRefGraphType() =
  inherit ObjectGraphType<ResourceRef>()

  do
    base.Field(fun f -> f.Id) |> ignore
    base.Field(fun f -> f.Url) |> ignore

type ItemContentTypeGraphType() = 
  inherit ObjectGraphType<ItemContentType>()

  do
    base.Field(fun f -> f.Base64Encoded) |> ignore
    base.Field(fun f -> f.RawText) |> ignore

type ItemContentGraphType() =
  inherit ObjectGraphType<ItemContent>()

  do
    base.Field(fun f -> f.Content) |> ignore
    base.Field(fun f -> f.ContentType) |> ignore
  
type AuthorInfoGraphType() =
  inherit ObjectGraphType<AuthorInfo>()
  do
    base.Field(fun r -> r.Name) |> ignore
    base.Field(fun r -> r.Email) |> ignore
    base.Field(fun r -> r.Date) |> ignore

// todo: MISSING: TeamProjectCollectionReference
// todo: MISSING: TeamProjectReference