namespace GogApi.DotNet.FSharp

open GogApi.DotNet.FSharp.Request

module GamesMovies =
    // TODO: Make it typesafer? More Domaintypes? Is that a good idea? As alternative?
    type OwnedGameIdsResponse =
        { owned: int list }

    let getOwnedGameIds authentication =
        makeRequest<OwnedGameIdsResponse> (Some authentication) []
            "https://embed.gog.com/user/data/games"

    type GameInfoResponse =
        { title: string
          downloads: obj list list }

    let getGameInfo id authentication =
        sprintf "https://embed.gog.com/account/gameDetails/%i.json" id
        |> makeRequest<GameInfoResponse> (Some authentication) []
