// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open Types
open Stuff

// Define a function to construct a message to print
let from whom =
    sprintf "from %s" whom

[<EntryPoint>]
let main argv =
    // greet ["Ammon"; "Mark"]
    // PigLatin.toPigLatinFile "input1.txt" |> ignore
    // Recursion.factorial 20L
    // |> printfn "%d"
    Records.printPeople
    0 // return an integer exit code