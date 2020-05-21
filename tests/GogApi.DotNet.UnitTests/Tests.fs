module GogApi.DotNet.UnitTests.Tests

open DiscriminatedUnionHelper

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
    ]
