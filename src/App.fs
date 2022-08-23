module App

open App
open Browser.Dom
open Fetch
open Thoth.Fetch
open Feliz


[<ReactComponent>]
let App () = React.fragment [ Components.ConwayGameOfLife() ]

open Browser.Dom

ReactDOM.render (App(), document.getElementById "root")