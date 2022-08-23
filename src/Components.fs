namespace App

open Feliz
open Fable.Core.JS

type Components =

    [<ReactComponent>]
    static member ConwayGameOfLife() =

        let X = 30
        let Y = X
        let w = 10
        let h = w

        let (gameStarted, setGameStarted) = React.useState(false)
        let (gameState, setGameState) = React.useStateWithUpdater(ConwayGameOfLife.initialState X Y)

        let subscribeToTimer() =
            let subscriptionId =
                setInterval (fun _ -> setGameState (fun prevState -> ConwayGameOfLife.updateState prevState)) 500
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
                            style.marginBottom X
                            style.border(1, borderStyle.dotted, color.black)
                            style.width (X * w)
                            style.display.flex
                        ]
                        prop.children [
                            for y in 0..Y-1 do
                                Html.div [
                                    for x in 0..X-1 do
                                        Html.div [
                                            prop.style [
                                                style.width w
                                                style.height h
                                                if ConwayGameOfLife.isCellAlive x y gameState then
                                                    style.backgroundColor "black"
                                                else
                                                    style.backgroundColor "white"
                                            ]
                                        ]
                                ]
                        ]
                    ]
                    Html.button [
                        prop.text "Abandon game"
                        prop.onClick (
                            fun _ ->
                                setGameStarted false
                                setGameState (fun _ -> ConwayGameOfLife.initialState X Y)
                        )
                    ]
                    Html.button [
                        prop.text "Start a fresh game"
                        prop.onClick (
                            fun _ ->
                                setGameStarted false
                                setGameState (fun _ -> ConwayGameOfLife.initialState X Y)
                                setGameStarted true
                        )
                    ]
            ]
        ]