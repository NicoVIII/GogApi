namespace GogApi.DomainTypes

open GogApi.Internal.Transforms

open FSharp.Json
open System

/// Contains info about a Dlc for a game
type Dlc = {
    title: string
    backgroundImage: string
    cdKey: string
    textInformation: string
    [<JsonField(Transform = typeof<DownloadsObjListTransform>)>]
    downloads: Map<string, Download>
}
