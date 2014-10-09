// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

open Support

type Suit = | Hearts | Diamonds | Clubs | Spades
type Rank = | Ace | King | Queen | Jack | Ten | Nine | Eight | Seven | Six | Five | Four | Three | Two

type Card = Rank * Suit
let rank = fst
let suit = snd

let ranks = allCasesOf<Rank>
let suits = allCasesOf<Suit>

let deck =
    seq {
        for rank in ranks do
        for suit in suits do
            yield rank, suit
    }
    |> List.ofSeq

let deal n players (deck: Card seq) =
    use e = deck.GetEnumerator()
    let pickCard() =
        if e.MoveNext() |> not then failwith "no more cards in deck" else
        e.Current
    let rec deal n hands =
        if n = 0 then
            hands
        else
            hands
            |> Seq.map (fun (player, cards) -> player, pickCard() :: cards)
            |> deal (n - 1)
    players
    |> Seq.map (fun player -> player, [])
    |> deal n

type Hand =
| StraightFlush of Card
| FourOfAKind of Rank * kicker: Rank
| FullHouse of Rank * over: Rank
| Flush of ranks: (Rank * Rank * Rank * Rank * Rank) * Suit
| Straight of Rank
| ThreeOfAKind of Rank * kickers: (Rank * Rank)
| TwoPair of Rank * over: Rank * kicker: Rank
| OnePair of Rank * kickers: (Rank * Rank * Rank)
| High of Rank * kickers: (Rank * Rank * Rank * Rank)

let (|Sorted|) = Seq.sort >> List.ofSeq

let (|RankGroups|) hand =
    hand |> (Seq.groupBy id >> Seq.map (snd >> List.ofSeq) >> Seq.sortBy (Seq.length >> (~-)) >> List.ofSeq)

let forall p list =
    list
    |> Seq.pairwise
    |> Seq.forall p
    |> Option.ofBool
    |> Option.map (fun () -> list |> Seq.head)

let (|StraightInHand|_|) =
    forall (fun (Tag a, Tag b) -> b - a = 1) >> Option.map Seq.head

let (|OfSameSuit|_|) =
    forall (fun (a, b) -> a = b)

let (|NumberOfAKind|_|) n group =
    if group |> Seq.length = n then
        group |> Seq.head |> Some
    else None

let (|FiveKickers|_|) groups =
    match groups with
    | [[kicker1]; [kicker2]; [kicker3]; [kicker4]; [kicker5]] -> (kicker1, kicker2, kicker3, kicker4, kicker5) |> Some
    | _ -> None

let (|FourKickers|_|) groups =
    match groups with
    | [[kicker1]; [kicker2]; [kicker3]; [kicker4]] -> (kicker1, kicker2, kicker3, kicker4) |> Some
    | _ -> None

let (|ThreeKickers|_|) groups =
    match groups with
    | [[kicker1]; [kicker2]; [kicker3]] -> (kicker1, kicker2, kicker3) |> Some
    | _ -> None

let (|TwoKickers|_|) groups =
    match groups with
    | [[kicker1]; [kicker2]] -> (kicker1, kicker2) |> Some
    | _ -> None

let (|OneKicker|_|) groups =
    match groups with
    | [[kicker1]] -> kicker1 |> Some
    | _ -> None

let detect (Sorted hand) =
    let RankGroups rankGroups, suits = hand |> List.unzip
    match rankGroups, suits with
    | StraightInHand rank                                               , OfSameSuit suit   -> StraightFlush (rank, suit)
    | NumberOfAKind 4 rank :: OneKicker kicker                          , _                 -> FourOfAKind (rank, kicker)
    | NumberOfAKind 3 rank :: NumberOfAKind 2 over :: []                , _                 -> FullHouse (rank, over)
    | FiveKickers kickers                                               , OfSameSuit suit   -> Flush (kickers, suit)
    | StraightInHand rank                                               , _                 -> Straight rank
    | NumberOfAKind 3 rank :: TwoKickers kickers                        , _                 -> ThreeOfAKind (rank, kickers)
    | NumberOfAKind 2 rank :: NumberOfAKind 2 over :: OneKicker kicker  , _                 -> TwoPair (rank, over, kicker)
    | NumberOfAKind 2 rank :: ThreeKickers kickers                      , _                 -> OnePair (rank, kickers)
    | NumberOfAKind 1 rank :: FourKickers kickers                       , _                 -> High (rank, kickers)
    | _                                                                 , _                 -> failwithf "unknown hand %A" hand



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
