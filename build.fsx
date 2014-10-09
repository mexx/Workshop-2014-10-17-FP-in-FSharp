#I @"packages/FAKE/tools"
#r @"FakeLib.dll"

open Fake

let codeDirectory = __SOURCE_DIRECTORY__ @@ "Code"

Target "Build" (fun _ ->
    { BaseDirectory = codeDirectory
      Includes = ["Poker.sln"]
      Excludes = [] }
    |> MSBuildRelease "" "Rebuild"
    |> ignore
)

Target "All" DoNothing

"Build" ==> "All"

RunTargetOrDefault "All"