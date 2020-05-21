namespace GogApi.DotNet.FSharp.DomainTypes

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

    let toString sort =
        match sort with
        | Custom sort -> sort
        | sort when Map.containsKey sort systemMap -> Map.find sort systemMap
        | _ -> failwithf "System case not handled: %A" sort

    let fromString str =
        match str with
        | str when Map.containsKey str reverseSystemMap -> Map.find str reverseSystemMap
        | str -> Custom str
