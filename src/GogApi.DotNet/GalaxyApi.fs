namespace GogApi.DotNet.FSharp

open GogApi.DotNet.FSharp.Request
open GogApi.DotNet.FSharp.Types

/// <summary>
/// Contains special endpoints for Galaxy
/// </summary>
module GalaxyApi =
    type InstallerFileInfo =
        { id: string
          size: int64
          downlink: DownLink }

    type InstallerInfo =
        { id: string
          os: string
          version: string option
          files: InstallerFileInfo list }

    type DownloadsInfo =
        { installers: InstallerInfo list
          patches: obj list
          language_packs: obj list
          bonus_content: BonusContent list }

    type Product =
        { id: uint32
          link: string
          expanded_link: string }

    type Dlcs =
        | Empty of obj list
        | Filled of {| products: Product list
                       all_products_url: string
                       expanded_all_products_url: string |}

    type ProductsResponse =
        { id: int
          title: string
          purchase_link: string
          slug: string
          content_system_compatibility: {| windows: bool
                                           osx: bool
                                           linux: bool |}
          languages: Map<string, string>
          links: {| purchase_link: string
                    product_card: string
                    support: string
                    forum: string |}
          in_development: {| active: bool; until: obj option |}
          is_secret: bool
          is_installable: bool
          game_type: string
          is_pre_order: bool
          release_date: string // TODO: to DateTime oder so?
          images: {| background: string
                     logo: string
                     logo2x: string
                     icon: string
                     sidebarIcon: string
                     sidebarIcon2x: string
                     menuNotificationAv: string
                     menuNotificationAv2: string |}
          // dlcs: Dlcs // TODO: fix somehow... When empty: [], when not record
          downloads: DownloadsInfo }

    /// <summary>
    /// Returns information about a product
    /// </summary>
    let getProduct (id: uint32) authentication =
        let queries =
            [ createRequestParameter "expand" "downloads" ]

        sprintf "https://api.gog.com/products/%i" id
        |> makeRequest<ProductsResponse> (Some authentication) queries

    type SecureUrlResponse =
        { downlink: DownLink
          checksum: string }

    /// <summary>
    /// Takes a downlink from productinfo and transforms it into a secure version
    /// which can be used to download installer files
    /// </summary>
    let getSecureDownlink (DownLink downlink) authentication =
        // Workaround: For some reason FsHttp isn't following the redirect correctly
        // Therefore I have to replace http with https here
        let downlink = downlink.Replace("http://", "https://")
        makeRequest<SecureUrlResponse> (Some authentication) [] downlink
