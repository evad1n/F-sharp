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

let moveUp (board: int list): int list = board
let moveDown (board: int list): int list = board
let moveLeft (board: int list): int list = board
let moveRight (board: int list): int list = board

let gameOver board: bool = false

let printRow (row: int list): unit =
    row
    |> List.iter (fun x -> if x = 0 then printf "|   " else printf "| %d " x)
    printfn "|\n+---+---+---+---+"

let printBoard (board: int list): unit =
    printfn "+---+---+---+---+"
    for row in 0 .. 3 do
        board.[row * 4..row * 4 + 3] |> printRow

let rec getMove () =
    let key = Console.ReadKey().Key
    match key with
    | ConsoleKey.Enter -> printfn "Enter"
    | ConsoleKey.UpArrow -> moveUp
    | ConsoleKey.DownArrow -> moveDown
    | ConsoleKey.LeftArrow -> moveLeft
    | ConsoleKey.RightArrow -> moveRight
    | _ -> printfn "%A" key

let doMove (board: int list) (score: int) (move: int list -> int list * int): int list * int =
    let mutable (newBoard, addedScore) = move board
    let newScore = score + addedScore
    newBoard <- addNewTile board
    printBoard newBoard
    printfn "Score: %d" newScore
    (newBoard, newScore)

let go2048 () =
    printfn "Let's play 2048!\n"
    let mutable board = createBoard ()
    let mutable score = 0
    while not (gameOver board) do
        board <- addNewTile board
        printBoard board
        let key = Console.ReadKey().Key
        match key with
        | ConsoleKey.Enter -> printfn "Enter"
        | ConsoleKey.UpArrow -> board <- moveUp board
        | ConsoleKey.DownArrow -> board <- moveDown board
        | ConsoleKey.LeftArrow -> board <- moveLeft board
        | ConsoleKey.RightArrow -> board <- moveRight board
        | _ -> printfn "%A" key
        printfn "Score: %d" score
    printfn "Game Over!"
