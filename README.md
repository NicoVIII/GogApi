# GogApi.DotNet

[![Project Status: Active – The project has reached a stable, usable state and is being actively developed.](https://www.repostatus.org/badges/latest/active.svg)](https://www.repostatus.org/#active)
[![GitHub Release](https://img.shields.io/github/release/NicoVIII/GogApi.DotNet.svg)](https://github.com/NicoVIII/GogApi.DotNet/releases/latest)
[![Github Pre-Release](https://img.shields.io/github/release/NicoVIII/GogApi.DotNet/all.svg?label=prerelease)](https://github.com/NicoVIII/GogApi.DotNet/releases)
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/075c69d86f154b40bef949483e04b98c?branch=production)](https://app.codacy.com/manual/NicoVIII/GogApi.Dotnet/dashboard?bid=14418205)
[![GitHub License](https://img.shields.io/badge/license-MIT-blue.svg)](https://raw.githubusercontent.com/NicoVIII/GogApi.DotNet/master/LICENSE.txt)

This project aims at providing an interface to use the (unofficial) GOG API documented at <https://www.gog.com/forum/general/unofficial_gog_api_documentation/page1> from .NET (atm only for F#).

Documentation can be found at <https://nicoviii.github.io/GogApi.DotNet>.

## Development

[![Build Status](https://github.com/NicoVIII/GogApi.DotNet/workflows/Continuous%20Integration/badge.svg)](https://github.com/NicoVIII/GogApi.DotNet/actions)
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/075c69d86f154b40bef949483e04b98c?branch=master)](https://app.codacy.com/manual/NicoVIII/GogApi.Dotnet/dashboard?bid=14410917)

### How to build application

1. Make sure you've installed .Net Core version defined in [global.json](global.json)
2. Run `dotnet tool install` to install all developer tools required to build the project
3. Run `dotnet fake build` to build default target of [build script](build.fsx)
4. To run tests use `dotnet fake build -t Test`
5. To build documentation use `dotnet fake build -t Docs`

### How to release.

Create release.cmd or release.sh file (already git-ignored) with following content (sample from `sh`, but `cmd` file is similar):

```
#! /bin/bash
export NUGET_KEY=YOUR_NUGET_KEY
export GITHUB_USER=YOUR_GH_USERNAME
export GITHUB_PW=YOUR_GH_PASSWORD_OR_ACCESS_TOKEN

dotnet tool restore
dotnet fake build --target Release
```

If you want to release a version you have to:

-   Adjust RELEASE_NOTES.md
-   Check if SECURITY.md has to be updated
-   Run release.sh

### How to contribute

_Imposter syndrome disclaimer_: I want your help. No really, I do.

There might be a little voice inside that tells you you're not ready; that you need to do one more tutorial, or learn another framework, or write a few more blog posts before you can help me with this project.

I assure you, that's not the case.

This project has some clear Contribution Guidelines and expectations that you can [read here](CONTRIBUTING.md).

The contribution guidelines outline the process that you'll need to follow to get a patch merged. By making expectations and process explicit, I hope it will make it easier for you to contribute.

And you don't just have to write code. You can help out by writing documentation, tests, or even by giving feedback about this work. (And yes, that includes giving feedback about the contribution guidelines.)

Thank you for contributing!

### Contributing and copyright

The project is hosted on [GitHub](https://github.com/NicoVIII/GogApi.DotNet) where you can report issues, fork
the project and submit pull requests.

The library is available under [MIT license](LICENSE.md), which allows modification and redistribution for both commercial and non-commercial purposes.

Please note that this project is released with a [Contributor Code of Conduct](CODE_OF_CONDUCT.md). By participating in this project you agree to abide by its terms.
