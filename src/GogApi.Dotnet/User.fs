module GogApi.DotNet.FSharp.User

type UserDataResponse =
    { username: string
      email: string }

let askForUserData authentication =
    makeRequest<UserDataResponse> authentication [] "https://embed.gog.com/userData.json"
