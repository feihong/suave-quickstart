#r "packages/Suave/lib/net40/Suave.dll"
#r "packages/DotLiquid/lib/net451/DotLiquid.dll"
#r "packages/Suave.DotLiquid/lib/net40/Suave.DotLiquid.dll"
open Suave
open Suave.Filters
open Suave.Operators

type Teacher = { Name: string; Students: string list }
let model = { Name="Sarah"; Students=["Peter"; "Linda"; "Mariah"]}

DotLiquid.setTemplatesDir(__SOURCE_DIRECTORY__)

let app = 
    choose [ 
        path "/" >=> DotLiquid.page ("template.html") model 
    ]
startWebServer defaultConfig app