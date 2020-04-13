namespace GogApi.DotNet.FSharp

open GogApi.DotNet.FSharp

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
    let autoRefresh authentication =
        async {
            // Refresh authentication only, when old one expired
            let oldTokenExpired =
                authentication.accessExpires
                |> DateTimeOffset.Now.CompareTo
                >= 0

            let authentication =
                if oldTokenExpired then
                    let! authentication = Authentication.getRefreshToken authentication
                    authentication
                else
                    Some authentication

            // Execute API function
            let! fncResult = match authentication with
                             | Some authentication ->
                                 async {
                                     let! fncResult = apiFnc authentication
                                     return fncResult }
                             | None ->
                                 // TODO: think again about error handling in the whole API
                                 async { failwith "Didn't get valid authentication!" }
            return (fncResult, authentication)
        }
