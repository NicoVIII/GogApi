module GogApi.DotNet.FSharp.Listing

type FilteredProductsRequest = {
    search :string
}

type ProductInfo = {
    id: int;
    title: string;
}

type FilteredProductsResponse = {
    totalProducts: int;
    products: ProductInfo list;
}

let askForFilteredProducts (request :FilteredProductsRequest) authentication =
    let queries = [
        createQuery "mediaType" "1";
        createQuery "search" request.search
    ]
    makeRequest<FilteredProductsResponse> authentication queries "https://embed.gog.com/account/getFilteredProducts"
