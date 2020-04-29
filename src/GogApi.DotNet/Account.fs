namespace GogApi.DotNet.FSharp

open FSharp.Json

open GogApi.DotNet.FSharp.Request
open GogApi.DotNet.FSharp.Transforms
open GogApi.DotNet.FSharp.Types

/// <summary>
/// This module holds all API calls which has to do with games/movies on GOG
/// </summary>
module Account =
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

    type FilteredProductsRequest =
        { search: string }

    type FilteredProductsResponse =
        { totalProducts: int
          products: ProductInfo list }

    /// <summary>
    /// Searches for games owned by the user matching the given search term.
    /// TODO: This is a specified version of an API, this will be generatlized
    /// </summary>
    let getFilteredGames (request: FilteredProductsRequest) authentication =
        let queries =
            [ createRequestParameter "mediaType" "1"
              createRequestParameter "search" request.search ]
        makeRequest<FilteredProductsResponse> (Some authentication) queries
            "https://embed.gog.com/account/getFilteredProducts"
