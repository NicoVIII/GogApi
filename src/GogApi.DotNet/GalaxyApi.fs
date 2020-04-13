namespace GogApi.DotNet.FSharp

open GogApi.DotNet.FSharp.Request

module GalaxyApi =
    type ProductInfoRequest =
        { id: int }

    type InstallerFileInfo =
        { id: string
          size: int64
          downlink: string }

    type InstallerInfo =
        { id: string
          os: string
          version: string option
          files: InstallerFileInfo list }

    type DownloadsInfo =
        { installers: InstallerInfo list }

    type ProductsResponse =
        { id: int
          title: string
          downloads: DownloadsInfo }

    let getProductInfo (request: ProductInfoRequest) authentication =
        let queries = [ createRequestParameter "expand" "downloads" ]
        sprintf "https://api.gog.com/products/%i" request.id
        |> makeRequest<ProductsResponse> (Some authentication) queries

    type SecureUrlRequest =
        { downlink: string }

    type SecureUrlResponse =
        { downlink: string
          checksum: string }

    let getSecureDownlink (request: SecureUrlRequest) authentication =
        makeRequest<SecureUrlResponse> (Some authentication) [] request.downlink
