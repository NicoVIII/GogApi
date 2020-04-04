#r "../_lib/Fornax.Core.dll"

type ApiReferences = {
    title: string
    link: string
}

type Shortcut = {
    title: string
    link: string
    icon: string
}

let loader (projectRoot: string) (siteContent: SiteContents) =
    siteContent.Add({title = "API reference"; link = "/Reference/apiRef.html"})
    siteContent.Add({title = "Home"; link = "/"; icon = "fas fa-home"})
    siteContent.Add({title = "GitHub repo"; link = "https://github.com/NicoVIII/GogApi.DotNet"; icon = "fab fa-github"})
    siteContent.Add({title = "License"; link = "/license.html"; icon = "far fa-file-alt"})
    siteContent
