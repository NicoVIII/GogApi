namespace GogApi.DomainTypes

/// <summary>
/// All possible languages with a custom field for new languages which are not
/// in this API wrapper yet
/// </summary>
type Language =
    | English
    | German
    | Frensh
    | Spanish
    | Italian
    | PortugueseBrazil
    | Portuguese
    | Russian
    | Polish
    | Japanese
    | Czech
    | Dutch
    | Chinese
    | Korean
    | Turkish
    | Hungarian
    | Swedish
    | Finnish
    | Norwegian
    | Danish
    | Custom of string

/// <summary>
/// Contains some helper function for <see cref="T:GogApi.DomainTypes.Language"/>
/// </summary>
module Language =
    let private languageMap =
        Map.empty
        |> Map.add English "en"
        |> Map.add German "de"
        |> Map.add Frensh "fr"
        |> Map.add Spanish "es"
        |> Map.add Italian "it"
        |> Map.add PortugueseBrazil "br"
        |> Map.add Portuguese "pt"
        |> Map.add Russian "ru"
        |> Map.add Polish "pl"
        |> Map.add Japanese "jp"
        |> Map.add Czech "cz"
        |> Map.add Dutch "nl"
        |> Map.add Chinese "cn"
        |> Map.add Korean "ko"
        |> Map.add Turkish "tr"
        |> Map.add Hungarian "hu"
        |> Map.add Swedish "sv"
        |> Map.add Finnish "fi"
        |> Map.add Norwegian "no"
        |> Map.add Danish "da"

    let private reverseLanguageMap =
        (Map.empty, languageMap)
        ||> Map.fold (fun rMap language str ->
            match Map.containsKey str rMap with
            | false -> rMap |> Map.add str language
            | true -> failwithf "The identifier of a language is duplicated: %s" str)

    /// <summary>
    /// Converts a <see cref="T:GogApi.DomainTypes.Language"/> into its string representation
    /// </summary>
    let toString language =
        match language with
        | Custom language -> language
        | language when Map.containsKey language languageMap -> Map.find language languageMap
        | _ -> failwithf "Language case not handled: %A" language

    /// <summary>
    /// Converts a string representation back to a <see cref="T:GogApi.DomainTypes.Language"/>
    /// </summary>
    let fromString str =
        match str with
        | str when Map.containsKey str reverseLanguageMap -> Map.find str reverseLanguageMap
        | str -> Custom str
