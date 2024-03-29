namespace GogApi

open GogApi.DomainTypes
open Internal.Request
open Internal.Transforms

open FSharp.Json

/// This module holds all API calls which have to do with games/movies on GOG
[<RequireQualifiedAccess>]
module Account =
    ///<summary>Contains detailed info about a game requested via
    /// <see cref="M:GogApi.Account.getGameDetails"/></summary>
    type GameInfoResponse = {
        title: string
        backgroundImage: string
        cdKey: string
        textInformation: string
        [<JsonField(Transform = typeof<DownloadsObjListTransform>)>]
        downloads: Map<string, Download>
        galaxyDownloads: obj list // TODO: #10
        extras: GameExtra list
        dlcs: Dlc list
        tags: Tag list
        isPreOrder: bool
        releaseTimestamp: uint64
        messages: obj list // TODO: #10
        changelog: string
        forumLink: string
        isBaseProductMissing: bool
        missingBaseProduct: obj option // TODO: #10
        features: obj list // TODO: #10
        simpleGalaxyInstallers: {| path: string; os: string |} list
    }

    /// Fetches some details about the game with given id
    let getGameDetails productId authentication =
        productId
        |> ProductId.getValue
        |> sprintf "https://embed.gog.com/account/gameDetails/%i.json"
        |> makeRequest<GameInfoResponse> (Some authentication) []

    ///<summary>Contains possible parameters with which one can specify what is searched
    /// for with <see cref="M:GogApi.Account.getFilteredGames" /></summary>
    type FilteredProductsRequest = {
        feature: GameFeature option
        language: Language option
        page: Page option
        search: string option
        sort: Sort option
        system: OS option
    }

    ///<summary>Contains info about products which matched the search requested
    /// via <see cref="M:GogApi.Account.getFilteredGames"/></summary>
    type FilteredProductsResponse = {
        sortBy: Sort option
        page: Page
        totalProducts: uint32
        totalPages: uint32
        productsPerPage: uint32
        contentSystemCompatibility: obj option
        moviesCount: uint32
        tags: Tag list
        products: ProductInfo list
        updatedProductsCount: uint32
        hiddenUpdatedProductsCount: uint32
        appliedFilters: {| tags: obj option |}
        hasHiddenProducts: bool
    }

    /// Module which contains internal stuff, which is NOT considered in applying SemVer
    module Internal =
        ///<summary>Is used to parse json and is converted to
        /// <see cref="T:GogApi.Account.FilteredProductsResponse"/> after
        /// that</summary>
        type RawFilteredProductsResponse = {
            [<JsonField("sort_by")>]
            sortBy: string option
            page: Page
            totalProducts: uint32
            totalPages: uint32
            productsPerPage: uint32
            contentSystemCompatibility: obj option
            moviesCount: uint32
            tags: Tag list
            products: ProductInfo list
            updatedProductsCount: uint32
            hiddenUpdatedProductsCount: uint32
            appliedFilters: {| tags: obj option |}
            hasHiddenProducts: bool
        }

        /// Converts the raw version into the nicely typed one
        let fromRawFilteredProductsResponse (internalResponse: RawFilteredProductsResponse) = {
            FilteredProductsResponse.sortBy = internalResponse.sortBy |> Option.map Sort.fromString
            page = internalResponse.page
            totalProducts = internalResponse.totalProducts
            totalPages = internalResponse.totalPages
            productsPerPage = internalResponse.productsPerPage
            contentSystemCompatibility = internalResponse.contentSystemCompatibility
            moviesCount = internalResponse.moviesCount
            tags = internalResponse.tags
            products = internalResponse.products
            updatedProductsCount = internalResponse.updatedProductsCount
            hiddenUpdatedProductsCount = internalResponse.hiddenUpdatedProductsCount
            appliedFilters = internalResponse.appliedFilters
            hasHiddenProducts = internalResponse.hasHiddenProducts
        }

    /// Searches for games owned by the user matching the given search parameters
    let getFilteredGames (request: FilteredProductsRequest) authentication =
        let queries =
            [
                createOptionalRequestParameter "feature" (request.feature |> Option.map GameFeature.toString)
                createOptionalRequestParameter "language" (request.language |> Option.map Language.toString)
                createRequestParameter "mediaType" "1"
                createOptionalRequestParameter "page" (Option.map (fun (Page p) -> p.ToString()) request.page)
                createOptionalRequestParameter "search" request.search
                createOptionalRequestParameter "sort" (request.sort |> Option.map Sort.toString)
                createOptionalRequestParameter "system" (request.system |> Option.map OS.toString)
            ]
            |> List.concat

        async {
            let! response =
                makeRequest<Internal.RawFilteredProductsResponse>
                    (Some authentication)
                    queries
                    "https://embed.gog.com/account/getFilteredProducts"

            return Result.map (Internal.fromRawFilteredProductsResponse) response
        }
