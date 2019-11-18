#!/bin/bash
dotnet tool restore
dotnet paket restore
dotnet build src/GogApi.DotNet/FSharp
