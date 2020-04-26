namespace GogApi.DotNet.FSharp

open GogApi.DotNet.FSharp.Request
open GogApi.DotNet.FSharp.Types

/// <summary>
/// This module holds all API calls which has to do with games/movies on GOG
/// </summary>
module GamesMovies =
    type OwnedGameIdsResponse =
        { owned: GameId list }

    /// <summary>
    /// Fetches a list of game ids of the games the authenticated account owns
    /// </summary>
    let getOwnedGameIds authentication =
        makeRequest<OwnedGameIdsResponse> (Some authentication) []
            "https://embed.gog.com/user/data/games"

    type GameInfoResponse =
        { title: string
          downloads: obj list list }

    /// <summary>
    /// Fetches a list of game ids of the games the authenticated account owns
    /// </summary>
    let getGameDetails (GameId id) authentication =
        sprintf "https://embed.gog.com/account/gameDetails/%i.json" id
        |> makeRequest<GameInfoResponse> (Some authentication) []
        // TODO: Improve return type structure
