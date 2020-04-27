namespace GogApi.DotNet

open CommandLine
open FSharp.Json
open GogApi.DotNet.FSharp
open GogApi.DotNet.FSharp.Types
open System
open System.IO

module Cli =
    let url =
        "https://login.gog.com/auth?client_id=46899977096215655&layout=client2&redirect_uri=https%3A%2F%2Fembed.gog.com%2Fon_login_success%3Forigin%3Dclient&response_type=code"
    let authFile = "authentication.json"

    let rec getAuthentication() =
        match File.Exists authFile with
        | false ->
            printfn
                "Please go to %s , log in and paste the code in the resulting get parameter:"
                url
            let authCode = Console.ReadLine()

            let authentication =
                Authentication.getNewToken
                    "https://embed.gog.com/on_login_success?origin=client" authCode
                |> Async.RunSynchronously

            match authentication with
            | Some authentication -> authentication
            | None -> getAuthentication()
        | true ->
            Json.deserialize<Authentication> <| File.ReadAllText authFile

    let printHelp() =
        printfn "-- GogApi.DotNet.Cli --"
        printfn "help\n - Shows this list"
        printfn "quit\n - Leaves program"
        printfn ""
        printfn "getUserData\n - Shows information of the currently logged in user"
        printfn ""

    let rec handleApiCall authentication =
        // Save authentication
        File.WriteAllText (authFile, authentication |> Json.serialize)

        printfn "Enter a command (Type 'help' to see available commands):"
        Console.ReadLine().Split ' '
        |> List.ofArray
        |> function
        | [ "help" ] ->
            printHelp()
            handleApiCall authentication
        | [ "getUserData" ] ->
            let result, authentication =
                Helpers.withAutoRefresh User.getUserData authentication
                |> Async.RunSynchronously
            printfn "%A\n" result
            handleApiCall authentication.Value
        | [ "quit" ] ->
            ()
        | _ ->
            printfn "Invalid command.\n"
            handleApiCall authentication

    [<EntryPoint>]
    let main _ =
        getAuthentication ()
        |> handleApiCall
        0
