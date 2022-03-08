namespace FsharpWordle.Core

module GameService =
    open Domain
    open System

    let private winningAttempt attempt =
        attempt |> List.forall (fun l -> l.Color = Green)

    let private winningMessage remainingTries =
        match remainingTries with
        | 5 -> "Amazing!"
        | 4 -> "Fantastic!"
        | 3 -> "Great!"
        | 2 -> "Not bad"
        | 1 -> "Cutting it close!"
        | _ -> "Phew!"

    let private updateBoard context attempt =
        let board = context.Board
        attempt |> List.iteri (fun i l -> board[i, 6 - context.RemainingTries] <- l)
        { context with Board = board }
    
    let private processEnter context =
        if not(WordService.isLongEnough context.Guess) then
            {context with Message = "Not enough letters"}
        elif WordService.isValidGuess(context.Guess) then
            let attempt = MatchingService.matches context.Guess context.Answer
            let updatedBoardContext = updateBoard context attempt
            let keyboard = KeyboardService.updateKeyboard updatedBoardContext.Keyboard attempt
            let won = winningAttempt attempt

            { updatedBoardContext with 
                Keyboard = keyboard
                Guess = ""
                KeyPressed = None
                RemainingTries = if won then 0 else context.RemainingTries - 1
                Message = if won then winningMessage(context.RemainingTries - 1) else "" }
        else { context with Message = "Not in Word list" }

    let private processBackspace context =
        let guessLength = context.Guess.Length
        if guessLength > 0 then
            let guess = context.Guess.Substring(0, guessLength - 1).PadRight(5, ' ')
            let guessBuffer = 
                guess
                |> Seq.map (fun c -> { 
                    Index = 0
                    Value = c
                    Color = Gray })
                 |> Seq.toList
            let updatedBoardContext = updateBoard context guessBuffer
            {updatedBoardContext with 
                Guess = guess.Replace(" ", "")
                Message = ""}
        else context

    let private processLetter (letter: char) context =
        let guess = context.Guess
        if guess.Length < 5 then
            let updatedGuess = 
                sprintf "%s%s" guess (letter.ToString().ToLowerInvariant())
            let guessBuffer = 
                updatedGuess
                |> Seq.map (fun c -> { 
                    Index = 0
                    Value = c
                    Color = Gray })
                |> Seq.toList
            let updatedBoardContext = updateBoard context guessBuffer
            {updatedBoardContext with 
                Guess = updatedGuess 
                Message = ""}
        else context

    let private processTurn (keyInfo: ConsoleKeyInfo) (context: Context) =
        match keyInfo with
        | keyInfo when keyInfo.Key = ConsoleKey.Enter -> processEnter context
        | keyInfo when keyInfo.Key = ConsoleKey.Backspace -> processBackspace context
        | keyInfo when Char.IsLetter(keyInfo.KeyChar) -> processLetter keyInfo.KeyChar context
        | _ -> context

    let takeTurn (context: Context) =
        let keyInfo = context.KeyPressed
        match keyInfo with
        | Some(keyInfo) -> processTurn keyInfo context
        | None -> context

    let createContext answer =
        let board = Array2D.init 5 6 (fun _ _ -> { 
            Index = 0
            Value = ' '
            Color = Gray })
        {   Board = board
            Keyboard = KeyboardService.keyboard
            Answer = answer
            Guess = ""
            KeyPressed = None
            RemainingTries = 6
            Message = "Good luck!" }