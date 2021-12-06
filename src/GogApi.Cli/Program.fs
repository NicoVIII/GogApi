namespace GogApi.Cli

open Pattern

open GogApi
open GogApi.DomainTypes
open System

module Program =
    let printHelp () =
        printfn "-- GogApi.Cli --"
        printfn "help\n - Shows this list"
        printfn "exit | quit\n - Leaves program"
        printfn ""
        printfn "Account/getGameDetails <gameid>\n - Shows details for given game"
        printfn "Account/getFilteredGames <search> [<feature> [<language> [<sort> [<system>]]]]"
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

    let handleGetFilteredGames search feature language sort system page authentication =
        let feature =
            if feature = "-" then
                None
            else
                feature |> GameFeature.fromString |> Some

        let language =
            if language = "-" then
                None
            else
                language |> Language.fromString |> Some

        let search =
            if search = "-" then
                None
            else
                search |> Some

        let sort =
            if sort = "-" then
                None
            else
                sort |> Sort.fromString |> Some

        let system =
            if system = "-" then
                None
            else
                system |> OS.fromString |> Some

        let page =
            match page with
            | UInt page -> page |> Page |> Some
            | "-" -> None
            | _ -> None

        handleApiCall
            (Account.getFilteredGames
                { feature = feature
                  language = language
                  page = page
                  search = search
                  sort = sort
                  system = system })
            authentication

    let rec handleCommand authentication =
        Authentication.save authentication

        printfn "Enter a command (Type 'help' to see available commands):"

        Console.ReadLine().Split ' '
        |> List.ofArray
        |> function
            | [ "help" ] ->
                printHelp ()
                handleCommand authentication
            | [ "Account/getGameDetails"; UInt gameId ] ->
                handleApiCall (Account.getGameDetails (ProductId gameId)) authentication
                |> handleCommand
            | [ "Account/getFilteredGames"; NonEmptyString search ] ->
                handleGetFilteredGames search "-" "-" "-" "-" "-" authentication
                |> handleCommand
            | [ "Account/getFilteredGames"; NonEmptyString search; NonEmptyString feature ] ->
                handleGetFilteredGames search feature "-" "-" "-" "-" authentication
                |> handleCommand
            | [ "Account/getFilteredGames"; NonEmptyString search; NonEmptyString feature; NonEmptyString language ] ->
                handleGetFilteredGames search feature language "-" "-" "-" authentication
                |> handleCommand
            | [ "Account/getFilteredGames"
                NonEmptyString search
                NonEmptyString feature
                NonEmptyString language
                NonEmptyString sort ] ->
                handleGetFilteredGames search feature language sort "-" "-" authentication
                |> handleCommand
            | [ "Account/getFilteredGames"
                NonEmptyString search
                NonEmptyString feature
                NonEmptyString language
                NonEmptyString sort
                NonEmptyString system ] ->
                handleGetFilteredGames search feature language sort system "-" authentication
                |> handleCommand
            | [ "Account/getFilteredGames"
                NonEmptyString search
                NonEmptyString feature
                NonEmptyString language
                NonEmptyString sort
                NonEmptyString system
                NonEmptyString page ] ->
                handleGetFilteredGames search feature language sort system page authentication
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
            | "GalaxyApi/getProduct" :: [ UInt productId ] ->
                handleApiCall (GalaxyApi.getProduct (ProductId productId)) authentication
                |> handleCommand
            | "GalaxyApi/getSecureDownlink" :: [ downlink ] ->
                handleApiCall (GalaxyApi.getSecureDownlink (DownLink downlink)) authentication
                |> handleCommand
            | [ "exit" ]
            | [ "quit" ] -> ()
            | _ ->
                printfn "Invalid command.\n"
                handleCommand authentication

    [<EntryPoint>]
    let main _ =
        Authentication.get () |> handleCommand
        0
