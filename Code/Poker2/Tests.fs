module Tests

open Xunit
open Xunit.Extensions
open FsUnit.Xunit

[<Fact>]
let ``Bewerte hand kommt high card``() =
    bewerte ((Zwei, Pik), (Drei, Pik), (Vier, Pik), (Sechs, Karo), (As, Herz))
    |> should equal (HighCard (As, Sechs, Vier, Drei, Zwei))

[<Fact>]
let ``Bewerte hand kommt high card 2``() =
    bewerte ((Zwei, Pik), (Drei, Pik), (König, Pik), (Sechs, Karo), (As, Herz))
    |> should equal (HighCard (As, König, Sechs, Drei, Zwei))

[<Fact>]
let ``Bewerte hand kommt ein Paar``() =
    bewerte ((Drei, Pik), (Drei, Karo), (König, Pik), (Sechs, Karo), (As, Herz))
    |> should equal (Pair (Drei, Drei, As, König, Sechs))

[<Fact>]
let ``Bewerte hand kommt zwei Paare``() =
    bewerte ((Drei, Pik), (Drei, Karo), (König, Pik), (König, Karo), (As, Herz))
    |> should equal (TwoPair (König, König, Drei, Drei, As))
