namespace GogApi.DotNet.FSharp

open GogApi.DotNet.FSharp.Authentication

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
    let withAutoRefresh apiFnc authentication =
        async {
            let! authentication = refreshAuthentication authentication

            // Execute API function
            let! fncResult = apiFnc authentication
            return (fncResult, authentication) }
