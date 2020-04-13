---
layout: post
title: Authentication
author: @nicoviii
published: 2020-04-05
menu_order: 1
---
# Authentication

You can read about the Authflow here:
<https://gogapidocs.readthedocs.io/en/latest/auth.html>

In this guide I will show you how this is effectivly done with this library in F#.

## Request authentication code

At first you need to request a notification code. For that you need some kind of embedded browser and point the browser to <https://login.gog.com/auth?client_id=46899977096215655&layout=client2&redirect_uri=https%3A%2F%2Fembed.gog.com%2Fon_login_success%3Forigin%3Dclient&response_type=code>.

You can find the documentation for that endpoint here:
<https://gogapidocs.readthedocs.io/en/latest/auth.html#methods>

The user has to login there and is redirected to an url like this:

    https://embed.gog.com/on_login_success?origin=client&code=iUZDhNPZNpQkQEPS9OqEUfyx-iNMpJT-W_AY9Miw8lpYnmkPOzE908Xp33t4cNXRUCyULAXB3koWEDFhQUzfboRrlZiKxTddDBiw-fnutQcAxmicbjdWdPnPuyZTnMC1tEsg2jZMhoeTvDt66Q5SP8idHGvqq8Z04AwOU09vkUw

The important part here is the `code=..` parameter. In this example this is

    iUZDhNPZNpQkQEPS9OqEUfyx-iNMpJT-W_AY9Miw8lpYnmkPOzE908Xp33t4cNXRUCyULAXB3koWEDFhQUzfboRrlZiKxTddDBiw-fnutQcAxmicbjdWdPnPuyZTnMC1tEsg2jZMhoeTvDt66Q5SP8idHGvqq8Z04AwOU09vkUw

This is the authentication code you need for the next step.

## Request authentication token

For this you need the authentication code from the previous step. This is used to request an authentication token, which is used to authenticate your API calls.

```fsharp
open GogApi.DotNet.FSharp

// let authCode = <AuthenticationCode>

// This returns an Async<Authentication>
let getNewToken = Authentication.getNewToken authCode


```
