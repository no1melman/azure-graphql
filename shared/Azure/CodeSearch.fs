namespace DevOpsCentre.Shared.Azure.Domain

open System.Text.Json.Serialization

type CodeSearchCollection = { Name: string }
type CodeSearchProjectRepositoryRef = { Name: string; Id: string }
type VersionControlType = { Custom: string; Git: string; Tfvc: string }

type CodeResult = 
  {
    ContentId: string
    FileName: string
    Matches: obj
    Path: string
    Project: CodeSearchProjectRepositoryRef
    Repository: CodeSearchProjectRepositoryRef
    Collection: CodeSearchCollection
    
  }

type CodeSearchResponse =
  {
    Count: int
    InfoCode: int
    Results: CodeResult[]
  }

type CodeSearchRequest =
  {
    [<JsonPropertyName("$top")>]
    Top: int
    [<JsonPropertyName("$skip")>]
    Skip: int
    SearchText: string
    IncludeSnippet: bool
  }
  static member Default (searchTerm: string) =
    {
      Top = 1000
      Skip = 0
      SearchText = searchTerm
      IncludeSnippet = true
    }
    
type AllCodeSearchResult =
  {
    Results: CodeResult list
    RepositoryCount: int
  }

