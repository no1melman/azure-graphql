namespace DevOpsCentre.Bff.Azure.Graph

open GraphQL.Types

module TypeInitialiser =
    let initialiseTypes(schema: ISchema) =
        typedefof<DevOpsSchema>.Assembly.GetTypes()
        |> List.ofArray 
        |> List.filter (fun t -> 
            let igraphtype = typedefof<IGraphType>
            t.IsClass 
                && not t.IsAbstract 
                && igraphtype.IsAssignableFrom(t)
                && not (t.Name = "DevOpsQuery")
        ) |> List.iter (fun t -> 
            let b = t.BaseType
            let gc = b.GenericTypeArguments.[0]
            schema.RegisterTypeMapping(gc, t) |> ignore
        )