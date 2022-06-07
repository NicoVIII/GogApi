open System
open System.IO

open Fake.IO

open RunHelpers
open RunHelpers.Shortcuts
open RunHelpers.Templates

[<RequireQualifiedAccess>]
module private Config =
    let runProject = "./src/GogApi.Cli/GogApi.Cli.fsproj"

    let libProject = "./src/GogApi/GogApi.fsproj"

    let testFolder = "./tests"

    let packPath = "./deploy"

[<AutoOpen>]
module private Types =
    type BuildMode =
        | Debug
        | Release

module private Task =
    let restore () =
        job {
            DotNet.toolRestore ()
            DotNet.restore Config.runProject
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
        Shell.deleteDir "./output"

        dotnet [ "fsdocs"; "build" ]

    let docsWatch () =
        Shell.deleteDir "./tmp"

        dotnet [ "fsdocs"; "watch" ]

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

        Shell.deleteDir outDir

        dotnet [ "pack"
                 "-c"
                 "Release"
                 "-o"
                 outDir
                 $"/p:Version=%s{version}"
                 Config.libProject ]

module private Command =
    let restore () = Task.restore ()

    let build () =
        job {
            Task.restore ()
            Task.build Debug
        }

    let docs () =
        job {
            Task.restore ()
            Task.build Debug
            Task.docs ()
        }

    let docsWatch () =
        job {
            Task.restore ()
            Task.build Debug
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
            Job.error [ "Usage: dotnet run [<command>]"
                        "Look up available commands at the bottom of run.fs" ]
    |> Job.execute
