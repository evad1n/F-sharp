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
    let crow = List.filter (fun x -> x <> 0) row

    // Can't skip iters in F# so this is ugly
    let mutable i = 0
    let mutable newRow = []
    let mutable score = 0

    while i < crow.Length do
        let (newidx, newVal, addedScore) = mapCombine (crow, i)
        i <- newidx
        score <- score + addedScore
        newRow <- newVal :: newRow
    // Add zeros at the end to pad
    while newRow.Length < 4 do
        newRow <- 0 :: newRow
    (newRow, score)

let getRows (board: int list): int list list =
    let mutable rows = []

    for i in 0 .. 3 do
        let newRow = board.[i * 4..(i * 4 + 3)]

        rows <- rows @ [ newRow ]
    rows

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
    |> List.fold (fun acc i -> (toCols rows i) @ acc) []



let moveUp (board: int list, score: int): int list * int =
    let (combinedRows, addedScore) =
        (getCols board
         |> List.map combine
         |> List.fold (fun acc x -> (fst acc @ [ fst x ], snd acc + snd x)) ([], 0))

    ((combinedRows |> rowsToCols true), addedScore)

let moveDown (board: int list, score: int): int list * int =
    let (combinedRows, addedScore) =
        ((List.map (List.rev >> combine) (getCols board))
         |> List.fold (fun acc x -> (fst acc @ [ List.rev (fst x) ], snd acc + snd x)) ([], 0))

    ((combinedRows |> rowsToCols false), addedScore)

let moveLeft (board: int list, score: int): int list * int =
    let (combinedRows, addedScore) =
        (getRows board
         |> List.map combine
         |> List.fold (fun acc x -> (fst acc @ [ List.rev (fst x) ], snd acc + snd x)) ([], 0))

    ((List.concat combinedRows), addedScore)

let moveRight (board: int list, score: int): int list * int =
    let (combinedRows, addedScore) =
        ((List.map (List.rev >> combine) (getRows board))
         |> List.fold (fun acc x -> (fst acc @ [ fst x ], snd acc + snd x)) ([], 0))

    ((List.concat combinedRows), addedScore)

let gameOver (board, _): bool =
    let (newBoard, _) =
        (moveUp (board, 0)
         |> moveDown
         |> moveLeft
         |> moveRight)
    // Board is full and no moves affect state
    (emptyTileIndices board).Length = 0
    && newBoard = board


let format (num: int) (len: int) =
    let numLen = String.length (sprintf "%i" num)
    let fHalf = (len - numLen) / 2
    let sHalf = (len - numLen) - fHalf

    let firstHalf =
        [ for i in 0 .. fHalf - 1 do
            yield " " ]
        |> String.concat ""

    let secondHalf =
        [ for i in 0 .. sHalf - 1 do
            yield " " ]
        |> String.concat ""

    printf "|%s%d%s" firstHalf num secondHalf

let printRow (row: int list): unit =
    printfn "|       |       |       |       |"
    row
    |> List.iter (fun x -> if x = 0 then printf "|       " else format x 7)
    printfn "|\n|       |       |       |       |"
    printfn "+-------+-------+-------+-------+"

let printState (board: int list, score: int): unit =
    Console.Clear()
    printfn "+-------+-------+-------+-------+"
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
    | _ -> getMove () // Ask for another move

let doMove (board: int list, score: int) (move: int list * int -> int list * int): int list * int =
    let (newBoard, addedScore) = move (board, score)

    let newState =
        if newBoard <> board
        then (addNewTile newBoard, score + addedScore)
        else (board, score)

    printState newState
    newState

let greeting () =
    for i in 5 .. -1 .. 1 do
        match i with
        | 5 -> Console.ForegroundColor <- ConsoleColor.Red
        | 4 -> Console.ForegroundColor <- ConsoleColor.Green
        | 3 -> Console.ForegroundColor <- ConsoleColor.Blue
        | 2 -> Console.ForegroundColor <- ConsoleColor.Yellow
        | 1 -> Console.ForegroundColor <- ConsoleColor.Magenta
        | _ -> ()
        printfn "
            ╦  ┌─┐┌┬┐┌─┐  ╔═╗┬  ┌─┐┬ ┬
            ║  ├┤  │ └─┐  ╠═╝│  ├─┤└┬┘
            ╩═╝└─┘ ┴ └─┘  ╩  ┴─┘┴ ┴ ┴

     222222222222222         000000000            444444444       888888888
    2:::::::::::::::22     00:::::::::00         4::::::::4     88:::::::::88
    2::::::222222:::::2  00:::::::::::::00      4:::::::::4   88:::::::::::::88
    2222222     2:::::2 0:::::::000:::::::0    4::::44::::4  8::::::88888::::::8
                2:::::2 0::::::0   0::::::0   4::::4 4::::4  8:::::8     8:::::8
                2:::::2 0:::::0     0:::::0  4::::4  4::::4  8:::::8     8:::::8
             2222::::2  0:::::0     0:::::0 4::::4   4::::4   8:::::88888:::::8
        22222::::::22   0:::::0 000 0:::::04::::444444::::444  8:::::::::::::8
      22::::::::222     0:::::0 000 0:::::04::::::::::::::::4 8:::::88888:::::8
     2:::::22222        0:::::0     0:::::04444444444:::::4448:::::8     8:::::8
    2:::::2             0:::::0     0:::::0          4::::4  8:::::8     8:::::8
    2:::::2             0::::::0   0::::::0          4::::4  8:::::8     8:::::8
    2:::::2       2222220:::::::000:::::::0          4::::4  8::::::88888::::::8
    2::::::2222222:::::2 00:::::::::::::00         44::::::44 88:::::::::::::88
    2::::::::::::::::::2   00:::::::::00           4::::::::4   88:::::::::88
    22222222222222222222     000000000             4444444444     888888888
                                                                               \n"
        printf "In %d..." i
        System.Threading.Thread.Sleep(1000)
        Console.Clear()
        Console.ResetColor()

let go2048 () =
    greeting ()
    let mutable state = (createBoard (), 0)
    printState state
    while not (gameOver state) do
        state <- (getMove () |> doMove state)
    Console.ForegroundColor <- ConsoleColor.Red
    printfn "Game Over!"
