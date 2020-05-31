---
title: Authentication
category: how-to
menu_order: 1
---

# Authentication

You can read about the Authflow here:
<https://gogapidocs.readthedocs.io/en/latest/auth.html>

In this guide I will show you how this is effectivly done with this library in F#.

## Request authentication code

At first you need to request a authentication code. For that you need some kind of embedded
browser and point the browser to an url like:  
<https://login.gog.com/auth?client_id=46899977096215655&layout=client2&redirect_uri=__REDIRECT_URI__&response_type=code>

I use always `https://embed.gog.com/on_login_success?origin=client` as redirect uri and therefore the link is:
<https://login.gog.com/auth?client_id=46899977096215655&layout=client2&redirect_uri=https%3A%2F%2Fembed.gog.com%2Fon_login_success%3Forigin%3Dclient&response_type=code>

You can find the documentation for that endpoint here:  
<https://gogapidocs.readthedocs.io/en/latest/auth.html#methods>

The user has to login there and is redirected to an url like this:

    https://embed.gog.com/on_login_success?origin=client&code=iUZDhNPZNpQkQEPS9OqEUfyx-iNMpJT-W_AY9Miw8lpYnmkPOzE908Xp33t4cNXRUCyULAXB3koWEDFhQUzfboRrlZiKxTddDBiw-fnutQcAxmicbjdWdPnPuyZTnMC1tEsg2jZMhoeTvDt66Q5SP8idHGvqq8Z04AwOU09vkUw

The important part here is the `code=..` parameter. In this example this is

    iUZDhNPZNpQkQEPS9OqEUfyx-iNMpJT-W_AY9Miw8lpYnmkPOzE908Xp33t4cNXRUCyULAXB3koWEDFhQUzfboRrlZiKxTddDBiw-fnutQcAxmicbjdWdPnPuyZTnMC1tEsg2jZMhoeTvDt66Q5SP8idHGvqq8Z04AwOU09vkUw

This is the authentication code you need for the next step.

## Request authentication token

For this you need the used redirect uri and authentication code from the previous step. This is used to request
an authentication token, which is used to authenticate your API calls.

```fsharp
open GogApi.DotNet.FSharp

// let redirectUri = <RedirectUri>
// let authCode = <AuthenticationCode>

let newTokenAsync = Authentication.getNewToken redirectUri authCode
// newTokenAsync: Async<Authentication>
```

From here you can fire the call to the API in any way you see fitting.
E.g. you could use `Async.RunSynchronously` or if you use Elmish something like
`Cmd.OfAsync`.

The `Authentication` you get from that has to be used for the other API calls.

## Refresh authentication

Because the access token you get from GOG is only valid for a limited time, you
have to regularly refresh your tokens. You can do this manually by calling
`Authentication.getRefreshToken` or use a helper function which automatically
refreshes the authentication when the tokens validity runs out.

You can use this to autorefresh before any API call:

```fsharp
Authentication.withAutoRefresh(Games.getAvailableGamesForSearch search) authentication
```
