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
    |> List.mapi (fun i x -> if i = indices.[idx] then newVal else x)


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

let moveUp (state: int list * int): int list * int = state
let moveDown (state: int list * int): int list * int = state
let moveLeft (state: int list * int): int list * int = state
let moveRight (state: int list * int): int list * int = state

let gameOver (board, score): bool =
    let (newBoard, _) =
        (moveUp (board, 0)
         |> moveDown
         |> moveLeft
         |> moveRight)
    // Board is full and no moves affect state
    (emptyTileIndices board).Length = 0
    && newBoard = board

let printRow (row: int list): unit =
    row
    |> List.iter (fun x -> if x = 0 then printf "|   " else printf "| %d " x)
    printfn "|\n+---+---+---+---+"

let printState (board: int list, score: int): unit =
    Console.Clear()
    printfn "+---+---+---+---+"
    for row in 0 .. 3 do
        board.[row * 4..row * 4 + 3] |> printRow
    printfn "Score: %d" score

let rec getMove () =
    let key = Console.ReadKey().Key
    match key with
    | ConsoleKey.UpArrow -> moveUp
    | ConsoleKey.DownArrow -> moveDown
    | ConsoleKey.LeftArrow -> moveLeft
    | ConsoleKey.RightArrow -> moveRight
    | _ -> getMove ()

let doMove (board: int list, score: int) (move: int list * int -> int list * int): int list * int =
    let mutable (newBoard, addedScore) = move (board, score)
    let newScore = score + addedScore
    newBoard <- addNewTile board
    printState (newBoard, newScore)
    (newBoard, newScore)

let go2048 () =
    printfn "Let's play 2048!\n"
    let mutable state = (createBoard (), 0)
    printState state
    while not (gameOver state) do
        state <- (getMove () |> doMove state)

    printfn "Game Over!"
