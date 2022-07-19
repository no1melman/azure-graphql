namespace DevOpsCentre.Bff

open System
open System.Collections.Generic
open System.IO
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Builder

open GraphQL.Server
open GraphQL.Types
open GraphQL.MicrosoftDI

open DevOpsCentre.Bff.Azure.Graph
open DevOpsCentre.Bff.Azure.Client.Application
open DevOpsCentre.Bff.Azure.Infrastructure
open DevOpsCentre.Bff.Azure.Graph

module Program =

    [<EntryPoint>]
    let main args =
        let builder = WebApplication.CreateBuilder(args)

        builder.Services.AddLogging() |> ignore
        
        builder.Services.AddSingleton<ISchema, DevOpsSchema>(fun services -> new DevOpsSchema(SelfActivatingServiceProvider(services))) |> ignore
        builder.Services.AddSingleton<DevOpsQuery>() |> ignore
        
        typedefof<DevOpsSchema>.Assembly.GetTypes()
        |> List.ofArray |> List.filter (fun t -> 
            let igraphtype = typedefof<IGraphType>
            t.IsClass 
                && not t.IsAbstract 
                && igraphtype.IsAssignableFrom(t)
        ) |> List.iter (fun t -> builder.Services.AddTransient(t) |> ignore)
        
        builder.Services
            .AddGraphQL(fun options ->
                options.EnableMetrics <- true
            )
            .AddGraphTypes(typedefof<DevOpsSchema>.Assembly)
            .AddSystemTextJson()
            .AddErrorInfoProvider(fun opt -> opt.ExposeExceptionStackTrace <- true) |> ignore
            // .AddGraphTypes(typedefof<DevOpsSchema>) |> ignore
        
        builder.Services.AddTransient<IAzureClient, AzureClient>() |> ignore
        builder.Services.AddHttpClient("azure") |> ignore

        let app = builder.Build()

        let schema = app.Services.GetRequiredService<ISchema>()
        TypeInitialiser.initialiseTypes(schema)

        if app.Environment.IsDevelopment() then
            app.UseDeveloperExceptionPage() |> ignore

        // add http for Schema at default url /graphql
        app.UseGraphQL<ISchema>() |> ignore

        // use graphql-playground at default url /ui/playground
        app.UseGraphQLPlayground() |> ignore

        app.Run()

        0 // Exit code
