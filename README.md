# GogApi

[![GitHub Release](https://img.shields.io/github/release/NicoVIII/GogApi.svg)](https://github.com/NicoVIII/GogApi/releases/latest)
[![Last commit](https://img.shields.io/github/last-commit/NicoVIII/GogApi)

This project aims at providing an interface to use the (unofficial) GOG API documented at <https://www.gog.com/forum/general/unofficial_gog_api_documentation/page1> from .NET (atm only for F#).

Documentation can be found at <https://nicoviii.github.io/GogApi>.

## Development

To develop this library, please use the provided Devcontainer with VSCode. You just need VSCode and
Docker installed, everything else is inside the container.

### How to build application

1. Run `dotnet run build` to build
2. To run tests use `dotnet run test`

## How to work with documentation

1. Run `dotnet run build` to build
2. Build documentation to make sure everything is fine with `dotnet run docs`
3. Start Fornax in watch mode `dotnet run watch-docs`
4. Your documentation should now be accessible on `localhost:8080` and will be regenerated on every file save

### How to contribute

_Imposter syndrome disclaimer_: I want your help. No really, I do.

There might be a little voice inside that tells you you're not ready; that you need to do one more tutorial, or learn another framework, or write a few more blog posts before you can help me with this project.

I assure you, that's not the case.

This project has some clear Contribution Guidelines and expectations that you can [read here](CONTRIBUTING.md).

The contribution guidelines outline the process that you'll need to follow to get a patch merged. By making expectations and process explicit, I hope it will make it easier for you to contribute.

And you don't just have to write code. You can help out by writing documentation, tests, or even by giving feedback about this work. (And yes, that includes giving feedback about the contribution guidelines.)

Thank you for contributing!

### Contributing and copyright

The project is hosted on [GitHub](https://github.com/NicoVIII/GogApi) where you can report issues, fork
the project and submit pull requests.

The library is available under [MIT license](LICENSE.md), which allows modification and redistribution for both commercial and non-commercial purposes.

Please note that this project is released with a [Contributor Code of Conduct](CODE_OF_CONDUCT.md). By participating in this project you agree to abide by its terms.
