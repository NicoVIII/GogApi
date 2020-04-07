namespace GogApi.DotNet.FSharp

open GogApi.DotNet.FSharp.Request

module GamesMovies =
    type OwnedGamesResponse =
        { owned: int list }

    let getOwnedGameIds authentication =
        makeRequest<OwnedGamesResponse> authentication [] "https://embed.gog.com/user/data/games"

    type GameDetailsResponse =
        { title: string
          downloads: obj list list }

    let getGameInfo id authentication =
        sprintf "https://embed.gog.com/account/gameDetails/%i.json" id
        |> makeRequest<GameDetailsResponse> authentication []
