#r "paket:
nuget Fake.IO.FileSystem
nuget Fake.DotNet.Cli
nuget Fake.Core.Target
nuget FSharp.Formatting //"
#load "./.fake/build.fsx/intellisense.fsx"

open Fake.IO
open Fake.IO.Globbing.Operators //enables !! and globbing
open Fake.DotNet
open Fake.Core
//open FSharp.MetadataFormat
open System.IO

// Properties
let projectPath = "./src/GogApi.DotNet/FSharp/"
let projectFile = "GogApi.DotNet.fsproj"
let projectFilePath = Path.Combine(projectPath, projectFile)

let forDebug (options:DotNet.BuildOptions) =
  { options with Configuration = DotNet.BuildConfiguration.Debug }

let packOptions (options:DotNet.PackOptions) =
    { options with OutputPath = Some __SOURCE_DIRECTORY__ }

// Targets
Target.create "Clean" (fun _ ->
  !! "./src/**/bin/"
    |> Shell.deleteDirs
  !! "./src/**/obj/"
    |> Shell.deleteDirs
)

Target.create "BuildApp" (fun _ ->
  DotNet.build forDebug projectPath
)

Target.create "Pack" (fun _ ->
  // Replace version in project file
  let version = Environment.environVarOrFail "VERSION"
  let replacements = Seq.ofList [
      ("<!--<Version>", "<Version>")
      ("</Version>-->", "</Version>")
      ("$version$", version)
    ]
  let files = Seq.ofList [ projectFilePath ]
  Shell.replaceInFiles replacements files

  DotNet.pack packOptions projectPath
)

Target.create "GenerateDocu" (fun _ -> ()
    (*Path.Combine (projectPath, "bin/GogApi.DotNet.dll")
    |> MetadataFormat.Generate
    |> ignore*)
)

// Dependencies
open Fake.Core.TargetOperators

"Clean"
  ==> "Pack"

"Clean"
  ==> "BuildApp"
  ==> "GenerateDocu"

// start build
Target.runOrDefault "BuildApp"