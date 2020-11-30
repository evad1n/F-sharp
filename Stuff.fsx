module StringFunctions =
    let mapText (text: string) f: string =
        (text.Split ' ')
        |> Array.toList
        |> List.map f
        |> String.concat " "

module PigLatin =
    let toPigLatin (word: string): string =
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

        if isVowel word.[0] then word + "yay" else word.[1..] + string (word.[0]) + "ay"


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
