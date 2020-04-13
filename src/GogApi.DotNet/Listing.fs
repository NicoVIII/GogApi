namespace GogApi.DotNet.FSharp

open GogApi.DotNet.FSharp.Request

/// <summary>
/// Contains methods which list games/movies
/// </summary>
module Listing =
    type FilteredProductsRequest =
        { search: string }

    type ProductInfo =
        { id: int
          title: string }

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
