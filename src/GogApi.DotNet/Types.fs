namespace GogApi.DotNet.FSharp

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
    type UserId = UserId of int

    type Currency =
        { code: string
          symbol: string }

    type Language =
        { code: string
          name: string }
