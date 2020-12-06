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
        [ for i in 0 .. 15 do
            yield 0 ]

    board <- (addNewTile board)
    board

let mapCombine (row: int list, i) =
    if i = row.Length - 1 then (i + 1, row.[i], 0)
    else if row.[i + 1] = row.[i] then (i + 2, row.[i] + row.[i + 1], row.[i] + row.[i + 1])
    else (i + 1, row.[i], 0)



// Combines a row and produces the new row and the added score
// Combines a row left to right
let combine (row: int list): int list * int =
    // Can't skip iters in F# so this is ugly
    let mutable i = 0
    let mutable newRow = []
    let mutable score = 0

    while i < row.Length do
        let (newidx, addedRow, addedScore) = mapCombine (row, i)
        i <- newidx
        score <- score + addedScore
        newRow <- addedRow :: newRow
    // Add zeros at the end to pad
    while newRow.Length < 4 do
        newRow <- 0 :: newRow
    (newRow, score)

let getCols (board: int list): int list list =
    let mutable cols = []
    for i in 0 .. 3 do
        let newCol =
            [ for j in 0 .. 3 do
                yield board.[i + j * 4] ]

        cols <- cols @ [ newCol ]
    cols

let toCols (rows: int list list) (idx: int) =
    [ 0 .. rows.Length - 1 ]
    |> List.fold (fun acc i -> acc @ [ rows.[i].[idx] ]) []

let rowsToCols (up: bool) (rows: int list list) =
    let correctRows = if up then List.rev rows else rows

    [ 0 .. correctRows.[0].Length - 1 ]
    |> List.fold (fun acc i -> acc @ (toCols rows i)) []

let moveUp (board: int list, score: int): int list * int =
    let (combinedRows, addedScore) =
        (getCols board
         |> List.map combine
         |> List.fold (fun acc x ->
             (printfn "%A" x
              fst acc @ [ fst x ], snd acc + snd x)) ([], 0))

    printfn "%A" combinedRows

    let newBoard = (combinedRows |> rowsToCols true)

    (newBoard, score + addedScore)

let moveDown (board: int list, score: int): int list * int = (board, score)
let moveLeft (board: int list, score: int): int list * int = (board, score)
let moveRight (board: int list, score: int): int list * int = (board, score)

let gameOver (board, _): bool =
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
    let (newBoard, addedScore) = move (board, score)

    let newState =
        (addNewTile newBoard, score + addedScore)

    printState newState
    newState

let go2048 () =
    printfn "Let's play 2048!\n"
    let mutable state = (createBoard (), 0)
    printState state
    while not (gameOver state) do
        state <- (getMove () |> doMove state)

    printfn "Game Over!"
