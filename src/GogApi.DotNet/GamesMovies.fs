namespace GogApi.DotNet.FSharp

open FSharp.Json

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

    type GameExtra =
        { manualUrl: string
          downloaderUrl: string
          name: string
          ``type``: string // TODO: Is it possible to match this to a DU?
          info: int
          size: string } // TODO: parse this somehow in a number? In additional field?

    type GameInfoResponse =
        { title: string
          backgroundImage: string
          cdKey: string
          textInformation: string
          [<JsonField(Transform=typeof<DownloadsObjListTransform>)>]
          downloads: Map<string, Download>
          galaxyDownloads: obj list
          extras: GameExtra list
          dlcs: obj list
          tags: obj list
          isPreOrder: bool
          releaseTimestamp: uint64
          messages: obj list
          changelog: string
          forumLink: string
          isBaseProductMissing: bool
          missingBaseProduct: obj option
          features: obj list
          simpleGalaxyInstallers: {| path: string; os: string |} list }

    /// <summary>
    /// Fetches some details about the game with given id
    /// </summary>
    let getGameDetails (GameId id) authentication =
        sprintf "https://embed.gog.com/account/gameDetails/%i.json" id
        |> makeRequest<GameInfoResponse> (Some authentication) []
// TODO: Improve return type structure
