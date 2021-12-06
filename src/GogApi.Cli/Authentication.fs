namespace GogApi.Cli

open FSharp.Json
open GogApi
open GogApi.DomainTypes
open System
open System.IO

[<RequireQualifiedAccess>]
module Authentication =
    let url =
        "https://login.gog.com/auth?client_id=46899977096215655&layout=client2&redirect_uri=https%3A%2F%2Fembed.gog.com%2Fon_login_success%3Forigin%3Dclient&response_type=code"

    let authFile = "authentication.json"

    let rec get () =
        match File.Exists authFile with
        | false ->
            printfn "Please go to %s , log in and paste the code in the resulting get parameter:" url
            let authCode = Console.ReadLine()

            let authentication =
                Authentication.getNewToken "https://embed.gog.com/on_login_success?origin=client" authCode
                |> Async.RunSynchronously

            match authentication with
            | Some authentication -> authentication
            | None -> get ()
        | true ->
            Json.deserialize<Authentication>
            <| File.ReadAllText authFile

    let save (authentication: Authentication) =
        File.WriteAllText(authFile, authentication |> Json.serialize)
