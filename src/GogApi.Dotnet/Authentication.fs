namespace GogApi.DotNet.FSharp

open GogApi.DotNet.FSharp.Types
open GogApi.DotNet.FSharp.Request

open System

module Authentication =
    let redirectUri = "https://embed.gog.com/on_login_success?origin=client"

    type TokenResponse =
        { expires_in: int
          scope: string
          token_type: string
          access_token: string
          user_id: string
          refresh_token: string
          session_id: string }

    let createAuth response =
        match response with
        | Ok response ->
            Auth
                { accessToken = response.access_token
                  refreshToken = response.refresh_token
                  // Safety second to avoid errors through code execution duration
                  accessExpires =
                      response.expires_in - 1
                      |> float
                      |> DateTimeOffset.Now.AddSeconds }
        | Error _ -> NoAuth

    let private getBasicQueries() =
        [ createQuery "client_id" "46899977096215655"
          createQuery "client_secret" "9d85c43b1482497dbbce61f6e4aa173a433796eeae2ca8c5f6129f2dc4de46d9" ]

    let newToken (code: string) =
        async {
            let! result = getBasicQueries()
                          |> List.append [ createQuery "grant_type" "authorization_code" ]
                          |> List.append [ createQuery "code" code ]
                          |> List.append [ createQuery "redirect_uri" redirectUri ]
                          |> makeRequest<TokenResponse> NoAuth
                          <| "https://auth.gog.com/token"
            return createAuth result
        }

    let refresh authentication =
        async {
            let! result = getBasicQueries()
                          |> List.append [ createQuery "grant_type" "refresh_token" ]
                          |> List.append [ createQuery "refresh_token" authentication.refreshToken ]
                          |> makeRequest<TokenResponse> NoAuth
                          <| "https://auth.gog.com/token"
            return createAuth result
        }

    let refreshAuthentication authentication =
        async {
            // Refresh authentication, when old one expired
            let! authentication = match authentication with
                                  | Auth authenticationData when authenticationData.accessExpires
                                                                 |> DateTimeOffset.Now.CompareTo
                                                                 >= 0 -> refresh authenticationData
                                  | _ -> async { return authentication }
            return authentication
        }
