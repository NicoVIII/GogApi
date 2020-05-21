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

    type GameFeature =
        | Singleplayer
        | Multiplayer
        | Coop
        | Achievements
        | Leaderboards
        | ControllerSupport
        | InDevelopment
        | CloudSaves
        | Overlay
        | Custom of string

    module GameFeature =
        let private featureMap =
            Map.empty
            |> Map.add Singleplayer "single"
            |> Map.add Multiplayer "multi"
            |> Map.add Coop "coop"
            |> Map.add Achievements "achievements"
            |> Map.add Leaderboards "leaderboards"
            |> Map.add ControllerSupport "controller_support"
            |> Map.add InDevelopment "in_development"
            |> Map.add CloudSaves "cloud_saves"
            |> Map.add Overlay "overlay"

        let private reverseFeatureMap =
            (Map.empty, featureMap)
            ||> Map.fold (fun rMap feature str ->
                match Map.containsKey str rMap with
                | false -> rMap |> Map.add str feature
                | true -> failwithf "The identifier of a feature is duplicated: %s" str)

        let toString feature =
            match feature with
            | Custom feature -> feature
            | feature when Map.containsKey feature featureMap -> Map.find feature featureMap
            | _ -> failwithf "GameFeature case not handled: %A" feature

        let fromString str =
            match str with
            | str when reverseFeatureMap.ContainsKey str -> Map.find str reverseFeatureMap
            | str -> Custom str
