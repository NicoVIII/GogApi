namespace GogApi.DotNet.FSharp

open GogApi.DotNet.FSharp.DomainTypes
open Request

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
        |> List.concat

    /// <summary>
    /// Uses authentication code to create a Authentication with authorization
    /// token
    /// </summary>
    let getNewToken (redirectUri: string) (code: string) =
        async {
            let queries =
                [ getBasicParameters ()
                  createRequestParameter "grant_type" "authorization_code"
                  createRequestParameter "code" code
                  createRequestParameter "redirect_uri" redirectUri ]
                |> List.concat

            let! result =
                makeRequest<TokenResponse> None queries "https://auth.gog.com/token"
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
            let queries =
                [ getBasicParameters ()
                  createRequestParameter "grant_type" "refresh_token"
                  createRequestParameter "refresh_token" authentication.refreshToken ]
                |> List.concat

            let! response =
                makeRequest<TokenResponse> None queries "https://auth.gog.com/token"
            return createAuth response
        }
