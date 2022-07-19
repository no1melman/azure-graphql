module JsonConfiguration

open DevOpsCentre.Bff
open DevOpsCentre.Shared.Azure.Domain

open Microsoft.Extensions.Logging
open System.Text.Json

let options (logger: ILogger) = 
  let theOptions = JsonSerializerOptions()
  theOptions.PropertyNameCaseInsensitive <- true
  theOptions.PropertyNamingPolicy <- JsonNamingPolicy.CamelCase
  theOptions.Converters.Add(DUConverter<BuildReason>(logger))
  theOptions.Converters.Add(DUConverter<BuildStatus>(logger))
  theOptions.Converters.Add(DUConverter<BuildResult>(logger))
  theOptions.Converters.Add(DUConverter<ValidationResult>(logger))
  theOptions.Converters.Add(DUConverter<GitStatusState>(logger))
  theOptions.Converters.Add(DUConverter<VersionControlChangeType>(logger))
  theOptions.Converters.Add(DUConverter<GitPullRequestMergeStrategy>(logger))
  theOptions.Converters.Add(DUConverter<ProjectState>(logger))
  theOptions.Converters.Add(DUConverter<ProjectVisibility>(logger))
  theOptions.Converters.Add(DUConverter<WebApiTagDefinition>(logger))
  theOptions.Converters.Add(DUConverter<PullRequestMergeFailureType>(logger))
  theOptions.Converters.Add(DUConverter<PullRequestAsyncStatus>(logger))
  theOptions.Converters.Add(DUConverter<PullRequestStatus>(logger))
  theOptions.Converters.Add(OptionConverterFactory(logger))
  theOptions

let extend (opts: JsonSerializerOptions) =
  opts.WriteIndented <- true
  opts.MaxDepth <- 10
  opts
