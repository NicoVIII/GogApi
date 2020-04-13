namespace GogApi.DotNet.FSharp

open GogApi.DotNet.FSharp.Request

module User =
    type UserDataResponse =
        { username: string
          email: string }

    let getUserData authentication =
        makeRequest<UserDataResponse> (Some authentication) []
            "https://embed.gog.com/userData.json"
