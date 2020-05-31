namespace GogApi.DotNet.FSharp.DomainTypes

/// <summary>
/// All possible game features with a custom field for new fields which are not
/// in this API wrapper yet
/// </summary>
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

/// <summary>
/// Contains some helper function for <see cref="T:GogApi.DotNet.FSharp.DomainTypes.GameFeature"/>
/// </summary>
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

    /// <summary>
    /// Converts a <see cref="T:GogApi.DotNet.FSharp.DomainTypes.GameFeature"/> into its string representation
    /// </summary>
    let toString feature =
        match feature with
        | Custom feature -> feature
        | feature when Map.containsKey feature featureMap -> Map.find feature featureMap
        | _ -> failwithf "GameFeature case not handled: %A" feature

    /// <summary>
    /// Converts a string representation back to a <see cref="T:GogApi.DotNet.FSharp.DomainTypes.GameFeature"/>
    /// </summary>
    let fromString str =
        match str with
        | str when Map.containsKey str reverseFeatureMap -> Map.find str reverseFeatureMap
        | str -> Custom str
