namespace GogApi.DotNet.FSharp

open GogApi.DotNet.FSharp.Request

/// <summary>
/// Methods used to manage the user’s account
/// </summary>
module User =
    type UserDataResponse =
        { username: string
          email: string }

    /// <summary>
    /// Fetches information about the currently authenticated user
    /// </summary>
    let getUserData authentication =
        makeRequest<UserDataResponse> (Some authentication) []
            "https://embed.gog.com/userData.json"
