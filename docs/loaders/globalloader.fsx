#r "../_lib/Fornax.Core.dll"

type UrlRoot = | Root of string
with
  member x.subRoute (route: string) =
    let (Root root) = x
    root.TrimEnd('/') + "/" + route.TrimStart('/')
  member x.subRoutef pattern =
    Printf.kprintf x.subRoute pattern

type SiteInfo = {
    title: string
    description: string
    theme_variant: string option
    root_url: UrlRoot
}

let config = {
    title = "GogApi.DotNet"
    description = "This project aims at providing an interface to use the (unofficial) GOG API from .NET."
    theme_variant = Some "blue"
    root_url =
      #if WATCH
        Root "http://localhost:8080"
      #else
        Root "https://nicoviii.github.io/GogApi.DotNet"
      #endif
}

let loader (_: string) (siteContent: SiteContents) =
    siteContent.Add(config)
    siteContent
