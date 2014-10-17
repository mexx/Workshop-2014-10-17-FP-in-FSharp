// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

let (|Safe|) o = if o = null then None else Some o

let f (Safe r) =
    r |> Option.map (fun _ -> "Juhu wir haben einen Wert!")


[<EntryPoint>]
let main argv = 
    printfn "%A" argv
    0 // return an integer exit code
