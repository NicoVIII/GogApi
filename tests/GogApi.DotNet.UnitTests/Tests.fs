module GogApi.DotNet.UnitTests.Tests

open Expecto
open GogApi.DotNet.FSharp.DomainTypes

let tests =
    testList "DU to/from string tests" [
        testProperty "GameFeature to/from string" (fun feature ->
            let actual =
                feature
                |> GameFeature.toString
                |> GameFeature.fromString
            actual = feature
        )
        testProperty "Language to/from string" (fun language ->
            let actual =
                language
                |> Language.toString
                |> Language.fromString
            actual = language
        )
        testProperty "Sort to/from string" (fun language ->
            let actual =
                language
                |> Sort.toString
                |> Sort.fromString
            actual = language
        )
    ]
