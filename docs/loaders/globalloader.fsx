#r "../_lib/Fornax.Core.dll"

type SiteInfo = {
    title: string
    description: string
    theme_variant: string option
    numbers_in_menu: bool
    root_url: string
}

let config = {
    title = "GogApi.DotNet"
    description = "This project aims at providing an interface to use the (unofficial) GOG API from .NET."
    theme_variant = Some "blue"
    numbers_in_menu = true
    root_url = "https://nicoviii.github.io/GogApi.DotNet"
}

let loader (_: string) (siteContent: SiteContents) =
    siteContent.Add(config)
    siteContent
