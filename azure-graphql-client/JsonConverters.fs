namespace DevOpsCentre.Bff

open System
open System.Text.Json
open System.Text.Json.Serialization
open Microsoft.FSharp.Reflection

open Microsoft.Extensions.Logging

module UnionHelpers =
  let toString<'T> (value: 'T) =
    let (case, _) =
      FSharpValue.GetUnionFields(value, typeof<'T>)

    case.Name.ToLowerInvariant()

type DUConverter<'T>(logger: ILogger) =
  inherit JsonConverter<'T>()

  do
    logger.LogDebug($"Register DU :: {(typedefof<'T>.Name)}")

  override _.CanConvert(``type``: System.Type) =
    let canConvert = FSharpType.IsUnion(``type``) && ``type``.Name = typedefof<'T>.Name
    if canConvert then logger.LogDebug($"Can Convert DU :: {(``type``.Name)} :: {canConvert} :: {typedefof<'T>.Name}") |> ignore
    canConvert

  override _.Read(reader: byref<Utf8JsonReader>, typeToConvert: System.Type, options: JsonSerializerOptions): 'T =
    logger.LogDebug($"Beginning :: {typeToConvert.Name} :: {(typedefof<'T>.Name)}") |> ignore
    match reader.TokenType with
    | JsonTokenType.String ->
      let s = reader.GetString().ToLowerInvariant()
      logger.LogDebug($"Deserializing {s} :: {(typedefof<'T>.Name)}") |> ignore
      match FSharpType.GetUnionCases typeof<'T>
            |> Array.filter (fun case -> case.Name.ToLowerInvariant() = s) with
      | [| case |] -> FSharpValue.MakeUnion(case, [||]) :?> 'T
      | _ -> invalidOp ($"{s} is not a valid union case of {typedefof<'T>.Name}")
    | _ -> invalidOp ($"Trying to conver the wrong Json Token Type for union case {typedefof<'T>.Name}")

  override _.Write(writer: Utf8JsonWriter, value: 'T, options: JsonSerializerOptions) =
    writer.WriteStringValue(UnionHelpers.toString value)
    |> ignore

type OptionConverter<'T>(logger: ILogger) =
    inherit JsonConverter<'T option>()

    let optionGenericTypeDef = (Some 1).GetType().GetGenericTypeDefinition()
    let getGenericTypeDef (typedef: System.Type) = typedef.GetGenericTypeDefinition()

    override _.CanConvert(``type``: System.Type) =
        if ``type``.IsGenericType then getGenericTypeDef ``type`` = optionGenericTypeDef
        else false

    override _.Read(reader: byref<Utf8JsonReader>, typeToConvert, options): 'T option =
        match reader.TokenType with
        | JsonTokenType.Null -> None
        | _ -> 
            // Don't do this, cause stack overflow... 
            // Some (JsonSerializer.Deserialize(&reader, typeToConvert, options) :?> 'T)
            let a = reader.GetString()
            if isNull a then None
            else
                Some (JsonSerializer.Deserialize(&reader, typedefof<'T>, options) :?> 'T)

    override _.Write(writer, value, options) =
        match value with
        | Some a -> writer.WriteRawValue(JsonSerializer.SerializeToUtf8Bytes(a, options))
        | None -> writer.WriteNullValue()

type OptionConverterFactory(logger: ILogger) =
    inherit JsonConverterFactory()

    let optionGenericTypeDef = (Some 1).GetType().GetGenericTypeDefinition()
    let getGenericTypeDef (typedef: System.Type) = typedef.GetGenericTypeDefinition()

    override _.CanConvert(``type``: System.Type) =
        if ``type``.IsGenericType then getGenericTypeDef ``type`` = optionGenericTypeDef
        else false

    override _.CreateConverter(typeToCreate, options) =
        if typeToCreate.IsGenericType && typeToCreate.GetGenericTypeDefinition() = optionGenericTypeDef then
            let innerType = typeToCreate.GenericTypeArguments.[0]
            let ocGenericType = typedefof<OptionConverter<_>>.GetGenericTypeDefinition()
            let closedType = ocGenericType.MakeGenericType(innerType)
            Activator.CreateInstance(closedType, logger) :?> JsonConverter
        else
            null