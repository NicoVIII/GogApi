namespace GogApi.DotNet.FSharp.DomainTypes

open FSharp.Json
open System

/// <summary>
/// Data which is needed to authenticate for the API
/// </summary>
type Authentication =
    { accessToken: string
      refreshToken: string
      accessExpires: DateTimeOffset }

/// <summary>
/// Represents the id of a product (game/movie)
/// </summary>
type ProductId = ProductId of uint32

/// <summary>
/// Represents the id of a user
/// </summary>
type UserId = UserId of uint64

/// <summary>
/// Represents the name of a user
/// </summary>
type UserName = UserName of string

/// <summary>
/// Represents a link, which can be used to request a safe download link
/// </summary>
type DownLink = DownLink of string

/// <summary>
/// Represents a link, which can be used to download a file
/// </summary>
type SafeDownLink = SafeDownLink of string

/// <summary>
/// Represents a size of a file
/// </summary>
type FileSize = FileSize of uint64

/// <summary>
/// Represents a page in requests
/// </summary>
type Page = Page of uint32

/// <summary>
/// Contains info about a friend
/// </summary>
type FriendInfo =
    { username: UserName
      userSince: int
      galaxyId: string
      avatar: string }

/// <summary>
/// Contains info about a currency
/// </summary>
type Currency = { code: string; symbol: string }

/// <summary>
/// Contains info about a specific download for a specific OS.
/// Is used inside of <see cref="T:GogApi.DotNet.FSharp.DomainTypes.Download"/>
/// </summary>
type DownloadOSInfo =
    { date: string
      downloaderUrl: string option
      manualUrl: string
      name: string
      size: string
      version: string option }

/// <summary>
/// Contains info about available downloads for different OSes
/// </summary>
type Download =
    { linux: DownloadOSInfo list
      mac: DownloadOSInfo list
      windows: DownloadOSInfo list }

/// <summary>
/// Contains info about an extra for a game
/// </summary>
type GameExtra =
    { manualUrl: string
      downloaderUrl: string option
      name: string
      ``type``: string
      info: int
      size: string }

/// <summary>
/// Contains some basic info about a product (game / movie)
/// </summary>
type ProductInfo = { id: ProductId; title: string }

/// <summary>
/// Contains some basic info about a file
/// </summary>
type File =
    { id: int
      size: uint32
      downlink: DownLink }

/// <summary>
/// Contains some basic info about bonus content
/// </summary>
type BonusContent =
    { id: int
      name: string
      ``type``: string
      count: int
      [<JsonField("total_size")>]
      totalSize: uint32
      files: File list }

/// <summary>
/// Contains info about a tag
/// </summary>
type Tag =
    { id: string
      name: string
      productCount: string }
