module GogApi.DotNet.FSharp.User

open HttpFs.Client

type UserDataResponse = {
    username: string;
    email: string;
}

let askForUserData auth =
    makeRequest<UserDataResponse> auth [] "https://embed.gog.com/userData.json"
