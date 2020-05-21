namespace GogApi.DotNet.FSharp

open GogApi.DotNet.FSharp.DomainTypes

open FSharp.Json
open System

/// <summary>
/// Contains all transforms which are necessary to parse json answers
/// </summary>
module Transforms =
    type UserIdStringTransform() =
        interface ITypeTransform with
            member __.targetType() = (fun _ -> typeof<string>)()

            member __.toTargetType value =
                (fun (v: obj) ->
                    (v :?> UserId)
                    |> (fun (UserId userId) -> userId)
                    |> string :> obj) value

            member __.fromTargetType value =
                (fun (v: obj) ->
                    v :?> string
                    |> uint64
                    |> UserId :> obj) value

    type GameIdBoolMapStringTransform() =
        interface ITypeTransform with
            member __.targetType() = (fun _ -> typeof<Map<string,bool>>)()

            member __.toTargetType _ =
                failwith "This is a one way transform only!"

            member __.fromTargetType value =
                (fun (v: obj) ->
                    v :?> Map<string,bool>
                    |> Map.fold (fun map key value -> map |> Map.add (key |> uint32 |> ProductId) value) Map.empty
                    :> obj) value

    let private extractDownloadOSInfoList (downloadMap: Map<string, obj>) key =
        let mapList = downloadMap.TryFind key
        match mapList with
        | Some mapList ->
            mapList :?> obj list
            |> List.map (fun (map: obj) ->
                let map = map :?> Map<string, obj>
                { date = map.Item "date" :?> string
                  downloaderUrl =
                      map.TryFind "downloaderUrl" |> Option.map (fun x -> x :?> string)
                  manualUrl = map.Item "manualUrl" :?> string
                  name = map.Item "name" :?> string
                  size = map.Item "size" :?> string
                  version = map.TryFind "version" |> Option.map (fun x -> x :?> string) })
        | None -> []

    let private objListToDownloadInfo (map: Map<string, Download>) (objList: obj list) =
        let downloadMap = objList.[1] :?> Map<string, obj>

        let download =
            { linux = extractDownloadOSInfoList downloadMap "linux"
              mac = extractDownloadOSInfoList downloadMap "mac"
              windows = extractDownloadOSInfoList downloadMap "windows" }
        map.Add(objList.[0] |> string, download)

    type DownloadsObjListTransform() =
        interface ITypeTransform with
            member __.targetType() = (fun _ -> typeof<list<list<obj>>>)()

            member __.toTargetType _ =
                failwith "This is a one way transform only!"

            member __.fromTargetType value =
                value :?> list<list<obj>> |> List.fold objListToDownloadInfo Map.empty :> obj
