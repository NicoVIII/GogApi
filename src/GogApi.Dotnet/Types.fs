namespace GogApi.DotNet.FSharp

open System

module Types =
    type QueryParameter =
        { name: string
          value: string }

    type AuthenticationData =
        { accessToken: string
          refreshToken: string
          accessExpires: DateTimeOffset }

    type Authentication =
        | NoAuth
        | Auth of AuthenticationData
