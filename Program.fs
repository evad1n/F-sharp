// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System

open Types
open Stuff
open Games

let studentDemo () = 
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
    // studentDemo
    // Recursion.sieve [ 2 .. 100 ]
    // |> printfn "%A"
    // [0 .. 20]
    // |> List.iter Patterns.fizzbuzz
    // Ammon.source()
    Games.go2048()

    // let b = Games.createBoard()
    // Games.printState (b, 0)
    // Games.getCols b
    // |> printfn "%A"
    // Games.getCols b
    // |> List.map Games.combine
    // |> printfn "%A"

    // [2; 0; 2; 2]
    // |> Games.combine
    // |> printfn "%A"

    0 // return an integer exit code
