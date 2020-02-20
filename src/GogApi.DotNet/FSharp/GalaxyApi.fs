module GogApi.DotNet.FSharp.GalaxyApi

open HttpFs.Client

type ProductInfoRequest = {
    id: int;
}

type InstallerFileInfo = {
    id: string;
    size: int64;
    downlink: string;
}

type InstallerInfo = {
    id: string;
    os: string;
    version: string option;
    files: InstallerFileInfo list;
}

type DownloadsInfo = {
    installers: InstallerInfo list;
}

type ProductsResponse = {
    id: int;
    title: string;
    downloads: DownloadsInfo;
}

let askForProductInfo (request :ProductInfoRequest) authentication =
    let queries = [
        createQuery "expand" "downloads"
    ]
    sprintf "https://api.gog.com/products/%i" request.id
    |> makeRequest<ProductsResponse> authentication queries

type SecureUrlRequest = {
    downlink: string;
}

type SecureUrlResponse = {
    downlink: string;
    checksum: string;
}

let askForSecureDownlink (request :SecureUrlRequest) authentication =
    makeRequest<SecureUrlResponse> authentication [] request.downlink
