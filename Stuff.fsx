open System.IO
open System

module StringFunctions =
    let mapText (text: string) f: string =
        (text.Split ' ')
        |> Array.toList
        |> List.map f
        |> String.concat " "

    // Haha lol just call word.ToUpper() hahahhaha
    let stringToUpper (word: string): string =
        Seq.toList word
        |> List.map System.Char.ToUpper
        |> Array.ofList
        |> System.String.Concat

module PigLatin =
    let toPigLatinWord (word: string): string =
        let isVowel (c: char) =
            match c with
            | 'a'
            | 'e'
            | 'i'
            | 'o'
            | 'u'
            | 'A'
            | 'E'
            | 'I'
            | 'O'
            | 'U' -> true
            | _ -> false

        // Check for ending punctuation marks
        let ignore (c: char) =
            match c with
            | '!'
            | '?'
            | '''
            | '.'
            | '"'
            | '`' -> true
            | _ -> false

        // FIX: Don't move punctuation

        if isVowel word.[0] then word + "yay" else word.[1..] + string (word.[0]) + "ay"

    let toPigLatinText (text: string): string =
        StringFunctions.mapText text toPigLatinWord

    let toPigLatinFile (file: string) =
        let text = File.ReadAllText(file)
        printfn "%A" text
        let pigText = toPigLatinText text

        // Check if pig folder exists
        if not (Directory.Exists("pig"))
        then Directory.CreateDirectory("./pig") |> ignore

        File.WriteAllText("pig/pig-" + file, pigText)


module ListStuff =
    let square x = x * x

    let squares (n: int): int list =
        [ for i in 1 .. n do
            yield i * i ]

module PracticeTesting =

    // An automorphic number is one whose square ends in the same digits as itself
    let automorphic (n: int): string =
        let sq = string (n * n)
        if sq.[String.length sq - String.length (string n)..String.length sq] = string n
        then "Automorphic"
        else "Not!!"

// Prints a greeting for each name!
let greet (people: string list) =
    people
    |> List.map (fun person -> "Hello " + person + "!")
    |> List.iter (fun greeting -> printfn "%s" greeting)

module Recursion =
    let rec factorial (n: int64): int64 =
        match n with
        | 0L -> 1L
        | n -> n * factorial (n - 1L)

    let rec sieve =
        function
        | (p :: xs) ->
            p
            :: sieve [ for x in xs do
                           if x % p > 0 then yield x ]
        | [] -> []

    let rec fib (n: int): int =
        match n with
        | n when n <= 1 -> n
        | _ -> fib (n - 2) + fib (n - 1)

module Ammon =
    let source () =
        [| 32
           124
           62
           32
           40
           102
           117
           110
           32
           120
           45
           62
           32
           112
           114
           105
           110
           116
           102
           110
           32
           34
           37
           65
           37
           115
           34
           32
           120
           32
           60
           124
           32
           83
           121
           115
           116
           101
           109
           46
           83
           116
           114
           105
           110
           103
           46
           74
           111
           105
           110
           40
           34
           34
           44
           32
           65
           114
           114
           97
           121
           46
           109
           97
           112
           32
           99
           104
           114
           32
           120
           41
           41 |]
        |> (fun x ->
            printfn "%A%s" x
            <| System.String.Join("", Array.map char x))
