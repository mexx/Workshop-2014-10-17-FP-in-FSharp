### function definition
Funktion ist die Mutter von Allem - EVA

`Eingabe => Verarbeitung => Ausgabe`

Eine Funktion, aka Abbildung, in der Mathematik ist genau dadurch definiert: eine Definitionsmenge, eine Zielmenge und eine Abbildungsvorschrift.

Eine Funktion ist eine Relation zwischen der Definitions- und Zielmenge, die linkstotal und rechtseindeutig ist. *(What the hack?)*

### functional purity
#### pure - no side effects

**Linkstotal**: Jeder Wert der Definitionsmenge kann als Eingabe verwendet werden. Die Funktion ist für jeden Wert definiert.

**Rechtseindeutig**: Jedem Wert der Definitionsmenge ist nur genau ein Wert der Zielmenge zugeordnet.

Beispiel:
```
Nachfolger einer natürlichen Zahl
f(x) = x + 1
```

Eigenschaften, welche eine Funktion erfüllen muss:
* bei gleicher Eingabe, liefert diese gleiche Ausgabe
    = **determiniert** (weniger als deterministisch)
* keine Seiteneffekte (besser als Nebeneffekte genannt, und noch besser als Wirkung)
    = **wirkungsfrei**, ändert keinen Programmzustand (IO, Zeit o.ä.)

#### expressions - immutable values
Werte werden an Namen gebunden, nicht zugewiesen!
```
let f x = x + 1
// let f x = x * 2, Fehler, da f nicht geändert werden darf.
```
Es gibt keine Variablen!
##### mutable keyword
Man kann in F# auch imperativ programmieren, also Variablen haben, jedoch gilt es diese zu vermeiden.
```
let mutable x = 1
x <- x + 1
```
##### reference cells
Manchmal ist `mutable` nicht genug und man brauch eine Referenzzelle :)
```
let x = ref 1
x := !x + 1
```

Der Compiler weist darauf hin, z.B. in einer Closure (seq-Expression).

### first class functions
#### higher order functions
Wenn man Funktionen als Funktionseingabe bzw. -ausgabe verwenden kann.
Beispiel als Eingabe:
```
let evalWith2AndAdd5 f = f 2 + 5
val evalWith2AndAdd5 : f:(int -> int) -> int

let succ x = x + 1
evalWith2AndAdd5 succ
```
Beispiel als Ausgabe:
```
let add x y = x + y
val add : x:int -> y:int -> int
```
#### lambdas
Anonyme Funktionen
```
let add x y = x + y
let add x = fun y -> x + y
let add = fun x -> (fun y -> x + y)
```
#### partial application
Binden einer Funktionseingabe an einen speziellen Wert.
```
let succ x = add 1 x
let succ x = 1 + x
```
##### Tacit programming, i.e. *point-free style*
Also eta-reduction
```
let succ x = add 1 x
let succ = add 1
let succ x = 1 + x
let succ x = (+) 1 x
let succ = (+) 1
```
#### composition
Komposition bzw. Verkettung von Funktionen.

`f: a -> b, g: b -> c` => `(>>) f g: a -> c`

`(>>) f g x = g(f(x))`
```
let succ x = 1 + x
let square x = x * x
let squareOfSucc = succ >> square
let succOfSquare = square >> succ

squareOfSucc 2
succOfSquare 2
```
##### pipelines [*F#*]
Werte an Funktionen übergeben.

`v: a, f: a -> b` => `(|>) v f: a -> (a -> b) -> b`

`(|>) v f = f v`
```
let succ = (+) 1
let square x = x * x
2 |> succ
2 |> square
```
```
2 |> succ |> square // = squareOfSucc 2
2 |> square |> succ // = succOfSquare 2
```
#### recursion
Abbruchbedingung und Rekursionsvorschrift!
Explizite Kennzeichnung rekursiven Funktionen.
```
let rec factorial (n) =
    if n = 1 then 1
    else n * factorial (n - 1)
```
### type system
#### type inference
Typeninferenz ist eine schöne Sache, damit brauchen wir fast nie den Datentypen angeben. Bisher haben wir bei der Definitionen der Funktionen nie die Datentypen angegeben.
#### different types
Ist sehr reichhaltig und das sollte ausgenutzt werden.
Es gibt die allbekannten primitiven Typen wie `int`, `float`, `bool` und `string`.
Dann haben wir auch schon Funktionstypen kennen gelernt, wie `int -> int`.
Natürlich gibt es das bekannte Array und die *unbekannte* Liste.
Zusätzlich haben wir dann noch

* Tuple,
mit diesem können Daten unterschiedlichen Typs zusammengefasst werden

```
type Person = string * int
let trainer = ("Max", 33)

let name = fst trainer
let age = snd trainer
let name, age = trainer
```
* Record,
auch mit diesem können Daten unterschiedlichen Typs zusammengefasst werden, bietet einwenig mehr als Tuple

```
type Person = { Name: string; Age: int }
let trainer = { Name = "Max"; Age = 33 }

let trainerAYearAgo = { trainer with Age = 32 }

let name = trainer.Name
let age = trainer.Age
let { Name = name; Age = age } = trainer
```
* Discriminated union,
mit diesem können unterschiedliche Ausprägungen zu einem zusammengefasst werden

```
type Person =
| Trainer
| Student

type Point = {X: float; Y: float}
type Shape =
| Circle of center: Point * radius: float
| Rect of corner: Point * width: float * height: float
```

Tuple und Record sind die sogenannten Produkttypen, DU ist ein Summentyp.
##### design for correctness / make illegal states unrepresentable
Mit vorgestellten Möglichkeiten ist es ein Leichtes ein korrektes Abbild der Domäne zu schaffen, damit nur die Zustände darstellbar werden, welche auch tatsächlich erlaubt bzw. möglich sind.
#### unit [*F#*]
Manchmal, wenn man Funktionen mit Wirkung haben muss, z.B. IO, Zeit oder Random, dann gibt es entweder keine Eingabe oder keine Ausgabe. In anderen Sprachen wird dafür entweder ein Schlüsselwort oder gar Konstrukt verwendet. In F# gibt es dafür einen speziellen Datentypen `unit` mit einzigem Wert `()`.
#### object expressions [*F#*]
```
let mock = { new IInterface with member __.Method() = "3" }
```
#### type extensions [*F#*]
```
type System.String with
    member x.M() = x.Substring(0, 1)
```

### currying = first class functions + tuple
```
let add(x, y) = x + y
val add : (int, int) -> int
let addCurried x y = add (x, y)
val addCurried : int -> int -> int
```

### pattern matching
Switch statements on steroids.
#### active patterns [*F#*]
Erweiterung des Matching mit eigenen Fällen.

### workflows
#### computational expressions [*F#*]
