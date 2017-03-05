#r "packages/Suave/lib/net40/Suave.dll"
#r "packages/Suave.Experimental/lib/net40/Suave.Experimental.dll"
open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.Html

type Teacher = { Name: string; Students: string list }
let model = { Name="Sarah"; Students=["Peter"; "Linda"; "Mariah"]}

let page (model: Teacher) =
    html [] [
        head [] [ title [] "Suave HTML" ]
        body [] [
            tag "h1" [] (text model.Name)
            tag "ul" [] [
                for item in model.Students do
                    yield tag "li" [] (item.ToUpper() |> text)]
            ]
    ] |> htmlToString

let app = 
    choose [
        path "/" >=> OK (page model)
    ]

startWebServer defaultConfig app