// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open Types
open Stuff

let studentDemo = 
    printfn "All students"
    Records.students
    |> Records.printStudents
    
    printfn "Good students"
    Records.students
    |> List.filter Records.goodStudents
    |> Records.printStudents

[<EntryPoint>]
let main argv =
    // PigLatin.toPigLatinFile "input1.txt" |> ignore
    // Recursion.factorial 20L
    // |> printfn "%d"
    studentDemo
    0 // return an integer exit code