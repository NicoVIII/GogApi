namespace GogApi.DotNet.FSharp

open FSharp.Json
open System

open GogApi.DotNet.FSharp.Types

/// <summary>
/// Contains all transforms which are necessary to parse json answers
/// </summary>
module Transforms =
    type UserIdStringTransform() =
        interface ITypeTransform with
            member x.targetType() = (fun _ -> typeof<string>)()

            member x.toTargetType value =
                (fun (v: obj) ->
                    (v :?> UserId)
                    |> (fun (UserId userId) -> userId)
                    |> string :> obj) value

            member x.fromTargetType value =
                (fun (v: obj) ->
                    v :?> string
                    |> uint64
                    |> UserId :> obj) value

    let private extractDownloadOSInfoList (downloadMap: Map<string, obj>) key =
        let mapList = downloadMap.TryFind key
        match mapList with
        | Some mapList ->
            mapList
            :?> obj list
            |> List.map (fun (map: obj) ->
                let map = map :?> Map<string, obj>
                { date = map.Item "date" :?> string
                  downloaderUrl = map.Item "downloaderUrl" :?> string
                  manualUrl = map.Item "manualUrl" :?> string
                  name = map.Item "name" :?> string
                  size = map.Item "size" :?> string // TODO: parse this somehow in a number? In additional field?
                  version = map.Item "version" :?> string })
        | None -> []

    let private objListToDownloadInfo (map: Map<string, Download>) (objList: obj list) =
        let downloadMap = objList.[1] :?> Map<string, obj>
        let download =
            { linux = extractDownloadOSInfoList downloadMap "linux"
              osx = extractDownloadOSInfoList downloadMap "osx"
              windows = extractDownloadOSInfoList downloadMap "windows" }
        map.Add(objList.[0] |> string, download)

    type DownloadsObjListTransform() =
        interface ITypeTransform with
            member x.targetType() = (fun _ -> typeof<list<list<obj>>>)()

            member x.toTargetType value = value // TODO: Do I have to implement this?

            member x.fromTargetType value =
                value :?> list<list<obj>> |> List.fold objListToDownloadInfo Map.empty :> obj
