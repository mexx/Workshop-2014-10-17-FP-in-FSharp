[<AutoOpen>]
module Code

type Quintuple<'t> = 't * 't * 't * 't * 't

module Seq =
    let toQuintuple s =
        match s with
        | [a; b; c; d; e] -> (a, b, c, d, e)
        | _ -> failwith "Ungültige Hand"

    let fromQuintuple (a, b, c, d, e) = [a; b; c; d; e]

type Farbe = Kreuz | Karo | Herz | Pik
type Wert =
    | Zwei | Drei | Vier | Fünf | Sechs | Sieben
    | Acht | Neun | Zehn | Bube | Dame | König | As
type Karte = Wert * Farbe
type Hand = Quintuple<Karte>

let wert = fst

type Handwert =
| HighCard of Wert Quintuple
| Pair of Wert Quintuple
| TwoPair of Wert Quintuple

let bewerte (hand:Hand) =
    let sorted =
        hand
        |> Seq.fromQuintuple
        |> List.map wert
        |> List.sort
        |> List.rev

    let groups =
        let anzahl = snd
        let negate = (~-)

        sorted
        |> Seq.groupBy id
        |> Seq.map (fun (wert, cards) -> wert, Seq.length cards)
        |> Seq.sortBy (anzahl >> negate)
        |> Seq.toList

    let nOfKind n (wert, anzahl) = if (anzahl = n) then Some wert else None
    let (|PairOf|_|) = nOfKind 2
    let (|SingleOf|_|) = nOfKind 1

    match groups with
    | [PairOf wert; PairOf wert2; SingleOf a] -> TwoPair (wert, wert, wert2, wert2, a)
    | [PairOf wert; SingleOf a; SingleOf b; SingleOf c] -> Pair (wert, wert, a, b, c)
    | _ -> HighCard (sorted |> Seq.toQuintuple)
