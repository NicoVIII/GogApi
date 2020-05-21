namespace GogApi.DotNet.Cli

open Pattern

open GogApi.DotNet.FSharp
open GogApi.DotNet.FSharp.DomainTypes
open System

module Program =
    let printHelp() =
        printfn "-- GogApi.DotNet.Cli --"
        printfn "help\n - Shows this list"
        printfn "exit | quit\n - Leaves program"
        printfn ""
        printfn "Account/getGameDetails <gameid>\n - Shows details for given game"
        printfn ""
        printfn "GalaxyApi/getProduct <productid>\n - Fetches information about a product"
        printfn "GalaxyApi/getSecureDownlink <downlink>\n - Fetches a secure version of a downlink"
        printfn ""
        printfn "User/getData\n - Shows information of the currently logged in user"
        printfn "User/getDataGames\n - Shows ids of owned games"
        printfn "User/getWishlist\n - Shows the contents of your wishlist"
        printfn ""

    let inline handleApiCall apiCall authentication =
        let result, authentication =
            Helpers.withAutoRefresh apiCall authentication
            |> Async.RunSynchronously
        printfn "%A\n" result
        authentication

    let rec handleCommand authentication =
        Authentication.save authentication

        printfn "Enter a command (Type 'help' to see available commands):"
        Console.ReadLine().Split ' '
        |> List.ofArray
        |> function
        | [ "help" ] ->
            printHelp()
            handleCommand authentication
        | "Account/getGameDetails"::[UInt gameId] ->
            handleApiCall (Account.getGameDetails (ProductId gameId)) authentication
            |> handleCommand
        | [ "User/getData" ] ->
            handleApiCall User.getData authentication
            |> handleCommand
        | [ "User/getDataGames" ] ->
            handleApiCall User.getDataGames authentication
            |> handleCommand
        | [ "User/getWishlist" ] ->
            handleApiCall User.getWishlist authentication
            |> handleCommand
        | "GalaxyApi/getProduct"::[UInt productId] ->
            handleApiCall (GalaxyApi.getProduct (ProductId productId)) authentication
            |> handleCommand
        | "GalaxyApi/getSecureDownlink"::[downlink] ->
            handleApiCall (GalaxyApi.getSecureDownlink (DownLink downlink)) authentication
            |> handleCommand
        | [ "exit" ]
        | [ "quit" ] ->
            ()
        | _ ->
            printfn "Invalid command.\n"
            handleCommand authentication

    [<EntryPoint>]
    let main _ =
        Authentication.get ()
        |> handleCommand
        0
