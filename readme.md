- title : Funktionale Programmierung mit F#
- description : Funktionale Programmierung mit F#
- author : Max Malook
- theme : Night
- transition : default

***
<img src="assets/fsharp/fsfoundation/img/logo/fsharp128.png" alt="F# Logo" style="background: rgba(255,255,255,0); border: 0px">
## Funktionale Programmierung mit F#
Es geht um die Kernkonzepte der funktionalen Programmierung. Dazu gehört zum Beispiel *Immutability*, *Data Types*, *Partial Application* und *Pattern Matching* die in praktischen Übungen angewendet werden. Dabei werden auch F#-spezifische Sprachfeatures wie *Active Patterns* und *Object Expressions* behandelt.

### Voraussetzungen
* Spass und ein eigenes Notebook
* F# Entwicklungsumgebung, Auswahl und Installationsanteilung für [Windows](http://fsharp.org/use/windows/), [Mac](http://fsharp.org/use/mac/) oder [Linux](http://fsharp.org/use/linux/)

***
### Themen
* function definition
* functional purity
    * pure - no side effects
    * expressions - immutable values
        * mutable keyword [*F#*]
        * reference cells [*F#*]
* first class functions
    * higher order functions
    * lambdas
    * partial application
        * Tacit programming, i.e. *point-free style*
    * composition
        * pipelines [*F#*]
    * recursion

---
### Themen
* type system
    * type inference
    * different types
        * design for correctness
        * make illegal states unrepresentable
    * unit [*F#*]
    * object expressions [*F#*]
    * type extensions [*F#*]
* currying = first class functions + tuple
* pattern matching
    * active patterns [*F#*]

***
### function definition
* Funktion ist die Mutter von Allem
* EVA = Eingabe => Verarbeitung => Ausgabe
* Eine Funktion, aka Abbildung, in der Mathematik ist genau dadurch definiert: eine Definitionsmenge, eine Zielmenge und eine Abbildungsvorschrift.
* Eine Funktion ist eine Relation zwischen der Definitions- und Zielmenge, die **linkstotal** und **rechtseindeutig** ist. *(What the hack?)*

---
### function definition
* **Linkstotal**: Jeder Wert der Definitionsmenge kann als Eingabe verwendet werden. Die Funktion ist für jeden Wert definiert.
* **Rechtseindeutig**: Jedem Wert der Definitionsmenge ist nur genau ein Wert der Zielmenge zugeordnet.

<br/><br/>
Beispiel:

    [lang=haskell]
    // Nachfolger einer natuerlichen Zahl
    f(x) = x + 1

***
### functional purity
#### pure - no side effects
Eigenschaften, welche eine Funktion erfüllen muss:
<br/><br/>

* bei gleicher Eingabe, liefert diese gleiche Ausgabe<br/>
    = **determiniert** (weniger als deterministisch)
* keine Seiteneffekte<br/>
    (besser als Nebeneffekte genannt, und noch besser als Wirkung)<br/>
    = **wirkungsfrei**, ändert keinen Programmzustand (IO, Zeit o.ä.)

---
### functional purity
#### expressions - immutable values

* Werte werden an Namen gebunden, nicht zugewiesen!
* Es gibt keine Variablen!

<br/><br/>
Beispiel:

    [module=Expressions]
    let f x = x + 1
    let f x = x * 2 // Fehler, da f nicht geändert werden darf.

---
### functional purity
#### expressions - immutable values
##### mutable keyword [*F#*]

* Man kann in F# auch imperativ programmieren.
* Also Variablen haben, jedoch gilt es diese zu vermeiden.

<br/><br/>
Beispiel:

    [module=Mutables]
    let mutable x = 1
    x <- x + 1

---
### functional purity
#### expressions - immutable values
##### reference cells [*F#*]

* Manchmal ist `mutable` nicht genug und man brauch eine Referenzzelle.
* Der Compiler weist darauf hin, z.B. in einer Closure (seq-Expression).

<br/><br/>
Beispiel:

    [module=RefCells]
    let x = ref 1
    x := !x + 1

***
### first class functions
#### higher order functions
Wenn man Funktionen als Funktionseingabe bzw. -ausgabe verwenden kann.

<br/>
Beispiel:

    [module=HighOrderFuncs]
    let evalWith2AndAdd5 f = f 2 + 5

    let succ x = x + 1
    evalWith2AndAdd5 succ

    let add x y = x + y

---
### first class functions
#### lambdas
Anonyme Funktionen

<br/>
Beispiel:

    [module=Lambdas]
    let add x y = x + y
    let add x = fun y -> x + y
    let add = fun x -> (fun y -> x + y)

---
### first class functions
#### partial application
Binden einer Funktionseingabe an einen speziellen Wert.

<br/>
Beispiel:

    [module=PartialApplication]
    let succ x = add 1 x
    let succ x = 1 + x

---
### first class functions
#### partial application
##### Tacit programming, i.e. *point-free style*
Also eta-reduction

<br/>
Beispiel:

    [module=Tacit]
    let succ x = add 1 x
    let succ = add 1
    let succ x = 1 + x
    let succ x = (+) 1 x
    let succ = (+) 1

---
### first class functions
#### composition
Komposition bzw. Verkettung von Funktionen.

    [lang=haskell]
    f: a -> b, g: b -> c => (>>) f g: a -> c

    (>>) f g x = g(f(x))

<br/>
Beispiel:

    [module=Composition]
    let succ x = 1 + x
    let square x = x * x
    let squareOfSucc = succ >> square
    let succOfSquare = square >> succ

    squareOfSucc 2
    succOfSquare 2

---
### first class functions
#### composition
##### pipelines [*F#*]
Werte an Funktionen übergeben.

    [lang=haskell]
    v: a, f: a -> b => (|>) v f: a -> (a -> b) -> b

    (|>) v f = f v

<br/>
Beispiel:

    [module=Pipelines]
    let succ = (+) 1
    let square x = x * x
    2 |> succ
    2 |> square

    2 |> succ |> square = squareOfSucc 2
    2 |> square |> succ = succOfSquare 2

---
### first class functions
#### recursion

* Abbruchbedingung und Rekursionsvorschrift!
* Explizite Kennzeichnung rekursiven Funktionen.

<br/><br/>
Beispiel:

    [module=Recursive]
    let rec factorial n =
        if n = 1 then 1
        else n * factorial (n - 1)

***
### type system
#### type inference
* Typeninferenz ist eine schöne Sache, damit brauchen wir fast nie den Datentypen angeben.
* Bisher haben wir bei der Definitionen der Funktionen nie die Datentypen angegeben.

---
### type system
#### different types
* Ist sehr reichhaltig und das sollte ausgenutzt werden.
* Es gibt die allbekannten primitiven Typen wie
    * int, float, bool und string.
* Dann haben wir auch schon Funktionstypen kennen gelernt, wie
    * int -> int.
* Natürlich gibt es das bekannte Array und die *unbekannte* Liste.

---
### type system
#### different types
* Tuple,
mit diesem können Daten unterschiedlichen Typs zusammengefasst werden.

<br/><br/>
Beispiel:

    [module=Tuples]
    type Person = string * int
    let trainer = ("Max", 33)

    let name = fst trainer
    let age = snd trainer
    let name, age = trainer

---
### type system
#### different types
* Record,
auch mit diesem können Daten unterschiedlichen Typs zusammengefasst werden, bietet einwenig mehr als Tuple.

<br/><br/>
Beispiel:

    [module=Records]
    type Person = { Name: string; Age: int }
    let trainer = { Name = "Max"; Age = 33 }

    let trainerAYearAgo = { trainer with Age = 32 }

    let name = trainer.Name
    let age = trainer.Age
    let { Name = name; Age = age } = trainer
    let { Name = name } = trainer
    let { Age = age } = trainer

---
### type system
#### different types
* Discriminated union,
mit diesem können unterschiedliche Ausprägungen zu einem zusammengefasst werden.

<br/><br/>
Beispiel:

    [module=DiscriminatedUnions]
    type Person =
    | Trainer
    | Student

    type Point = {X: float; Y: float}
    type Shape =
    | Circle of center: Point * radius: float
    | Rect of corner: Point * width: float * height: float
---
### type system
#### different types
##### Option
* Ist wie Nullable, geht aber auch mit Klassen.

> Just say NO to NullPointerExceptions [@jessitron](https://twitter.com/jessitron)

<br/>
Beispiel:

    [module=Options]
    type Option<'t> =
    | Some of 't
    | None

---
### type system
#### different types
* Tuple und Record sind die sogenannten Produkttypen.
* Discriminated union ist ein Summentyp.

---
### type system
#### different types
##### design for correctness / make illegal states unrepresentable
Mit vorgestellten Möglichkeiten ist es ein Leichtes ein korrektes Abbild der Domäne zu schaffen, damit nur die Zustände darstellbar werden, welche auch tatsächlich erlaubt bzw. möglich sind.

---
### type system
#### unit [*F#*]
Manchmal, wenn man Funktionen mit Wirkung haben muss,<br/>
z.B. IO, Zeit oder Random,<br/>
dann gibt es entweder keine Eingabe oder keine Ausgabe.<br/>
In anderen Sprachen wird dafür entweder ein Schlüsselwort oder gar Konstrukt verwendet.<br/>
In F# gibt es dafür einen speziellen Datentypen<br/>
`unit` mit einzigem Wert `()`.

---
### type system
#### object expressions [*F#*]
Anonyme Objekte

<br/>
Beispiel:

    [module=ObjectExpresions]
    let mock = { new IInterface with member this.Method() = "3" }

---
### type system
#### type extensions [*F#*]
Erweiterungsmethoden

<br/>
Beispiel:

    [module=TypeExtensions]
    type System.String with
        member x.FirstChar() = x.Substring(0, 1)

***
### currying
#### tuple vs. first class functions

<br/>
Beispiel:

    [module=Curring]
    let add(x, y) = x + y
    let addCurried x y = add (x, y)

***
### pattern matching
Switch statements on steroids.

<br/>
Beispiel:

    [module=Patterns]
    let maybe = Some "Name"
    match maybe with
    | Some value -> value
    | None       -> "Unbekannt"

---
### pattern matching
Switch statements on steroids.

<br/>
Beispiel:

    [module=Patterns]
    match System.Int32.TryParse("Max") with
    | true, value -> value
    | false, _    -> -1

---
### pattern matching
#### active patterns [*F#*]
Erweiterung des Matching mit eigenen Fällen.

<br/>
Beispiel:

    [module=ActivePatterns]
    let (|IsFizz|) n = n % 3 = 0
    let (|IsBuzz|) n = n % 5 = 0
    let number = 5
    match number with
    | IsFizz -> "Fizz"
    | IsBuzz -> "Buzz"
    | number -> string number

---
### pattern matching
#### active patterns [*F#*]
Erweiterung des Matching mit eigenen Fällen.

<br/>
Beispiel:

    [module=ActivePatterns]
    let (|Number|_|) (s:string) =
        match System.Int32.TryParse(s) with
        | true, value -> Some value
        | false, _    -> None
    let text = "5"
    match text with
    | Number x -> x + 1
    | _        -> -1

---
### pattern matching
#### active patterns [*F#*]
Erweiterung des Matching mit eigenen Fällen.

<br/>
Beispiel:

    [module=ActivePatterns]
    let (|Fizz|Buzz|FizzBuzz|None|) n =
        match n with
        | IsFizz & IsBuzz -> FizzBuzz
        | IsFizz          -> Fizz
        | IsBuzz          -> Buzz
        | _               -> None
    let number = 60
    match number with
    | FizzBuzz -> printfn "number is FizzBuzz"
    | Fizz     -> printfn "number is Fizz"
    | Buzz     -> printfn "number is Buzz"
    | None     -> printfn "number is None"

***
### Bonus stuff

---
### workflows
#### computational expressions [*F#*]

---
### agents
#### mailbox processor [*F#*]

***
### Links
* [The F# Software Foundation](http://fsharp.org/)
* [Try F#](http://www.tryfsharp.org/)
* [F# for fun and profit](http://fsharpforfunandprofit.com/)
* [F# Active Patterns](www.devjoy.com/series/active-patterns/)
* [F# for you](http://fsharp4u.com/)
* [F# cheat sheet](http://dungpa.github.io/fsharp-cheatsheet/)
* [F# workshop](http://www.fsharpworkshop.com/)

<br/><br/>

* [Functional Principles for OO Devs](http://www.youtube.com/watch?v=GpXsQ-NIKXY) by [Jessica Kerr](https://twitter.com/jessitron)
* [Active Patterns explained](http://www.devjoy.com/series/active-patterns/) by [Richard Dalton](https://twitter.com/richardadalton)
* [FP Europe 2014 Tour](http://bit.ly/fsTour2014) by [Mathias Brandewinder](https://twitter.com/brandewinder)

***
### Notes
* indentation is for scope
* modules are for code organization
* object oriented programming in F#
* unit of measure

***
### Build with
<div><a href="http://kimsk.github.io/FsReveal/"><img src="assets/kimsk/FsReveal/docs/files/img/logo-2.png" alt="FsReveal Logo" style="background: rgba(255,255,255,0); border: 0px; height: 2em;"><br/>FsReveal</a></div>
<div><a href="http://fsharp.github.io/FAKE/"><img src="assets/fsharp/FAKE/help/pics/logo.png" alt="FAKE Logo" style="background: rgba(255,255,255,0); border: 0px; height: 2em;"><br/>FAKE</a></div>
<div><a href="http://fsprojects.github.io/Paket/"><img src="assets/fsprojects/Paket/docs/files/img/logo.png" alt="Paket Logo" style="background: rgba(255,255,255,0); border: 0px; height: 2em;"><br/>Paket</a></div>
