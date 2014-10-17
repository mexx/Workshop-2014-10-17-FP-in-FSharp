// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.
let assert_ x = System.Diagnostics.Debug.Assert(x)

let split (x:string) = x.Split ([| "," |], System.StringSplitOptions.RemoveEmptyEntries)
let stringToInt = System.Int32.Parse

let sc =
    split
    >> Seq.map stringToInt
    >> Seq.sum

let sc2 =
    Seq.sum << Seq.map stringToInt << split

[<EntryPoint>]
let main argv = 
    sc "" = 0 |> assert_
    sc "1" = 1 |> assert_
    sc "1,2" = 3 |> assert_
    assert_ <| (sc2"1,2" = 3)

    printfn "%A" argv
    0 // return an integer exit code
