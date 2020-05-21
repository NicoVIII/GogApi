namespace GogApi.DotNet.UnitTests

open Tests

open Expecto
open System

module Program =
    [<EntryPoint>]
    let main args =
        runTestsWithArgs defaultConfig args tests
