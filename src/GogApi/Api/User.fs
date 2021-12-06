namespace GogApi

open GogApi.DomainTypes
open Internal.Request
open Internal.Transforms

open FSharp.Json

/// <summary>
/// Methods used to manage the user’s account
/// </summary>
[<RequireQualifiedAccess>]
module User =
    type UserDataResponseInternal =
        { country: string
          currencies: Currency list
          selectedCurrency: Currency
          preferredLanguage: {| code: string; name: string |}
          ratingBrand: string
          isLoggedIn: bool
          checksum: {| cart: string option
                       games: string option
                       wishlist: string option
                       reviews_votes: string option
                       games_rating: string option |}
          updates: {| messages: int
                      pendingFriendRequests: int
                      unreadChatMessages: int
                      products: int
                      total: int |}
          [<JsonField(Transform = typeof<UserIdStringTransform>)>]
          userId: UserId
          username: UserName
          galaxyUserId: string
          email: string
          avatar: string option
          walletBalance: {| currency: string; amount: int |}
          purchasedItems: {| games: int; movies: int |}
          wishlistedItems: int
          friends: FriendInfo list
          personalizedProductPrices: obj list // TODO: #10
          personalizedSeriesPrices: obj list } // TODO: #10

    /// <summary>
    /// Contains info about a user requested via
    /// <see cref="M:GogApi.User.getData"/>
    /// </summary>
    type UserDataResponse =
        { country: string
          currencies: Currency list
          selectedCurrency: Currency
          preferredLanguage: {| code: string
                                name: string
                                language: Language |}
          ratingBrand: string
          isLoggedIn: bool
          checksum: {| cart: string option
                       games: string option
                       wishlist: string option
                       reviews_votes: string option
                       games_rating: string option |}
          updates: {| messages: int
                      pendingFriendRequests: int
                      unreadChatMessages: int
                      products: int
                      total: int |}
          userId: UserId
          username: UserName
          galaxyUserId: string
          avatar: string option
          walletBalance: {| currency: string; amount: int |}
          purchasedItems: {| games: int; movies: int |}
          wishlistedItems: int
          friends: FriendInfo list
          email: string
          personalizedProductPrices: obj list // TODO: #10
          personalizedSeriesPrices: obj list } // TODO: #10

    let private fromInternalDataResponse (internalResponse: UserDataResponseInternal) =
        { country = internalResponse.country
          currencies = internalResponse.currencies
          selectedCurrency = internalResponse.selectedCurrency
          preferredLanguage =
            {| code = internalResponse.preferredLanguage.code
               name = internalResponse.preferredLanguage.name
               language =
                internalResponse.preferredLanguage.code
                |> Language.fromString |}
          ratingBrand = internalResponse.ratingBrand
          isLoggedIn = internalResponse.isLoggedIn
          checksum = internalResponse.checksum
          updates = internalResponse.updates
          userId = internalResponse.userId
          username = internalResponse.username
          galaxyUserId = internalResponse.galaxyUserId
          avatar = internalResponse.avatar
          walletBalance = internalResponse.walletBalance
          purchasedItems = internalResponse.purchasedItems
          wishlistedItems = internalResponse.wishlistedItems
          friends = internalResponse.friends
          email = internalResponse.email
          personalizedProductPrices = internalResponse.personalizedProductPrices
          personalizedSeriesPrices = internalResponse.personalizedSeriesPrices }

    /// <summary>
    /// Fetches information about the currently authenticated user
    /// </summary>
    let getData authentication =
        async {
            let! response =
                makeRequest<UserDataResponseInternal> (Some authentication) [] "https://embed.gog.com/userData.json"

            return response |> Result.map fromInternalDataResponse
        }

    /// <summary>
    /// Contains information about owned games requested via <see cref="M:GogApi.User.getDataGames"/>
    /// </summary>
    type DataGamesResponse = { owned: ProductId list }

    /// <summary>
    /// Fetches a list of ids of the games and movies the authenticated account owns
    /// </summary>
    let getDataGames authentication =
        makeRequest<DataGamesResponse> (Some authentication) [] "https://embed.gog.com/user/data/games"

    /// <summary>
    /// Contains info about a wishlist requested via <see cref="M:GogApi.User.getWishlist"/>
    /// </summary>
    type WishlistResponse =
        { [<JsonField(Transform = typeof<ProductIdBoolMapStringTransform>)>]
          wishlist: Map<ProductId, bool>
          checksum: string }

    /// <summary>
    /// Fetches information about the wishlist of the current user
    /// </summary>
    let getWishlist authentication =
        makeRequest<WishlistResponse> (Some authentication) [] "https://embed.gog.com/user/wishlist.json"
