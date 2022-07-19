namespace DevOpsCentre.Shared.Azure.Domain

open System
open System.Collections.Generic

type ResourceRef =
  {
    Id: string
    Url: string
  }

type ItemContentType = 
    {
        Base64Encoded: string
        RawText: string
    }
type ItemContent =
    {
        Content: string
        ContentType: ItemContentType  
    }

type AuthorInfo =
  {
    Name: string
    Email: string
    Date: DateTimeOffset
  }

type TeamProjectCollectionReference =
  {
    Id: string
    Name: string
    Url: string
  }

type ProjectState =
  | All
  | CreatePending
  | Deleted
  | Deleting
  | New
  | Unchanged
  | WellFormed

type ProjectVisibility =
  | Private
  | Public
  | Unchanged

type TeamProjectReference =
  {
    Abbreviation: string
    DefaultTeamImageUrl: string
    Description: string
    Id: string
    LastUpdateTime: string
    Name: string
    Revision: int
    State: ProjectState
    Url: string
    Visibility: ProjectVisibility
  }