module GogApi.DotNet.FSharp.GamesMovies

type OwnedGamesResponse = {
    owned: int list;
}

let askForOwnedGameIds authentication =
    makeRequest<OwnedGamesResponse> authentication [] "https://embed.gog.com/user/data/games"

type GameDetailsResponse = {
    title: string;
    downloads: obj list list;
}

let askForGameInfo id authentication =
    sprintf "https://embed.gog.com/account/gameDetails/%i.json" id
    |> makeRequest<GameDetailsResponse> authentication [ ]
