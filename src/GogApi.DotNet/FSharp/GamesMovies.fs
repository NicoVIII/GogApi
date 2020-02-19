module GogApi.DotNet.FSharp.GamesMovies

type OwnedGamesResponse = {
    owned: int list;
}

let askForOwnedGameIds auth =
    makeRequest<OwnedGamesResponse> auth [] "https://embed.gog.com/user/data/games"

type GameDetailsResponse = {
    title: string;
    downloads: obj list list;
}

let askForGameInfo id auth =
    sprintf "https://embed.gog.com/account/gameDetails/%i.json" id
    |> makeRequest<GameDetailsResponse> auth [ ]
