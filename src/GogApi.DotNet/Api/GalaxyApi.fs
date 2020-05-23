namespace GogApi.DotNet.FSharp

open GogApi.DotNet.FSharp.DomainTypes
open Internal.Request

open FSharp.Json

/// <summary>
/// Contains special endpoints for Galaxy
/// </summary>
[<RequireQualifiedAccess>]
module GalaxyApi =
    /// <summary>
    /// Contains info about an installer file
    /// </summary>
    type InstallerFileInfo =
        { id: string
          size: int64
          downlink: DownLink }

    /// <summary>
    /// Contains info about an installer for a game
    /// </summary>
    type InstallerInfo =
        { id: string
          os: string
          version: string option
          files: InstallerFileInfo list }

    /// <summary>
    /// Contains info about a patch for a game
    /// </summary>
    type Patch =
        { id: string
          language: string
          [<JsonField("language_full")>]
          languageFull: string
          name: string
          os: string
          [<JsonField("total_size")>]
          totalSize: FileSize
          version: string
          files: {| downlink: DownLink
                    id: string
                    size: FileSize |} list }

    /// <summary>
    /// Contains info about available downloads for a game
    /// </summary>
    type DownloadsInfo =
        { installers: InstallerInfo list
          patches: Patch list
          [<JsonField("language_packs")>]
          languagePacks: obj list // TODO: #10
          [<JsonField("bonus_content")>]
          bonusContent: BonusContent list }

    /// <summary>
    /// Contains info about a product
    /// </summary>
    type Product =
        { id: ProductId
          link: string
          [<JsonField("expanded_link")>]
          expandedLink: string }

    /// <summary>
    /// Contains info about a product requested via <see cref="M:GogApi.DotNet.FSharp.GalaxyApi.getProduct"/>
    /// </summary>
    type ProductsResponse =
        { id: ProductId
          title: string
          [<JsonField("purchase_link")>]
          purchaseLink: string
          slug: string
          [<JsonField("content_system_compatibility")>]
          contentSystemCompatibility: {| windows: bool
                                         osx: bool
                                         linux: bool |}
          languages: Map<string, string>
          links: {| purchase_link: string
                    product_card: string
                    support: string
                    forum: string |}
          [<JsonField("in_development")>]
          inDevelopment: {| active: bool; until: obj option |} // TODO: #10
          [<JsonField("is_secret")>]
          isSecret: bool
          [<JsonField("is_installable")>]
          isInstallable: bool
          [<JsonField("game_type")>]
          gameType: string
          [<JsonField("is_pre_order")>]
          isPreOrder: bool
          [<JsonField("release_date")>]
          releaseDate: string
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
            |> List.concat

        sprintf "https://api.gog.com/products/%i" id
        |> makeRequest<ProductsResponse> (Some authentication) queries

    /// <summary>
    /// Contains info about a secure url requested via <see cref="M:GogApi.DotNet.FSharp.GalaxyApi.getSecureDownlink"/>
    /// </summary>
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
