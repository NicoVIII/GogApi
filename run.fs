open System
open System.IO

open Fake.Core
open Fake.IO

open RunHelpers
open RunHelpers.BasicShortcuts
open RunHelpers.FakeHelpers

[<RequireQualifiedAccess>]
module Config =
    let runProject = "./src/GogApi.Cli/GogApi.Cli.fsproj"

    let libProject = "./src/GogApi/GogApi.fsproj"

    let docsFolder = "./docs"

    let testFolder = "./tests"

    let packPath = "./deploy"

type BuildMode =
    | Debug
    | Release

module Task =
    open System.IO
    open System.IO

    let restore () =
        job {
            Template.DotNet.toolRestore ()
            Template.DotNet.restore Config.runProject
        }

    let build mode =
        dotnet [ "build"
                 "-c"
                 match mode with
                 | Debug -> "Debug"
                 | Release -> "Release"
                 Config.runProject
                 "--no-restore" ]

    let docs () =
        CreateProcess.fromRawCommand "dotnet" [ "fornax"; "build" ]
        |> CreateProcess.withWorkingDirectory Config.docsFolder
        |> Proc.runAsJob 10

    let docsWatch () =
        CreateProcess.fromRawCommand "dotnet" [ "fornax"; "watch" ]
        |> CreateProcess.withWorkingDirectory Config.docsFolder
        |> Proc.runAsJob 10

    let run () =
        dotnet [ "run"
                 "--project"
                 Config.runProject ]

    let test () =
        let projects =
            Directory.EnumerateFiles(Config.testFolder, "*.fsproj", SearchOption.AllDirectories)

        job {
            for project in projects do
                printfn "\nRun tests in %s:" project

                dotnet [ "run"; "--project"; project ]
        }

    let pack version () =
        let outDir = Config.packPath

        if Directory.Exists outDir then
            Directory.Delete outDir

        dotnet [ "pack"
                 "-c"
                 "Release"
                 "-o"
                 outDir
                 $"/p:Version=%s{version}"
                 Config.libProject ]

module Command =
    let restore () = Task.restore ()

    let build () =
        job {
            Task.restore ()
            Task.build Debug
        }

    let docs () =
        job {
            Task.restore ()
            Task.build Release
            Task.docs ()
        }

    let docsWatch () =
        job {
            Task.restore ()
            Task.build Release
            Task.docsWatch ()
        }

    let run () =
        job {
            Task.restore ()
            Task.run ()
        }

    let test () =
        job {
            Task.restore ()
            Task.test ()
        }

    let pack version () =
        job {
            Task.restore ()
            Task.pack version ()
        }

[<EntryPoint>]
let main args =
    args
    |> List.ofArray
    |> function
        | [ "restore" ] -> Command.restore ()
        | [ "build" ] -> Command.build ()
        | [ "build-docs" ]
        | [ "docs" ] -> Command.docs ()
        | [ "watch-docs" ]
        | [ "docs-watch" ] -> Command.docsWatch ()
        | []
        | [ "run" ] -> Command.run ()
        | [ "test" ] -> Command.test ()
        | [ "pack"; version ] -> Command.pack version ()
        | _ ->
            let msg =
                [ "Usage: dotnet run [<command>]"
                  "Look up available commands at the bottom of run.fs" ]

            Error(1, msg)
    |> ProcessResult.wrapUp
