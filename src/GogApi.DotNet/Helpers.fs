namespace GogApi.DotNet.FSharp

open GogApi.DotNet.FSharp.Types
open System

/// <summary>
/// This module holds everything which isn't an API call but can help with handling
/// them
/// </summary>
module Helpers =
    /// <summary>
    /// This helper function is used to extend every API call function and
    /// provide auto refresh, if the current authentication token is not valid
    /// anymore.
    /// </summary>
    /// <param name="apiFnc">This is one of the regular functions of the API
    /// which should be extended</param>
    /// <param name="authentication">Authentication which should be used for
    /// this request</param>
    /// <returns>Async which after execution holds API response and current
    /// Authentication (could change because of the refresh)</returns>
    let withAutoRefresh apiFnc (authentication: Authentication) =
        async {
            // Refresh authentication only, when old one nearly expired
            let oldTokenExpired =
                authentication.accessExpires
                |> DateTimeOffset.Now.AddMinutes(-1.0).CompareTo
                >= 0

            let! authentication =
                if oldTokenExpired then
                    Authentication.getRefreshToken authentication
                else
                    Some authentication |> async.Return

            // Execute API function
            let! fncResult = match authentication with
                             | Some authentication ->
                                 async { return! apiFnc authentication }
                             | None ->
                                 // I guess it's okay to throw an exception here
                                 // Only if the authentication wasn't valid before
                                 // this should fail (or GOG is down or something?)
                                 failwith "Didn't get valid authentication!"
            return (fncResult, authentication.Value)
        }
