namespace DevOpsCentre.Shared.Azure.Domain

open System.Collections.Generic
open System.Text.Json.Serialization

type LinksInfo = { Href: string }

type LinksSection = 
  {
    Commits: LinksInfo
  }

type IdentityRef = 
  {
    Links: Dictionary<string, LinksInfo>
    Descriptor: string
    DisplayName: string
    Id: string
    IsDeletedInOrigin: bool
    Url: string
  }

type Pipeline =
  {
    Id: int
    Folder: string
    Name: string
    Revision: int
    Url: string
    [<JsonPropertyName("_links")>]
    Links: Dictionary<string, LinksInfo>
  }

type VersionControlChangeType =
  | Add
  | All
  | Branch
  | Delete
  | Edit
  | Encoding
  | Lock
  | Merge
  | None
  | Property
  | Rename
  | Rollback
  | SourceRename
  | TargetRename
  | Undelete
