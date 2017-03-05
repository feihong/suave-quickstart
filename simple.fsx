#r "packages/Suave/lib/net40/Suave.dll"
open Suave
open System.Net

let config = 
    { defaultConfig with 
        bindings = [HttpBinding.create HTTP IPAddress.Loopback 8000us] }
startWebServer config (Successful.OK "Hello World!")