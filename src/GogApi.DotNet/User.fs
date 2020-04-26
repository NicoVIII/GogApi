namespace GogApi.DotNet.FSharp

open GogApi.DotNet.FSharp.Request
open GogApi.DotNet.FSharp.Types

/// <summary>
/// Methods used to manage the user’s account
/// </summary>
module User =
    type UserDataResponse =
        { country: string
          currencies: Currency list
          selectedCurrency: Currency
          preferredLanguage: Language
          ratingBrand: string
          isLoggedIn: bool
          checksum: {| cart: string option; games: string option; wishlist: string option; reviews_votes: string option; games_rating: string option |}
          updates: {| messages: int; pendingFriendRequests: int; unreadChatMessages: int; products: int; forum: int; total: int |}
          userId: UserId
          username: string
          email: string
          personalizedProductPrices: obj list
          personalizedSeriesPrices: obj list }

    /// <summary>
    /// Fetches information about the currently authenticated user
    /// </summary>
    let getUserData authentication =
        makeRequest<UserDataResponse> (Some authentication) []
            "https://embed.gog.com/userData.json"
