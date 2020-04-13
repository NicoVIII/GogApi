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
