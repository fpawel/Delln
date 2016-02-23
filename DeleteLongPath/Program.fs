open System  
open System.IO
open System.Diagnostics

[<EntryPoint>]
let main argv = 
    printfn "%A" argv
    let pth = argv.[0]
    if argv.Length < 0 then printfn "usage: delln [path to folder]"; 0 else
    printfn "delln: deleting folder %A..." pth
    let emptyDirectory = new DirectoryInfo(Path.GetTempPath() + "\\TempEmptyDirectory-" + Guid.NewGuid().ToString())
    try    
        emptyDirectory.Create();
        use proc = new Process()
        let i = proc.StartInfo
        i.FileName <- "robocopy.exe"
        i.Arguments <- "\"" + emptyDirectory.FullName + "\" \"" + pth + "\" /mir /r:1 /w:1 /np /xj /sl"
        i.UseShellExecute <- false
        i.CreateNoWindow <- true
        if proc.Start() then
            proc.WaitForExit()        
            emptyDirectory.Delete()
            (new DirectoryInfo(pth)).Attributes <- FileAttributes.Normal
            Directory.Delete(pth,true)
        else
            printfn "error! cant start %A with %A" i.FileName i.Arguments
    with e ->
        printfn "error! %A" e
    0 
