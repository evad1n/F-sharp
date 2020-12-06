open System

let emptyTileIndices (board: int list) =
    [ 0 .. board.Length - 1 ]
    |> List.filter (fun i -> board.[i] = 0)

let addNewTile (board: int list) =
    let indices = emptyTileIndices board
    let r = Random()
    let idx = (r.Next indices.Length)
    // Decide on 2 or 4
    let newVal = if (r.Next 10) < 8 then 2 else 4

    board
    |> List.mapi (fun i x -> if i = idx then newVal else x)


let createBoard (): int list =
    let mutable board =
        [ 0
          0
          0
          0
          0
          0
          0
          0
          0
          0
          0
          0
          0
          0
          0
          0 ]

    board <- (addNewTile board)
    board



let printRow (row: int list): unit =
    row |> List.iter (printf "| %d ")
    printfn "|\n+---+---+---+---+"

let printBoard (board: int list): unit =
    printfn "+---+---+---+---+"
    for row in 0 .. 3 do
        board.[row * 4..row * 4 + 3] |> printRow

let go2048 () =
    printfn "Let's play 2048!\n"
    let mutable board = createBoard ()
    while true do
        board <- addNewTile board
        printBoard board
        let key = Console.ReadKey()
        printfn "Key Char is : %A" key
