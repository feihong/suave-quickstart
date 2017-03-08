(*
Note that Files.browse is defined in Suave/Combinators.fs

*)
#r "../packages/Suave/lib/net40/Suave.dll"
open System
open System.IO
open Suave
open Suave.Filters
open Suave.Operators

let isDirectory (path : string) = 
  if File.Exists path then
    let attributes = File.GetAttributes(path)
    attributes.HasFlag(FileAttributes.Directory)
  else 
    false

let resolvePath (rootPath : string) (fileName : string) =
  let fileName =
    if Path.DirectorySeparatorChar.Equals('/') then fileName
    else fileName.Replace('/', Path.DirectorySeparatorChar)
  let calculatedPath =
    Path.Combine(rootPath, fileName.TrimStart([| Path.DirectorySeparatorChar; Path.AltDirectorySeparatorChar |]))
    |> Path.GetFullPath
    // If the path resolves to a directory, then use the index.html file in that directory.
    |> fun p -> if isDirectory p then Path.Combine(p, "index.html") else p
  printfn "%s" calculatedPath
  if calculatedPath.StartsWith rootPath then
    calculatedPath
  else raise <| Exception("File canonalization issue.")

let webBrowse rootPath : WebPart =
  warbler (fun ctx ->
    Files.file (resolvePath rootPath ctx.request.path))

let app : WebPart =
  choose [
    GET >=> webBrowse __SOURCE_DIRECTORY__
    RequestErrors.NOT_FOUND "Page not found." 
  ]

startWebServer defaultConfig app