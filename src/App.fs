module App

open Browser.Dom
open Fetch
open Thoth.Fetch
open Feliz

[<ReactComponent>]
let Counter () =
    let (count, setCount) = React.useState (0)

    Html.div [ Html.h1 count
               Html.button [ prop.text "Increment"
                             prop.onClick (fun _ -> setCount (count + 1)) ] ]

[<ReactComponent>]
let App () = React.fragment [ Counter(); ]

open Browser.Dom

ReactDOM.render (App(), document.getElementById "root")
