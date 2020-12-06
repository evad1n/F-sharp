open System

let go2048 () =
    printfn "Let's play 2048!\n"
    while true do
        let key = Console.ReadKey()
        printfn "Key Char is : %A" key
