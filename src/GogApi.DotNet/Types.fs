namespace GogApi.DotNet.FSharp

open System

/// <summary>
/// Contains all basic types which are needed in the whole domain
/// </summary>
module Types =
    /// <summary>
    /// Data which is needed to authenticate for the API
    /// </summary>
    type Authentication =
        { accessToken: string
          refreshToken: string
          accessExpires: DateTimeOffset }

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

    type FriendInfo =
        { username: UserName
          userSince: int
          galaxyId: string
          avatar: string }

    type Currency = { code: string; symbol: string }

    type DownloadOSInfo =
        { date: string
          downloaderUrl: string option
          manualUrl: string
          name: string
          size: string
          version: string option }

    type Download =
        { linux: DownloadOSInfo list
          mac: DownloadOSInfo list
          windows: DownloadOSInfo list }

    type Language = { code: string; name: string }

    type GameExtra =
        { manualUrl: string
          downloaderUrl: string option
          name: string
          ``type``: string
          info: int
          size: string }

    type ProductInfo = { id: ProductId; title: string }

    type File =
        { id: int
          size: uint32
          downlink: DownLink }

    type BonusContent =
        { id: int
          name: string
          ``type``: string
          count: int
          total_size: uint32
          files: File list }

    type Tag =
        { id: string
          name: string
          productCount: string }
