﻿module SuaveMusicStore.App

open Suave
open Suave.Http
open Suave.Http.Successful
open Suave.Http.Applicatives
open Suave.Http.RequestErrors
open Suave.Types
open Suave.Web

let html container =
    OK (View.index container)

let browse =
    request (fun r ->
        match r.queryParam "genre" with
        | Choice1Of2 genre -> OK (sprintf "Genre: %s" genre)
        | Choice2Of2 msg -> BAD_REQUEST msg)

let webPart = 
    choose [
        path Path.home >>= html View.home
        path Path.Store.overview >>= (OK "Store")
        path Path.Store.browse >>= browse
        pathScan Path.Store.details (fun id -> OK (sprintf "Details %d" id))

        pathRegex "(.*)\.(css|png)" >>= Files.browseHome
    ]

startWebServer defaultConfig webPart