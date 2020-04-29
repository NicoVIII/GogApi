namespace GogApi.DotNet.FSharp

open FSharp.Json

open GogApi.DotNet.FSharp.Request
open GogApi.DotNet.FSharp.Transforms
open GogApi.DotNet.FSharp.Types

/// <summary>
/// This module holds all API calls which has to do with your wishlist
/// </summary>
module Wishlist =
    type WishlistResponse =
        { [<JsonField(Transform=typeof<GameIdBoolMapStringTransform>)>]
          wishlist: Map<GameId, bool>
          checksum: string }

    /// <summary>
    /// Fetches a list of game ids of the games the authenticated account owns
    /// </summary>
    let getWishlist authentication =
        makeRequest<WishlistResponse> (Some authentication) []
            "https://embed.gog.com/user/wishlist.json"
