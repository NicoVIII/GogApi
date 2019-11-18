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
open System.IO

// Properties
let projectPath = "./src/GogApi.DotNet/FSharp/"

let forDebug (options:DotNet.BuildOptions) =
  { options with Configuration = DotNet.BuildConfiguration.Debug }

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

// Dependencies
open Fake.Core.TargetOperators

"Clean"
  ==> "BuildApp"

// start build
Target.runOrDefault "BuildApp"
