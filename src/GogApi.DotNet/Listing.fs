namespace GogApi.DotNet.FSharp

open GogApi.DotNet.FSharp.Request

module Listing =
    type FilteredProductsRequest =
        { search: string }

    type ProductInfo =
        { id: int
          title: string }

    type FilteredProductsResponse =
        { totalProducts: int
          products: ProductInfo list }

    let getFilteredProducts (request: FilteredProductsRequest) authentication =
        let queries =
            [ createRequestParameter "mediaType" "1"
              createRequestParameter "search" request.search ]
        makeRequest<FilteredProductsResponse> (Some authentication) queries
            "https://embed.gog.com/account/getFilteredProducts"
