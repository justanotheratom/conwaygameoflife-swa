module ConwayGameOfLife

open System

type Cell =
    | Dead
    | Alive

type GameState =
    {
        Width: int
        Height: int
        Grid: Cell list
    }

let toXY index gameState =
    let x = index % gameState.Width
    let y = index / gameState.Width
    (x, y)

let toIndex x y gameState =
    y * gameState.Width + x

let random = Random()

let isCellAlive x y gameState =
    match gameState.Grid.[toIndex x y gameState] with
    | Dead -> false
    | Alive -> true

let countLiveNeighbors x y gameState =
    [ for i in (x-1)..(x+1) do
        for j in (y-1)..(y+1) do
            (i, j)
    ]
    |> List.filter (
        fun (i, j) ->
            i >= 0 && i < gameState.Width
            &&
            j >= 0 && j < gameState.Height
            &&
            (i, j) <> (x, y)
    )
    |> List.map (fun (i, j) -> gameState.Grid.[toIndex i j gameState])
    |> List.sumBy (fun cell ->
        match cell with
        | Dead -> 0
        | Alive -> 1
    )

let initialState width height =
    {
        Width = width
        Height = height
        Grid = [
            for y in 0..height-1 do
                for x in 0..width-1 do
                    if random.Next(0, 2) = 0 then
                        Dead
                    else
                        Alive
        ]
    }

let updateState gameState =
    {
        gameState with
            Grid = [
                for y in 0..gameState.Height-1 do
                    for x in 0..gameState.Width-1 do
                        let liveNeighbors = countLiveNeighbors x y gameState
                        match (isCellAlive x y gameState, liveNeighbors) with
                        | (true, 2) | (true, 3) | (false, 3) -> Alive
                        | _ -> Dead
            ]
    }