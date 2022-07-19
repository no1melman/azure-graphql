namespace DevOpsCentre.Shared.Azure.Domain

open System.Collections.Generic
open System.Text.Json.Serialization

type GitPullRequestMergeStrategy =
  | NoFastForward
  | Rebase
  | RebaseMerge
  | Squash

type GitPullRequestCompletionOptions =
  {
    AutoCompleteIgnoreConfigIds: int[]
    BypassPolicy: bool
    BypassReason: string
    DeleteSourceBranch: bool
    MergeCommitMessage: string
    MergeStrategy: GitPullRequestMergeStrategy
    SquashMerge: bool
    TransitionWorkItems: bool
    TriggeredByAutoComplete: bool
  }

type GitForkRef =
  {
    [<JsonPropertyName("_links")>]
    Links: Dictionary<string, LinksInfo>
    Creator: IdentityRef
    IsLocked: bool
    IsLockedBy: IdentityRef
    Name: string
    ObjectId: string
    PeeledObjectId: string
    Repository: GitRepository
    Statuses: GitStatus[]
    Url: string
  }

type WebApiTagDefinition =
  {
    Active: bool // Whether or not the tag definition is active.
    Id: string //ID of the tag definition.
    Name: string //The name of the tag definition.
    Url: string //Resource URL for the Tag Definition.
  }

type PullRequestMergeFailureType =
  | CaseSensitive
  | None
  | ObjectTooLarge
  | Unknown

type GitPullRequestMergeOptions =
  {
    ConflictAuthorshipCommits: bool // If true, conflict resolutions applied during the merge will be put in separate commits to preserve authorship info for git blame, etc.
    DetectRenameFalsePositives: bool     
    DisableRenames: bool // If true, rename detection will not be performed during the merge.
  }

type PullRequestAsyncStatus =
  | Conflicts
  | Failure
  | NotSet
  | Queued
  | RejectedByPolicy
  | Succeeded

type IdentityRefWithVote =
  {
    [<JsonPropertyName("_links")>]
    Links: Dictionary<string, LinksInfo>
    Descriptor: string // The descriptor is the primary way to reference the graph subject while the system is running. This field will uniquely identify the same graph subject across both Accounts and Organizations.
    //DirectoryAlias: string // Deprecated - Can be retrieved by querying the Graph user referenced in the "self" entry of the IdentityRef "_links" dictionary
    DisplayName: string // This is the non-unique display name of the graph subject. To change this field, you must alter its value in the source provider.
    HasDeclined: bool // Indicates if this reviewer has declined to review this pull request.
    Id: string
    //ImageUrl: string // Deprecated - Available in the "avatar" entry of the IdentityRef "_links" dictionary
    //Inactive: bool // Deprecated - Can be retrieved by querying the Graph membership state referenced in the "membershipState" entry of the GraphUser "_links" dictionary
    //IsAadIdentity: bool // Deprecated - Can be inferred from the subject type of the descriptor (Descriptor.IsAadUserType/Descriptor.IsAadGroupType)
    //IsContainer: bool // Deprecated - Can be inferred from the subject type of the descriptor (Descriptor.IsGroupType)
    IsDeletedInOrigin: bool
    IsFlagged: bool // Indicates if this reviewer is flagged for attention on this pull request.
    IsRequired: bool // Indicates if this is a required reviewer for this pull request. Branches can have policies that require particular reviewers are required for pull requests.
    //ProfileUrl: string // Deprecated - not in use in most preexisting implementations of ToIdentityRef
    ReviewerUrl: string // URL to retrieve information about this identity
    //UniqueName: string // Deprecated - use Domain+PrincipalName instead
    Url: string // This url is the full route to the source resource of this graph subject.
    Vote: int // Vote on a pull request: 10 - approved 5 - approved with suggestions 0 - no vote -5 - waiting for author -10 - rejected
    VotedFor: IdentityRefWithVote[] // Groups or teams that that this reviewer contributed to. Groups and teams can be reviewers on pull requests but can not vote directly. When a member of the group or team votes, that vote is rolled up into the group or team vote. VotedFor is a list of such votes.
  }

type PullRequestStatus =
  | Abandoned
  | Active
  | All
  | Completed
  | NotSet

type PullRequest = 
  {
    [<JsonPropertyName("_links")>]
    Links: Dictionary<string, LinksInfo>
    ArtifactId: string
    AutoCompleteSetBy: IdentityRef
    ClosedBy: IdentityRef
    ClosedDate: string
    CodeReviewId: int
    Commits: GitCommitRef[]
    CompletionOptions: GitPullRequestCompletionOptions
    CompletionQueueTime: string
    CreatedBy: IdentityRef
    CreationDate: string
    Description: string
    ForkSource: GitForkRef
    IsDraft: bool
    Labels: WebApiTagDefinition[]
    LastMergeCommit: GitCommitRef
    LastMergeSourceCommit: GitCommitRef
    LastMergeTargetCommit: GitCommitRef
    MergeFailureMessage: string
    MergeFailureType: PullRequestMergeFailureType
    MergeId: string
    MergeOptions: GitPullRequestMergeOptions
    MergeStatus: PullRequestAsyncStatus
    PullRequestId: int
    RemoteUrl: string
    Repository: GitRepository
    Reviewers: IdentityRefWithVote[]
    SourceRefName: string
    Status: PullRequestStatus
    SupportsIterations: bool
    TargetRefName: string
    Title: string
    Url: string
    WorkItemRefs: ResourceRef[]
  }