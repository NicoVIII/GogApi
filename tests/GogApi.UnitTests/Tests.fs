module GogApi.UnitTests.Tests

open Expecto
open GogApi.DomainTypes

let tests =
    testList
        "DU to/from string tests"
        [ testProperty "GameFeature to/from string" (fun feature ->
              let actual =
                  feature
                  |> GameFeature.toString
                  |> GameFeature.fromString

              actual = feature)
          testProperty "Language to/from string" (fun language ->
              let actual =
                  language
                  |> Language.toString
                  |> Language.fromString

              actual = language)
          testProperty "Sort to/from string" (fun sort ->
              let actual = sort |> Sort.toString |> Sort.fromString
              actual = sort)
          testProperty "OS to/from string" (fun system ->
              let actual = system |> OS.toString |> OS.fromString
              actual = system) ]
