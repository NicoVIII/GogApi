namespace GogApi.DotNet.FSharp

open GogApi.DotNet.FSharp.Authentication

module Helpers =
    let withAutoRefresh apiFnc authentication =
        async {
            let! authentication = refreshAuthentication authentication

            // Execute API function
            let! fncResult = apiFnc authentication
            return (fncResult, authentication) }
