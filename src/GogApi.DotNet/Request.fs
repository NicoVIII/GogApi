namespace GogApi.DotNet.FSharp

open GogApi.DotNet.FSharp.DomainTypes

open FSharp.Json
open FsHttp
open FsHttp.DslCE

/// <summary>
/// This module contains low-level functions and types to make requests to the GOG API
/// </summary>
module Request =
    let jsonConfig =
        { JsonConfig.Default with
              allowUntyped = true }

    /// <summary>
    /// Simple record for request parameters
    /// </summary>
    type RequestParameter = { name: string; value: string }

    /// <summary>
    /// Creates a RequestParameter
    /// </summary>
    let createRequestParameter name value = [ { name = name; value = value } ]

    let createOptionalRequestParameter name valueOption =
        match valueOption with
        | Some value -> createRequestParameter name value
        | None -> []

    /// <summary>
    /// Creates the GET request with correct authentication headers and parameters to given url
    /// </summary>
    /// <returns>An Async which can be executed to send the request</returns>
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
            | Some { accessToken = token } -> httpRequest baseHeader { BearerAuth token }
            | None -> baseHeader

        request |> sendAsync

    /// <summary>
    /// Helper function which catches exception from FSharp.Json and returns Result type
    /// </summary>
    /// <typeparam name="'T">Type which represents a typesafe variant of the json</typeparam>
    /// <returns>
    /// - Error when exception occured
    /// - otherwise Ok with parsed object
    /// </returns>
    let parseJson<'T> rawJson =
        let parsedJson =
            try
                Json.deserializeEx<'T> jsonConfig rawJson |> Ok
            with ex -> Error(rawJson, ex.ToString())

        parsedJson

    /// <summary>
    /// Function which creates an request which will be parsed into an object after returning
    /// </summary>
    /// <typeparam name="'T">Type which represents a typesafe variant of the json response</typeparam>
    /// <returns>An Async which can be executed to send the request and parse the answer</returns>
    let makeRequest<'T> authentication parameters url =
        async {
            let! response = setupRequest authentication parameters url

            let message = response |> toText
            return parseJson<'T> message
        }
