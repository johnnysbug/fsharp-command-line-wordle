module FsharpWordle.UI

open FsharpWordle.Core
open FsharpWordle.Core.Domain
open System

let rec main context =
    if context.RemainingTries = 0 then Environment.Exit 0
    
    RenderService.drawBoard(context.Board, context.Message)

    main(GameService.takeTurn({context with Guess = Console.ReadLine()}))

    main(context)

[<EntryPoint>]
main(GameService.createContext(WordService.randomWord()))