[<AutoOpen>]
module Support

open Microsoft.FSharp.Reflection

let allCasesOf<'t> =
    let createCase c =
        FSharpValue.MakeUnion(c, [||]) :?> 't

    FSharpType.GetUnionCases(typeof<'t>)
    |> Array.map createCase

let tagOfCase (a: 't) =
    FSharpValue.PreComputeUnionTagReader(typeof<'t>) (box a)    

let (|Tag|) x = tagOfCase x

let shuffleArray seed a =
    let length = Array.length a
    let random = new System.Random(seed)
    let newIndex min = random.Next(min, length)

    let swap (a: _[]) x y =
        let v  = a.[x]
        a.[x] <- a.[y]
        a.[y] <- v

    Array.iteri (fun i _ -> swap a i (newIndex i)) a
    a

let shuffle seed = Array.ofList >> shuffleArray seed >> List.ofArray

module Option =
    let ofBool value =
        if value then
            Some()
        else None