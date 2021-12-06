namespace GogApi.Internal

open GogApi.DomainTypes

open FSharp.Json
open FsHttp
open FsHttp.DslCE

/// This module contains low-level functions and types to make requests to the GOG API
module Request =
    open FsHttp.Helper
    /// Used config for parsing the JSON API responses
    let jsonConfig = { JsonConfig.Default with allowUntyped = true }

    /// Simple record for request parameters
    type RequestParameter = { name: string; value: string }

    /// Creates a RequestParameter
    /// Returns a list to simplify using of optional RequestParameters
    ///
    /// Example:
    /// ```
    ///   let queries =
    ///     [ createOptionalRequestParameter "feature"
    ///           (request.feature |> Option.map GameFeature.toString)
    ///       createOptionalRequestParameter "language"
    ///           (request.language |> Option.map Language.toString)
    ///       createRequestParameter "mediaType" "1" ]
    ///     |> List.concat
    /// ```
    let createRequestParameter name value = [ { name = name; value = value } ]

    /// Creates an optional RequestParameter
    /// Returns an empty list, if provided parameter is None and the parameter otherwise
    ///
    /// Example:
    /// ```
    ///   let queries =
    ///     [ createOptionalRequestParameter "feature"
    ///           (request.feature |> Option.map GameFeature.toString)
    ///       createOptionalRequestParameter "language"
    ///           (request.language |> Option.map Language.toString)
    ///       createRequestParameter "mediaType" "1" ]
    ///     |> List.concat
    /// ```
    let createOptionalRequestParameter name valueOption =
        match valueOption with
        | Some value -> createRequestParameter name value
        | None -> []

    /// Creates the GET request with correct authentication headers and parameters to given url
    /// Returns an Async which can be executed to send the request
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

    /// Helper function which catches exception from FSharp.Json and returns Result type
    ///
    /// The 'T Type represents a typesafe variant of the json
    ///
    /// Returns:
    /// - Error when exception occured
    /// - otherwise Ok with parsed object
    let parseJson<'T> rawJson =
        let parsedJson =
            try
                Json.deserializeEx<'T> jsonConfig rawJson |> Ok
            with
            | ex -> Error(rawJson, ex.ToString())

        parsedJson

    /// Function which creates an request which will be parsed into an object after returning
    ///
    /// The 'T Type represents a typesafe variant of the json response
    ///
    /// Returns an Async which can be executed to send the request and parse the answer
    let makeRequest<'T> authentication parameters url =
        async {
            let! response = setupRequest authentication parameters url

            let message = response |> Response.toText
            return parseJson<'T> message
        }
