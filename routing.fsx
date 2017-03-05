#r "packages/Suave/lib/net40/Suave.dll"
open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful

let greet area (reqParams: (string * (string option)) list) =
    reqParams
    |> List.tryPick (fun (k,v) -> if k = "name" then v else None)
    |> function Some name -> name | None -> "World"
    |> sprintf "Hello from %s, %s!" area

let auth = 
    Authentication.authenticateBasic (fun (username, password) -> 
        username = "me" && password = "abc")

let app = 
    choose [
        path "/public" >=> choose [
            // Access the HttpRequest and check the query of form parameters 
            GET >=> request(fun request ->
                greet "public" request.query |> OK)
            POST >=> request(fun request -> 
                greet "public" request.form |> OK)
        ]
        // This route is protected by HTTP Basic Authentication 
        path "/private" >=> auth 
            (choose [
                GET >=> request(fun request -> 
                    greet "private" request.query |> OK)
                POST >=> request(fun request -> 
                    greet "private" request.form |> OK)
            ])
        RequestErrors.NOT_FOUND "Found no handlers"
    ]

startWebServer defaultConfig app