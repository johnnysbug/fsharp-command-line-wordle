module FsharpWordle.UI

open FsharpWordle.Core
open FsharpWordle.Core.Domain
open System

let rec main context =    
    RenderService.drawBoard(context.Board, context.Message)
    RenderService.drawKeyboard(context.Keyboard)

    if context.RemainingTries = 0 then 
        Console.ReadKey() |> ignore
        Environment.Exit 0
    
    let info = Console.ReadKey(true)
    main(GameService.takeTurn({context with KeyPressed = Some(info)}))

[<EntryPoint>]
main(GameService.createContext(WordService.randomWord()))