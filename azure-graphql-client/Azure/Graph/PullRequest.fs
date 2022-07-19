namespace DevOpsCentre.Bff.Azure.Graph

open DevOpsCentre.Shared.Azure.Domain

open GraphQL
open GraphQL.Types

open GraphQL.Utilities.Federation

type GitPullRequestCompletionOptionsGraphType() =
  inherit ObjectGraphType<GitPullRequestCompletionOptions>()

  do
    base.Field(fun f -> f.AutoCompleteIgnoreConfigIds) |> ignore
    base.Field(fun f -> f.BypassPolicy) |> ignore
    base.Field(fun f -> f.BypassReason) |> ignore
    base.Field(fun f -> f.DeleteSourceBranch) |> ignore
    base.Field(fun f -> f.MergeCommitMessage) |> ignore
    base.Field<StringGraphType, string>("MergeStrategy").Resolve(fun f -> f.Source.MergeStrategy |> DevOpsCentre.Bff.UnionHelpers.toString) |> ignore
    base.Field(fun f -> f.SquashMerge) |> ignore
    base.Field(fun f -> f.TransitionWorkItems) |> ignore
    base.Field(fun f -> f.TriggeredByAutoComplete) |> ignore

type GitForkRefGraphType() =
  inherit ObjectGraphType<GitForkRef>()

  do
    base.Field<AnyScalarGraphType>().Name("Links").Resolve(fun r -> r.Source.Links :> obj) |> ignore |> ignore
    base.Field(fun f -> f.Creator) |> ignore
    base.Field(fun f -> f.IsLocked) |> ignore
    base.Field(fun f -> f.IsLockedBy) |> ignore
    base.Field(fun f -> f.Name) |> ignore
    base.Field(fun f -> f.ObjectId) |> ignore
    base.Field(fun f -> f.PeeledObjectId) |> ignore
    base.Field(fun f -> f.Repository) |> ignore
    base.Field(fun f -> f.Statuses) |> ignore
    base.Field(fun f -> f.Url) |> ignore

type WebApiTagDefinitionGraphType() =
    inherit ObjectGraphType<WebApiTagDefinition>()

    do
      base.Field(fun f -> f.Active) |> ignore
      base.Field(fun f -> f.Id) |> ignore
      base.Field(fun f -> f.Name) |> ignore
      base.Field(fun f -> f.Url) |> ignore

type GitPullRequestMergeOptionsGraphType() =
  inherit ObjectGraphType<GitPullRequestMergeOptions>()

  do
    base.Field(fun f -> f.ConflictAuthorshipCommits) |> ignore // If true, conflict resolutions applied during the merge will be put in separate commits to preserve authorship info for git blame, etc.>(fun f -> f.ConflictAuthorshipCommits) 
    base.Field(fun f -> f.DetectRenameFalsePositives) |> ignore
    base.Field(fun f -> f.DisableRenames) |> ignore // If true, rename detection will not be performed during the merge.>(fun f -> f.DisableRenames) 

type IdentityRefWithVoteGraphType() =
    inherit ObjectGraphType<IdentityRefWithVote>()
    
    do
    base.Field<AnyScalarGraphType>().Name("Links").Resolve(fun r -> r.Source.Links :> obj) |> ignore
    base.Field(fun f -> f.Descriptor) |> ignore
    base.Field(fun f -> f.DisplayName) |> ignore
    base.Field(fun f -> f.HasDeclined) |> ignore
    base.Field(fun f -> f.Id) |> ignore
    base.Field(fun f -> f.IsDeletedInOrigin) |> ignore
    base.Field(fun f -> f.IsFlagged) |> ignore
    base.Field(fun f -> f.IsRequired) |> ignore
    base.Field(fun f -> f.ReviewerUrl) |> ignore
    base.Field(fun f -> f.Url) |> ignore
    base.Field(fun f -> f.Vote) |> ignore 
    base.Field(fun f -> f.VotedFor) |> ignore

type PullRequestGraphType() = 
  inherit ObjectGraphType<PullRequest>()

  do
    base.Field<AnyScalarGraphType>().Name("Links").Resolve(fun r -> r.Source.Links :> obj) |> ignore |> ignore
    base.Field(fun f -> f.ArtifactId) |> ignore
    base.Field(fun f -> f.AutoCompleteSetBy) |> ignore
    base.Field(fun f -> f.ClosedBy) |> ignore
    base.Field(fun f -> f.ClosedDate) |> ignore
    base.Field(fun f -> f.CodeReviewId) |> ignore
    base.Field(fun f -> f.Commits) |> ignore
    base.Field(expression = (fun f -> f.CompletionOptions), nullable = true) |> ignore
    base.Field(fun f -> f.CompletionQueueTime) |> ignore
    base.Field(fun f -> f.CreatedBy) |> ignore
    base.Field(fun f -> f.CreationDate) |> ignore
    base.Field(fun f -> f.Description) |> ignore
    base.Field(fun f -> f.ForkSource) |> ignore
    base.Field(fun f -> f.IsDraft) |> ignore
    base.Field(expression = (fun f -> f.Labels), nullable = true) |> ignore
    base.Field(fun f -> f.LastMergeCommit) |> ignore
    base.Field(fun f -> f.LastMergeSourceCommit) |> ignore
    base.Field(fun f -> f.LastMergeTargetCommit) |> ignore
    base.Field(fun f -> f.MergeFailureMessage) |> ignore
    base.Field<StringGraphType, string>("MergeFailureType").Resolve(fun f -> f.Source.MergeFailureType |> DevOpsCentre.Bff.UnionHelpers.toString) |> ignore
    base.Field(fun f -> f.MergeId) |> ignore
    base.Field(fun f -> f.MergeOptions) |> ignore
    base.Field<StringGraphType, string>("MergeStatus").Resolve(fun f -> f.Source.MergeStatus |> DevOpsCentre.Bff.UnionHelpers.toString) |> ignore
    base.Field(fun f -> f.PullRequestId) |> ignore
    base.Field(fun f -> f.RemoteUrl) |> ignore
    base.Field(fun f -> f.Repository) |> ignore
    base.Field(fun f -> f.Reviewers) |> ignore
    base.Field(fun f -> f.SourceRefName) |> ignore
    base.Field<StringGraphType, string>("Status").Resolve(fun f -> f.Source.Status |> DevOpsCentre.Bff.UnionHelpers.toString) |> ignore
    base.Field(fun f -> f.SupportsIterations) |> ignore
    base.Field(fun f -> f.TargetRefName) |> ignore
    base.Field(fun f -> f.Title) |> ignore
    base.Field(fun f -> f.Url) |> ignore
    base.Field(fun f -> f.WorkItemRefs) |> ignore