namespace GogApi.DotNet.FSharp

open GogApi.DotNet.FSharp.Types
open GogApi.DotNet.FSharp.Request

open System

/// <summary>
/// This module holds everything which is needed to authenticate to the API
/// </summary>
module Authentication =
    /// <summary>
    /// Typesafe variant of the response data of https://auth.gog.com/token
    /// </summary>
    type TokenResponse =
        { expires_in: int
          scope: string
          token_type: string
          access_token: string
          user_id: string
          refresh_token: string
          session_id: string }

    /// <summary>
    /// Creates a new authentication from a TokenResponse
    /// </summary>
    let createAuth response =
        match response with
        | Ok response ->
            Some
                { accessToken = response.access_token
                  refreshToken = response.refresh_token
                  // Safety second to avoid errors through code execution duration
                  accessExpires =
                      response.expires_in
                      - 1
                      |> float
                      |> DateTimeOffset.Now.AddSeconds }
        | Error _ -> None

    let private getBasicParameters () =
        [ createRequestParameter "client_id" "46899977096215655"
          createRequestParameter "client_secret"
              "9d85c43b1482497dbbce61f6e4aa173a433796eeae2ca8c5f6129f2dc4de46d9" ]

    /// <summary>
    /// Uses authentication code to create a Authentication with authorization
    /// token
    /// </summary>
    let getNewToken (redirectUri: string) (code: string) =
        async {
            let! result =
                getBasicParameters ()
                |> List.append
                    [ createRequestParameter "grant_type" "authorization_code" ]
                |> List.append [ createRequestParameter "code" code ]
                |> List.append [ createRequestParameter "redirect_uri" redirectUri ]
                |> makeRequest<TokenResponse> None
                <| "https://auth.gog.com/token"
            return createAuth result
        }

    /// <summary>
    /// Tries to refresh the current authentication.
    /// </summary>
    /// <returns>
    /// - New Authentication, if token in given authentication expired
    /// - otherwise the input
    /// </returns>
    let getRefreshToken authentication =
        async {
            let! response =
                getBasicParameters ()
                |> List.append [ createRequestParameter "grant_type" "refresh_token" ]
                |> List.append
                    [ createRequestParameter "refresh_token" authentication.refreshToken ]
                |> makeRequest<TokenResponse> None
                <| "https://auth.gog.com/token"
            return createAuth response
        }
