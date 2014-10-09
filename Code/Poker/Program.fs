open Game

[<EntryPoint>]
let main argv =
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

    printfn "%A" argv
    System.Console.ReadLine() |> ignore
    0 // return an integer exit code
