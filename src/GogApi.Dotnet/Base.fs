namespace GogApi.DotNet.FSharp

open FSharp.Json
open FsHttp
open FsHttp.DslCE
open System

[<AutoOpen>]
module Base =
    type QueryParameter =
        { name: string
          value: string }

    let createQuery name value =
        { name = name
          value = value }

    type AuthenticationData =
        { accessToken: string
          refreshToken: string
          accessExpires: DateTimeOffset }

    type Authentication =
        | NoAuth
        | Auth of AuthenticationData

    let config = JsonConfig.create (allowUntyped = true)

    let setupRequest auth queries url =
        let url =
            match queries with
            | [] ->
                url
            | queries ->
                let parameters =
                    queries
                    |> List.map (fun param -> param.name + "=" + param.value)
                    |> List.reduce (fun param1 param2 -> param1 + "&" + param2)
                url + "?" + parameters

        let baseHeader =
            httpLazy {
                GET url
                CacheControl "no-cache"
            }
        let request =
            match auth with
            | NoAuth ->
                baseHeader
            | Auth { accessToken = token } ->
                httpRequest baseHeader {
                    BearerAuth token
                }
        request |> sendAsync

    let parseJson<'T> rawJson =
        let parsedJson =
            try
                Json.deserializeEx<'T> config rawJson |> Ok
            with ex -> Error(rawJson, ex.Message)
        parsedJson

    let makeRequest<'T> auth queries url =
        async {
            let! response = setupRequest auth queries url

            let message =
                response
                |> toText
            return parseJson<'T> message
        }
