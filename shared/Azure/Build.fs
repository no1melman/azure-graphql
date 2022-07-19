namespace DevOpsCentre.Shared.Azure.Domain

open System.Collections.Generic
open System.Text.Json.Serialization

type BuildStatus = 
  | All
  | Cancelling
  | Completed
  | InProgress
  | NotStarted
  | Postponed
  | None

type BuildReason =
  | All
  | BatchedCI
  | BuildCompletion
  | CheckInShelveset
  | IndividualCI
  | Manual
  | None
  | PullRequest
  | ResourceTrigger
  | Schedule
  | ScheduleForced
  | Triggered
  | UserCreated
  | ValidateShelveset

type AgentSpecification =
  {
    Identifier: string
  }

type BuildLogReference =
  {
    Id: int
    Type: string
    Url: string
  }

type TaskOrchestrationPlanReference =
  {
    OrchestrationType: int
    PlanId: string
  }

type BuildRepository =
  {
    CheckoutSubmodules: bool
    Clean: string
    DefaultBranch: string
    Id: string
    Name: string
    Properties: Dictionary<string, string>
    RootFolder: string
    Type: string
    Url: string
  }

type BuildResult =
  | Canceled
  | Failed
  | None
  | PartiallySucceeded
  | Succeeded

type ValidationResult = 
  | Error
  | Ok
  | Warning

type BuildRequestValidationResult =
  {
    Message: string
    Result: ValidationResult
  }

type Build = 
  {
    [<JsonPropertyName("_links")>]
    Links: Dictionary<string, LinksInfo>
    AgentSpecification: AgentSpecification
    BuildNumber: string
    BuildNumberRevision: int
    // Controller: BuildController // only for xaml
    // Definition: DefinitionReference
    Deleted: bool
    DeletedBy: IdentityRef
    DeletedDate: string
    DeletedReason: string
    // Demands: Demand[]
    FinishTime: System.DateTime option
    Id: int
    KeepForever: bool
    LastChangedBy: IdentityRef
    LastChangedDate: string
    Logs: BuildLogReference
    OrchestrationPlan: TaskOrchestrationPlanReference
    Parameters: string
    Plans: TaskOrchestrationPlanReference[]
    // Priority: QueuePriority
    // Project: TeamProjectReference
    //Properties: PropertiesCollection
    Quality: string
    //Queue: AgentPoolQueue
    //QueueOptions: QueueOptions
    QueuePosition: int
    QueueTime: string
    Reason: BuildReason
    Repository: BuildRepository
    RequestedBy: IdentityRef
    RequestedFor: IdentityRef
    Result: BuildResult
    RetainedByRelease: bool
    SourceBranch: string
    SourceVersion: string
    StartTime: System.DateTime
    Status: BuildStatus
    Tags: string[]
    TemplateParameters: Dictionary<string, string>
    TriggerInfo: Dictionary<string, string>
    TriggeredByBuild: Build
    Uri: string
    Url: string
    ValidationResults: BuildRequestValidationResult[]
  }

