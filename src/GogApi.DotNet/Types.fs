namespace GogApi.DotNet.FSharp

open FSharp.Json
open System

/// <summary>
/// Contains all basic types which are needed in the whole domain
/// </summary>
module Types =
    /// <summary>
    /// Data which is needed to authenticate for the API
    /// </summary>
    type Authentication =
        { accessToken: string
          refreshToken: string
          accessExpires: DateTimeOffset }

    type GameId = GameId of int

    type UserId = UserId of uint64

    type UserName = UserName of string

    type FriendInfo =
        { username: UserName
          userSince: int
          galaxyId: string
          avatar: string }

    type Currency =
        { code: string
          symbol: string }

    type Language =
        { code: string
          name: string }

    type UserIdStringTransform() =
        interface ITypeTransform with
            member x.targetType () = (fun _ -> typeof<string>) ()
            member x.toTargetType value = (fun (v: obj) -> (v :?> UserId) |> (fun (UserId userId) -> userId) |> string :> obj) value
            member x.fromTargetType value = (fun (v: obj) -> v :?> string |> uint64 |> UserId :> obj) value
