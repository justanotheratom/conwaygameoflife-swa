namespace App

open Feliz
open Fable.Core.JS

type Components () =

    [<Literal>]
    static let boardWidth = 30
    [<Literal>]
    static let boardHeight = boardWidth

    [<Literal>]
    static let cellWidth = 10
    [<Literal>]
    static let cellHeight = cellWidth

    [<Literal>]
    static let updateIntervalMs = 500
    [<Literal>]
    static let msPerSecond = 1000

    static let mutable updateCount : int = 0

    [<ReactComponent>]
    static member ConwayGameOfLife() =


        let (gameStarted, setGameStarted) = React.useState(false)
        let (gameState, setGameState) = React.useStateWithUpdater(ConwayGameOfLife.initialState boardWidth boardHeight)

        let updateGameState updateFn =
            updateCount <- updateCount + 1
            setGameState updateFn
        
        let resetGameState () =
            updateCount <- 0
            setGameState (fun _ -> ConwayGameOfLife.initialState boardWidth boardHeight)

        let subscribeToTimer() =
            let subscriptionId =
                setInterval (fun _ -> updateGameState (fun prevState -> ConwayGameOfLife.updateState prevState))
                            updateIntervalMs
            React.createDisposable (fun _ -> clearTimeout subscriptionId)

        React.useEffect(subscribeToTimer, [| |])

        Html.div [
            prop.style [ style.textAlign.center ]
            prop.children [
                Html.h1 "Conway's Game of Life"
                if not gameStarted then
                    Html.button [
                        prop.text "Start game"
                        prop.onClick (fun _ -> setGameStarted true)
                    ]
                else
                    Html.div [
                        prop.style [
                            style.margin.auto
                            style.marginBottom boardWidth
                            style.border(1, borderStyle.dotted, color.black)
                            style.width (boardWidth * cellWidth)
                            style.display.flex
                        ]
                        prop.children [
                            for y in 0..boardHeight-1 do
                                Html.div [
                                    for x in 0..boardWidth-1 do
                                        Html.div [
                                            prop.style [
                                                style.width cellWidth
                                                style.height cellHeight
                                                if ConwayGameOfLife.isCellAlive x y gameState then
                                                    style.backgroundColor "black"
                                                else
                                                    style.backgroundColor "white"
                                            ]
                                        ]
                                ]
                        ]
                    ]
                    Html.p [
                        Html.text (sprintf "Elapsed time: %u seconds" ((updateCount * updateIntervalMs) / msPerSecond))
                    ]
                    Html.div [
                        Html.button [
                            prop.text "Abandon game"
                            prop.onClick (
                                fun _ ->
                                    setGameStarted false
                                    resetGameState
                            )
                        ]
                        Html.button [
                            prop.text "Start a fresh game"
                            prop.onClick (
                                fun _ ->
                                    setGameStarted false
                                    resetGameState
                                    setGameStarted true
                            )
                        ]
                    ]
            ]
        ]