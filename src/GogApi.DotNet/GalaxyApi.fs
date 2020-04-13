namespace GogApi.DotNet.FSharp

open GogApi.DotNet.FSharp.Request

/// <summary>
/// Contains special endpoints for Galaxy
/// </summary>
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

    /// <summary>
    /// Returns information about a product
    /// </summary>
    let getProductInfo (request: ProductInfoRequest) authentication =
        let queries = [ createRequestParameter "expand" "downloads" ]
        sprintf "https://api.gog.com/products/%i" request.id
        |> makeRequest<ProductsResponse> (Some authentication) queries

    type SecureUrlRequest =
        { downlink: string }

    type SecureUrlResponse =
        { downlink: string
          checksum: string }

    /// <summary>
    /// Takes a downlink from productinfo and transforms it into a secure version
    /// which can be used to download installer files
    /// </summary>
    let getSecureDownlink (request: SecureUrlRequest) authentication =
        makeRequest<SecureUrlResponse> (Some authentication) [] request.downlink
