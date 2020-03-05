namespace GogApi.DotNet.FSharp

open FSharp.Json
open Hopac
open HttpFs.Client
open System

[<AutoOpen>]
module Base =
    type QueryString =
        { name: QueryStringName
          value: QueryStringValue }

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

    let setupRequest method auth queries url =
        (Request.createUrl method url, auth)
        // Add auth data
        |> function
        | (r, NoAuth) -> r
        | (r, Auth { accessToken = token }) -> Request.setHeader (Authorization("Bearer " + token)) r
        // Add query parameters
        |> List.fold (fun request query -> Request.queryStringItem query.name query.value request)
        <| queries

    let startRequest method auth queries url =
        setupRequest method auth queries url
        |> Request.responseAsString
        |> startAsTask

    let parseJson<'T> rawJson =
        let parsedJson =
            try
                Json.deserializeEx<'T> config rawJson |> Ok
            with ex -> Error(rawJson, ex.Message)
        parsedJson

    let makeRequest<'T> auth queries url =
        async {
            let! jsonString = startRequest Get auth queries url |> Async.AwaitTask
            return parseJson<'T> jsonString }
