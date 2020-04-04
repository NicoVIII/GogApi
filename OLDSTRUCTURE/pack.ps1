param (
    [Parameter(Mandatory = $true)][string]$Version
)

# Set correct version
$projectFile = "src/GogApi.DotNet/FSharp/GogApi.DotNet.fsproj"
(Get-Content $projectFile).replace('$version$', $Version) | Set-Content $projectFile
(Get-Content $projectFile).replace('<!--<Version>', '<Version>') | Set-Content $projectFile
(Get-Content $projectFile).replace('</Version>-->', '</Version>') | Set-Content $projectFile

# Pack as NugetPackage
dotnet tool restore
dotnet paket restore
dotnet pack src/GogApi.DotNet/FSharp -c Release -o ../../..
