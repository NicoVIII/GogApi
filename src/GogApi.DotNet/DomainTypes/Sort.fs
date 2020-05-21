namespace GogApi.DotNet.FSharp.DomainTypes

type Sort =
    | BestSelling
    | Alphabetically
    | UserRating
    | DateAdded
    | BestSellingAllTime
    | OldestFirst
    | NewestFirst
    | Custom of string

module Sort =
    let private sortMap =
        Map.empty
        |> Map.add BestSelling "popularity"
        |> Map.add Alphabetically "title"
        |> Map.add UserRating "rating"
        |> Map.add DateAdded "date"
        |> Map.add BestSellingAllTime "bestselling"
        |> Map.add OldestFirst "release_asc"
        |> Map.add NewestFirst "release_desc"

    let private reverseSortMap =
        (Map.empty, sortMap)
        ||> Map.fold (fun rMap sort str ->
            match Map.containsKey str rMap with
            | false -> rMap |> Map.add str sort
            | true -> failwithf "The identifier of a language is duplicated: %s" str)

    let toString sort =
        match sort with
        | Custom sort -> sort
        | sort when Map.containsKey sort sortMap -> Map.find sort sortMap
        | _ -> failwithf "Sort case not handled: %A" sort

    let fromString str =
        match str with
        | str when Map.containsKey str reverseSortMap -> Map.find str reverseSortMap
        | str -> Custom str
