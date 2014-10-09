open Owin
open Microsoft.Owin.Hosting

open EkonBenefits.FSharp.Dynamic
open Microsoft.AspNet.SignalR

open Game

let consoleTest() =
    while true do
        let game =
            deck
            |> shuffle (int System.DateTime.UtcNow.Ticks)
            |> deal 5 [1..10]
            |> Seq.map (fun (player, cards) -> player, (detect cards, cards))
            |> Seq.sortBy (snd >> fst)
            |> Array.ofSeq
        
        printfn "%A" game
        if game.[0] |> snd |> fst < Straight Two then
            System.Console.ReadLine() |> ignore
            System.Console.Clear()
        else
            System.Console.WriteLine()
            System.Console.WriteLine("Next game: ")

type GameHub =
    inherit Hub
    member __.Send =
        base.Clients.All?addMessage ()

let startup (app: IAppBuilder) =
    app.MapSignalR() |> ignore
    ()

let webserverTest() =
    let url = "http://localhost:8085"

    use app = WebApp.Start(url, startup)
    System.Console.WriteLine("Server running on {0}", url)
    System.Console.ReadLine() |> ignore

[<EntryPoint>]
let main argv =
    //webserverTest() |> ignore
    consoleTest() |> ignore

    //printfn "%A" argv
    //System.Console.ReadLine() |> ignore
    0 // return an integer exit code
