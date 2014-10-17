#I @"packages/FAKE/tools"
#r @"FakeLib.dll"
#I @"packages/FSharp.Compiler.Service/lib/net40"
#r "FSharp.Compiler.Service.dll"
#I @"packages/FSharp.Formatting/lib/net40"
#r "FSharp.Literate.dll"
#I @"packages/FsReveal/lib/net40"
#r @"FsReveal.dll"

open Fake
open FsReveal

let codeDirectory = __SOURCE_DIRECTORY__ @@ "Code"
let presentationOutputDirectory = __SOURCE_DIRECTORY__ @@ "Presentation"

Target "Clean" (fun _ ->
    [presentationOutputDirectory]
    |> CleanDirs
)

Target "Build" (fun _ ->
    { BaseDirectory = codeDirectory
      Includes = ["Poker.sln"]
      Excludes = [] }
    |> MSBuildRelease "" "Rebuild"
    |> ignore
)

Target "BuildPresentation" (fun _ ->
    FsRevealHelper.Folder <- __SOURCE_DIRECTORY__ @@ "packages" @@ "FsReveal" @@ "fsreveal"

    let gen filePath =
        let htmlFileName = changeExt ".html" filePath
        FsReveal.GenerateOutputFromMarkdownFile presentationOutputDirectory htmlFileName filePath

    ["readme.md"]
    |> Seq.iter gen

    ["paket-files"; "content"]
    |> Seq.map ((@@) __SOURCE_DIRECTORY__)
    |> Seq.iter (fun d -> CopyDir (presentationOutputDirectory @@ "assets") d (fun _ -> true))
)

Target "Code" DoNothing
Target "Presentation" DoNothing
Target "All" DoNothing

"Build" ==> "Code"
"Clean" ==> "BuildPresentation" ==> "Presentation"

"Code" ==> "All"
"Presentation" ==> "All"

RunTargetOrDefault "Code"
