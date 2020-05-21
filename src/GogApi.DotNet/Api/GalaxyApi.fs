namespace GogApi.DotNet.FSharp

open GogApi.DotNet.FSharp.DomainTypes
open Request

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

    type Patch =
        { id: string
          language: string
          language_full: string
          name: string
          os: string
          total_size: FileSize
          version: string
          files: {| downlink: DownLink
                    id: string
                    size: FileSize |} list }

    type DownloadsInfo =
        { installers: InstallerInfo list
          patches: Patch list
          //language_packs: obj list
          bonus_content: BonusContent list }

    type Product =
        { id: ProductId
          link: string
          expanded_link: string }

    type ProductsResponse =
        { id: ProductId
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
          in_development: {| active: bool; until: obj option |} // TODO: #10
          is_secret: bool
          is_installable: bool
          game_type: string
          is_pre_order: bool
          release_date: string
          images: {| background: string
                     logo: string
                     logo2x: string
                     icon: string
                     sidebarIcon: string
                     sidebarIcon2x: string
                     menuNotificationAv: string
                     menuNotificationAv2: string |}
          // dlcs: Dlcs // TODO: #11 fix somehow... When empty: [], when not record
          downloads: DownloadsInfo }

    /// <summary>
    /// Returns information about a product
    /// </summary>
    let getProduct (ProductId id) authentication =
        let queries =
            [ createRequestParameter "expand" "downloads" ]

        sprintf "https://api.gog.com/products/%i" id
        |> makeRequest<ProductsResponse> (Some authentication) queries

    type SecureUrlResponse =
        { downlink: SafeDownLink
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
