module GogApi.DotNet.FSharp.Authentication

let redirectUri = "https://embed.gog.com/on_login_success?origin=client"

type TokenResponse = {
    expires_in: int;
    scope: string;
    token_type: string;
    access_token: string;
    user_id: string;
    refresh_token: string;
    session_id: string;
}

let createAuth response =
    match response with
    | Ok response ->
        Auth { accessToken = response.access_token; refreshToken = response.refresh_token }
    | Error _ ->
        NoAuth

let private getBasicQueries () =
    [
        createQuery "client_id" "46899977096215655";
        createQuery "client_secret" "9d85c43b1482497dbbce61f6e4aa173a433796eeae2ca8c5f6129f2dc4de46d9";
    ]

let newToken (code :string) =
    async {
        let! result =
            getBasicQueries ()
            |> List.append [ createQuery "grant_type" "authorization_code" ]
            |> List.append [ createQuery "code" code ]
            |> List.append [ createQuery "redirect_uri" redirectUri ]
            |> makeRequest<TokenResponse> NoAuth <| "https://auth.gog.com/token"
        return createAuth result
    }

let refresh auth =
    async {
        let! result =
            getBasicQueries ()
            |> List.append [ createQuery "grant_type" "refresh_token" ]
            |> List.append [ createQuery "refresh_token" auth.refreshToken ]
            |> makeRequest<TokenResponse> NoAuth <| "https://auth.gog.com/token"
        return createAuth result
    }
