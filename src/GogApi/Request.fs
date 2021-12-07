namespace GogApi.Internal

open GogApi.DomainTypes

open FSharp.Json
open FsHttp
open FsHttp.DslCE

/// This module contains low-level functions and types to make requests to the GOG API
module Request =
    /// Used config for parsing the JSON API responses
    let jsonConfig = { JsonConfig.Default with allowUntyped = true }

    /// Simple record for request parameters
    type RequestParameter = { name: string; value: string }

    ///<summary>Creates a RequestParameter</summary>
    ///<returns>A list to simplify using of optional RequestParameters</returns>
    ///<example><code lang="fsharp">
    ///let queries =
    ///    [ createOptionalRequestParameter "feature" (request.feature |> Option.map GameFeature.toString)
    ///      createOptionalRequestParameter "language" (request.language |> Option.map Language.toString)
    ///      createRequestParameter "mediaType" "1" ]
    ///    |> List.concat
    ///</code></example>
    let createRequestParameter name value = [ { name = name; value = value } ]

    ///<summary>Creates an optional RequestParameter</summary>
    ///<returns>An empty list, if provided parameter is None and the parameter otherwise</returns>
    ///<example><code lang="fsharp">
    ///let queries =
    ///    [ createOptionalRequestParameter "feature" (request.feature |> Option.map GameFeature.toString)
    ///      createOptionalRequestParameter "language" (request.language |> Option.map Language.toString)
    ///      createRequestParameter "mediaType" "1" ]
    ///    |> List.concat
    ///</code></example>
    let createOptionalRequestParameter name valueOption =
        match valueOption with
        | Some value -> createRequestParameter name value
        | None -> []

    ///<summary>Creates the GET request with correct authentication headers and
    ///parameters to given url</summary>
    ///<returns>An Async which can be executed to send the request</returns>
    let setupRequest authentication parameters url =
        // Add parameters to request url
        let url =
            match parameters with
            | [] -> url
            | queries ->
                let parameters =
                    queries
                    |> List.map (fun param -> param.name + "=" + param.value)
                    |> List.reduce (fun param1 param2 -> param1 + "&" + param2)

                url + "?" + parameters
        // Headerpart which is always used - with authentication and without it
        let baseHeader =
            httpLazy {
                GET url
                CacheControl "no-cache"
            }
        // Extend request header with authentication info if available
        let request =
            match authentication with
            | Some { accessToken = token } -> baseHeader { AuthorizationBearer token }
            | None -> baseHeader

        request |> Request.sendAsync

    ///<summary>Helper function which catches exception from FSharp.Json and
    /// returns Result type</summary>
    ///<returns>Error when exception occured, otherwise Ok with parsed object</returns>
    let parseJson<'T> rawJson =
        let parsedJson =
            try
                Json.deserializeEx<'T> jsonConfig rawJson |> Ok
            with
            | ex -> Error(rawJson, ex.ToString())

        parsedJson

    ///<summary>Function which creates an request which will be parsed into an
    /// object after returning</summary>
    ///<returns>An Async which can be executed to send the request and parse the answer</returns>
    let makeRequest<'T> authentication parameters url =
        async {
            let! response = setupRequest authentication parameters url

            let message = response |> Response.toText
            return parseJson<'T> message
        }
