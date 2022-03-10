module FsharpWordle.UI

open FsharpWordle.Core
open FsharpWordle.Core.Domain
open System

let mutable windowWidth = Console.WindowWidth

let rec main context =
    if windowWidth <> Console.WindowWidth then
        windowWidth <- Console.WindowWidth
        Console.Clear()
        
    RenderService.drawBoard(context.Board, context.Message)
    RenderService.drawKeyboard(context.Keyboard)

    if context.RemainingTries = 0 then 
        Console.ReadKey() |> ignore
        Environment.Exit 0
    
    let info = Console.ReadKey(true)
    main(GameService.takeTurn({context with KeyPressed = Some(info)}))

[<EntryPoint>]
Console.Clear()
Console.CursorVisible <- false
main(GameService.createContext(WordService.randomWord()))