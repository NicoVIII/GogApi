namespace GogApi.Cli

open System

module Pattern =
    let (|Int|_|) (str: string) =
        match Int32.TryParse str with
        | true, number -> Some number
        | _ -> None

    let (|UInt|_|) (str: string) =
        match UInt32.TryParse str with
        | true, number -> Some number
        | _ -> None

    let (|UInt64|_|) (str: string) =
        match UInt64.TryParse str with
        | true, number -> Some number
        | _ -> None

    let (|NonEmptyString|_|) (str: string) =
        match str with
        | "" -> None
        | str -> Some str
