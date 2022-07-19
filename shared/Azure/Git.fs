namespace DevOpsCentre.Shared.Azure.Domain

open System
open System.Collections.Generic
open System.Text.Json.Serialization

type GitUserDate =
  {
    Date: string
    Email: string
    ImageUrl: string
    Name: string
  }

type GitPushRef =
    {
        Links: Dictionary<string, LinksInfo>
        Date: DateTimeOffset
        PushId: int
        PushedBy: IdentityRef
        Url: string
    }

type GitRepositoryRef =
    {
        Collection: TeamProjectCollectionReference
        Id: string
        IsFork: bool
        Name: string
        Project: TeamProjectReference
        RemoteUrl: string
        SshUrl: string
        Url: string
    }

type GitRepository = 
    {
        Id: string
        Name: string
        DefaultBranch: string
        Url: string
        [<JsonPropertyName("_links")>]
        Links: Dictionary<string, LinksInfo>
        IsFork: bool
        ParentRepository: GitRepositoryRef
        Project: TeamProjectReference
        RemoteUrl: string
        Size: int64
        SshUrl: string
        ValidRemoteUrls: string[]
        WebUrl: string
    }

type GitStatusState = Error | Failed | NotApplicable | NotSet | Pending | Succeeded

type GitStatus = 
    {
        [<JsonPropertyName("_links")>]
        Links: Dictionary<string, LinksInfo>
        CreationDate: DateTimeOffset
        Description: string
        Id: int
        State: GitStatusState
    }

type GitTemplate =
  {
    Name: string
    Type: string
  }

type GitChange =
  {
    ChangeId: int
    ChangeType: VersionControlChangeType
    Item: string
    NewContent: ItemContent
    NewContentTemplate: GitTemplate
    OriginalPath: string
    SourceServerItem: string
    Url: string
  }


type GitCommitRef =
  {
    [<JsonPropertyName("_links")>]
    Links: Dictionary<string, LinksInfo>
    Author: GitUserDate
    ChangeCounts: Dictionary<string, obj> // possibly a GitChange
    Changes: GitChange[]
    Comment: string
    CommentTruncated: bool
    CommitId: string
    Committer: GitUserDate
    Parents: string[]
    Push: GitPushRef
    RemoteUrl: string
    Statuses: GitStatus[]
    Url: string
    WorkItems: ResourceRef[]
  }

type Commit = 
  {
    [<JsonPropertyName("_links")>]
    Links: Dictionary<string, LinksInfo>
    Author: GitUserDate
    ChangeCounts: Dictionary<string, obj> // possibly a GitChange
    Changes: GitChange[]
    Comment: string
    CommentTruncated: bool
    CommitId: string
    Committer: GitUserDate
    Parents: string[]
    Push: GitPushRef
    RemoteUrl: string
    Statuses: GitStatus[]
    Url: string
    WorkItems: ResourceRef[]
    TreeId: string
  }