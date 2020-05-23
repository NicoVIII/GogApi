namespace GogApi.DotNet.FSharp.DomainTypes

/// <summary>
/// All possible OSes with a custom field for new OSes which are not
/// in this API wrapper yet
/// </summary>
type OS =
    | WindowsXP
    | WindowsVista
    | Windows7
    | Windows8
    | Windows10
    | OSX106
    | OSX107
    | Ubuntu
    | Mint
    | Ubuntu18
    | Custom of string

/// <summary>
/// Contains some helper function for <see cref="T:GogApi.DotNet.FSharp.DomainTypes.OS"/>
/// </summary>
module OS =
    let private systemMap =
        Map.empty
        |> Map.add WindowsXP "windows_xp"
        |> Map.add WindowsVista "windows_vista"
        |> Map.add Windows7 "windows_7"
        |> Map.add Windows8 "windows_8"
        |> Map.add Windows10 "windows_10"
        |> Map.add OSX106 "osx_106"
        |> Map.add OSX107 "osx_107"
        |> Map.add Ubuntu "lin_ubuntu"
        |> Map.add Mint "lin_mint"
        |> Map.add Ubuntu18 "lin_ubuntu_18"

    let private reverseSystemMap =
        (Map.empty, systemMap)
        ||> Map.fold (fun rMap system str ->
                match Map.containsKey str rMap with
                | false -> rMap |> Map.add str system
                | true -> failwithf "The identifier of a system is duplicated: %s" str)

    /// <summary>
    /// Converts a <see cref="T:GogApi.DotNet.FSharp.DomainTypes.OS"/> into its string representation
    /// </summary>
    let toString sort =
        match sort with
        | Custom sort -> sort
        | sort when Map.containsKey sort systemMap -> Map.find sort systemMap
        | _ -> failwithf "System case not handled: %A" sort

    /// <summary>
    /// Converts a string representation back to a <see cref="T:GogApi.DotNet.FSharp.DomainTypes.OS"/>
    /// </summary>
    let fromString str =
        match str with
        | str when Map.containsKey str reverseSystemMap -> Map.find str reverseSystemMap
        | str -> Custom str
