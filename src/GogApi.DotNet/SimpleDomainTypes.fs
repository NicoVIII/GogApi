namespace GogApi.DotNet.FSharp.DomainTypes

open FSharp.Json
open System

/// Data which is needed to authenticate for the API
type Authentication =
    { accessToken: string
      refreshToken: string
      accessExpires: DateTimeOffset }

/// Represents the id of a product (game/movie)
type ProductId = ProductId of uint32

/// Represents the id of a user
type UserId = UserId of uint64

/// Represents the name of a user
type UserName = UserName of string

/// Represents a link, which can be used to request a safe download link
type DownLink = DownLink of string

/// Represents a link, which can be used to download a file
type SafeDownLink = SafeDownLink of string

/// Represents a size of a file
type FileSize = FileSize of uint64

/// Represents a page in requests
type Page = Page of uint32

/// Contains info about a friend
type FriendInfo =
    { username: UserName
      userSince: int
      galaxyId: string
      avatar: string }

/// Contains info about a currency
type Currency = { code: string; symbol: string }

/// Contains info about a specific download for a specific OS.
/// Is used inside of <T:GogApi.DotNet.FSharp.DomainTypes.Download>
type DownloadOSInfo =
    { date: string
      downloaderUrl: string option
      manualUrl: string
      name: string
      size: string
      version: string option }

/// Contains info about available downloads for different OSes
type Download =
    { linux: DownloadOSInfo list
      mac: DownloadOSInfo list
      windows: DownloadOSInfo list }

/// Contains info about an extra for a game
type GameExtra =
    { manualUrl: string
      downloaderUrl: string option
      name: string
      ``type``: string
      info: int
      size: string }

/// Contains some basic info about a product (game / movie)
type ProductInfo = { id: ProductId; title: string }

/// Contains some basic info about a file
type File =
    { id: int
      size: uint32
      downlink: DownLink }

/// Contains some basic info about bonus content
type BonusContent =
    { id: int
      name: string
      ``type``: string
      count: int
      [<JsonField("total_size")>]
      totalSize: uint32
      files: File list }

/// Contains info about a tag
type Tag =
    { id: string
      name: string
      productCount: string }

/// Contains info about an installer file
type InstallerFileInfo =
    { id: string
      size: int64
      downlink: DownLink }

/// Contains info about an installer for a game
type InstallerInfo =
    { id: string
      os: string
      version: string option
      files: InstallerFileInfo list }

/// Contains info about a patch for a game
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

/// Contains info about available downloads for a game
type DownloadsInfo =
    { installers: InstallerInfo list
      patches: Patch list
      [<JsonField("language_packs")>]
      languagePacks: obj list // TODO: #10
      [<JsonField("bonus_content")>]
      bonusContent: BonusContent list }

/// Contains info about a product
type Product =
    { id: ProductId
      link: string
      [<JsonField("expanded_link")>]
      expandedLink: string }
