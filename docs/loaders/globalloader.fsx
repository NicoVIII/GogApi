#r "../_lib/Fornax.Core.dll"

type SiteInfo = {
    title: string
    description: string
    theme_variant: string option
    root_url: string
}

let config = {
    title = "GogApi.DotNet"
    description = "This project aims at providing an interface to use the (unofficial) GOG API from .NET."
    theme_variant = Some "blue"
    root_url =
      #if WATCH
        "http://localhost:8080/"
      #else
        "https://nicoviii.github.io/GogApi.DotNet"
      #endif
}

let loader (_: string) (siteContent: SiteContents) =
    siteContent.Add(config)
    siteContent
