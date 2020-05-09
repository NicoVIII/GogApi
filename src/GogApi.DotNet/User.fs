namespace GogApi.DotNet.FSharp

open FSharp.Json

open GogApi.DotNet.FSharp.Request
open GogApi.DotNet.FSharp.Transforms
open GogApi.DotNet.FSharp.Types

/// <summary>
/// Methods used to manage the user’s account
/// </summary>
[<RequireQualifiedAccess>]
module User =
    type UserDataResponse =
        { country: string
          currencies: Currency list
          selectedCurrency: Currency
          preferredLanguage: Language
          ratingBrand: string
          isLoggedIn: bool
          checksum: {| cart: string option; games: string option; wishlist: string option; reviews_votes: string option; games_rating: string option |}
          updates: {| messages: int; pendingFriendRequests: int; unreadChatMessages: int; products: int; total: int |}
          [<JsonField(Transform = typeof<UserIdStringTransform>)>]
          userId: UserId
          username: UserName
          galaxyUserId: string
          avatar: string option
          walletBalance: {| currency: string; amount: int |}
          purchasedItems: {| games: int; movies: int |}
          wishlistedItems: int
          friends: FriendInfo list
          email: string
          personalizedProductPrices: obj list
          personalizedSeriesPrices: obj list }

    /// <summary>
    /// Fetches information about the currently authenticated user
    /// </summary>
    let getData authentication =
        makeRequest<UserDataResponse> (Some authentication) []
            "https://embed.gog.com/userData.json"

    type DataGamesResponse =
        { owned: GameId list }

    /// <summary>
    /// Fetches a list of game ids of the games the authenticated account owns
    /// </summary>
    let getDataGames authentication =
        makeRequest<DataGamesResponse> (Some authentication) []
            "https://embed.gog.com/user/data/games"

    type WishlistResponse =
        { [<JsonField(Transform = typeof<GameIdBoolMapStringTransform>)>]
          wishlist: Map<GameId, bool>
          checksum: string }

    /// <summary>
    /// TODO:
    /// </summary>
    let getWishlist authentication =
        makeRequest<WishlistResponse> (Some authentication) []
            "https://embed.gog.com/user/wishlist.json"
