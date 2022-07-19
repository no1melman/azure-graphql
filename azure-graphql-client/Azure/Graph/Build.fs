namespace DevOpsCentre.Bff.Azure.Graph

open DevOpsCentre.Shared.Azure.Domain

open GraphQL
open GraphQL.Types

open GraphQL.Utilities.Federation
open GraphQL.Utilities

type AgentSpecificationGraphType() =
  inherit ObjectGraphType<AgentSpecification>()

  do
    base.Field(fun f -> f.Identifier) |> ignore

type BuildLogReferenceGraphType() =
  inherit ObjectGraphType<BuildLogReference>()

  do
    base.Field(fun f -> f.Id) |> ignore
    base.Field(fun f -> f.Type) |> ignore
    base.Field(fun f -> f.Url) |> ignore

type TaskOrchestrationPlanReferenceGraphType() =
  inherit ObjectGraphType<TaskOrchestrationPlanReference>()

  do 
    base.Field(fun f -> f.PlanId) |> ignore
    base.Field(fun f -> f.OrchestrationType) |> ignore

type BuildRepositoryGraphType() =
  inherit ObjectGraphType<BuildRepository>()

  do
    base.Field(fun f -> f.Name) |> ignore
    base.Field(fun f -> f.RootFolder) |> ignore
    base.Field(fun f -> f.Type) |> ignore
    base.Field(fun f -> f.Url) |> ignore
    base.Field<AnyScalarGraphType>()
      .Name("Properties")
      .Resolve(fun ctx -> ctx.Source.Properties :> obj) |> ignore

type BuildRequestValidationResultGraphType() =
  inherit ObjectGraphType<BuildRequestValidationResult>()

  do
    base.Field(fun f -> f.Message) |> ignore
    base.Field<StringGraphType, string>()
      .Name("Result")
      .Resolve(fun f -> f.Source.Result |> DevOpsCentre.Bff.UnionHelpers.toString) |> ignore

type BuildGraphType() =
  inherit ObjectGraphType<Build>()

  do
    base.Field(fun r -> r.Id) |> ignore
    base.Field(fun r -> r.AgentSpecification) |> ignore
    base.Field(fun r -> r.BuildNumber) |> ignore
    base.Field(fun r -> r.BuildNumberRevision) |> ignore
    base.Field(fun r -> r.Deleted) |> ignore
    base.Field(fun r -> r.DeletedBy) |> ignore
    base.Field(fun r -> r.DeletedDate) |> ignore
    base.Field(fun r -> r.DeletedReason) |> ignore
    base.Field<DateTimeGraphType>("FinishTime", resolve = (fun r -> 
            match r.Source.FinishTime with 
            | Some d -> d :> obj
            | Option.None -> null
        )) |> ignore
    base.Field(fun r -> r.KeepForever) |> ignore
    base.Field(fun r -> r.LastChangedBy) |> ignore
    base.Field(fun r -> r.LastChangedDate) |> ignore
    base.Field(fun r -> r.Logs) |> ignore
    base.Field(fun r -> r.OrchestrationPlan) |> ignore
    base.Field(fun r -> r.Parameters) |> ignore
    base.Field(fun r -> r.Plans) |> ignore
    base.Field(fun r -> r.Repository) |> ignore
    base.Field(fun r -> r.RequestedBy) |> ignore
    base.Field(fun r -> r.RequestedFor) |> ignore
    base.Field(fun r -> r.Quality) |> ignore
    base.Field(fun r -> r.QueuePosition) |> ignore
    base.Field(fun r -> r.QueueTime) |> ignore
    base.Field(fun r -> r.RetainedByRelease) |> ignore
    base.Field(fun r -> r.SourceBranch) |> ignore
    base.Field(fun r -> r.SourceVersion) |> ignore
    base.Field(fun r -> r.Tags) |> ignore
    base.Field(fun r -> r.TriggeredByBuild) |> ignore
    base.Field(fun r -> r.Uri) |> ignore
    base.Field(fun r -> r.Url) |> ignore
    base.Field(fun r -> r.ValidationResults) |> ignore
    base.Field(expression = (fun r -> r.StartTime), nullable = true) |> ignore
    base.Field<AnyScalarGraphType>().Name("Links").Resolve(fun r -> r.Source.Links :> obj) |> ignore
    base.Field<StringGraphType, string>().Name("Reason").Resolve(fun r -> r.Source.Reason |> DevOpsCentre.Bff.UnionHelpers.toString) |> ignore
    base.Field<StringGraphType, string>().Name("Result").Resolve(fun r -> r.Source.Result |> DevOpsCentre.Bff.UnionHelpers.toString) |> ignore
    base.Field<StringGraphType, string>().Name("Status").Resolve(fun r -> r.Source.Status |> DevOpsCentre.Bff.UnionHelpers.toString) |> ignore
    base.Field<AnyScalarGraphType>().Name("TemplateParameters").Resolve(fun r -> r.Source.TemplateParameters :> obj) |> ignore
    base.Field<AnyScalarGraphType>().Name("TriggerInfo").Resolve(fun r -> r.Source.TriggerInfo :> obj) |> ignore

module BuildsGraphTypes =
  let initialiseTypes(schema: ISchema) =
    schema.RegisterTypeMapping<AgentSpecification, AgentSpecificationGraphType>() |> ignore
    schema.RegisterTypeMapping<BuildRepository, BuildRepositoryGraphType>() |> ignore
    schema.RegisterTypeMapping<TaskOrchestrationPlanReference, TaskOrchestrationPlanReferenceGraphType>() |> ignore
    schema.RegisterTypeMapping<BuildLogReference, BuildLogReferenceGraphType>() |> ignore
    schema.RegisterTypeMapping<BuildRequestValidationResult, BuildRequestValidationResultGraphType>() |> ignore
    schema.RegisterTypeMapping<Build, BuildGraphType>() |> ignore