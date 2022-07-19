namespace DevOpsCentre.Shared.Azure.Domain

open System.Collections.Generic
open System.Text.Json.Serialization


type GitRef =
  {
    [<JsonPropertyName("_links")>]
    Links: Dictionary<string, LinksInfo>
    Creator: IdentityRef
    IsLocked: bool
    IsLockedBy: IdentityRef
    Name: string
    ObjectId: string
    PeeledObjectId: string
    Statuses: GitStatus[]
    Url: string
  }

type GitStatusContext =
  {
    Genre: string
    Name: string
  }