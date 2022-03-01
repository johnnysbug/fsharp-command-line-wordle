namespace FsharpWordle.Core

module GameService =
    open FsharpWordle.Core.Domain

    let private keyboard _ = 
        let firstRow = [| 'q';'w';'e';'r';'t';'y';'u';'i';'o';'p' |] |> Array.map (fun c -> (c, LightGray))
        let secondRow = [| 'a';'s';'d';'f';'g';'h';'j';'k';'l' |] |> Array.map (fun c -> (c, LightGray))
        let thirdRow = [| 'z';'x';'c';'v';'b';'n';'m' |] |> Array.map (fun c -> (c, LightGray))
        {   FirstRow = firstRow
            SecondRow = secondRow
            ThirdRow = thirdRow}

    let private updateKeyboard (keyboard: Keyboard) (guess: seq<(char * Color)>) =
        guess 
        |> Seq.iter (fun (key, color) ->
            let firstRowIndex = keyboard.FirstRow |> Array.tryFindIndex (fun (k, _) -> k = key)
            if firstRowIndex.IsSome then
                keyboard.FirstRow[firstRowIndex.Value] <- (key, color)
            else
                let secondRowIndex = keyboard.SecondRow |> Array.tryFindIndex (fun (k, _) -> k = key)
                if secondRowIndex.IsSome then
                    keyboard.SecondRow[secondRowIndex.Value] <- (key, color)
                else
                    let thirdRowIndex = keyboard.ThirdRow |> Array.tryFindIndex (fun (k, _) -> k = key)
                    if thirdRowIndex.IsSome then
                        keyboard.ThirdRow[thirdRowIndex.Value] <- (key, color))
        keyboard
        
    let private countChars = fun (c: char) (chars: seq<(int * char)>) -> 
        chars
        |> Seq.where (fun (_, g) -> g = c)
        |> Seq.length

    let private processMultipleMatches (guessChar: char) (pairs:seq<(int * char) * (int * char)>) =
        let guesses = pairs |> Seq.map (fun (l, _) -> l) |> Seq.where (fun (_, c) -> c = guessChar)
        let answers = pairs |> Seq.map (fun (_, r) -> r) |> Seq.where (fun (_, c) -> c = guessChar)
        let guessCount = countChars guessChar guesses
        let answerCount = countChars guessChar answers
        let diff = if guessCount > answerCount then answerCount else guessCount

        guesses
        |> Seq.take (diff)
        |> Seq.map (fun (i, c) -> (i, c, Yellow))
        |> Seq.append (guesses
            |> Seq.skip (diff)
            |> Seq.map (fun (i, c) -> (i, c, Gray)))

    let private winningAttempt attempt =
        attempt |> Seq.forall (fun (_, c) -> c = Green)

    let private winningMessage remainingTries =
        match remainingTries with
        | 5 -> "Amazing!"
        | 4 -> "Fantastic!"
        | 3 -> "Great!"
        | 2 -> "Not bad"
        | 1 -> "Cutting it close!"
        | _ -> "Phew!"
        
    let matches (guess:string) (answer:string) =
        let guessChars = guess.ToCharArray() |> Seq.indexed
        let answerChars = answer.ToCharArray() |> Seq.indexed
        let pairs = Seq.zip guessChars answerChars
        let matchingPairs = pairs |> Seq.where (fun ((_, g), (_, a)) -> g = a)
        let nonMatchingPairs = pairs |> Seq.except matchingPairs

        nonMatchingPairs
        |> Seq.map (fun ((i, g), _) -> 
            if Seq.exists (fun (_, (_, a)) -> a = g) nonMatchingPairs then
                processMultipleMatches g nonMatchingPairs
            else
                seq { (i, g, Gray) })
        |> Seq.append (matchingPairs |> Seq.map(fun ((i, c), _) -> seq { (i, c, Green) }))
        |> Seq.concat
        |> Seq.distinct
        |> Seq.sortBy (fun (i, _, _) -> i)
        |> Seq.map (fun (_, l, c) -> (l, c))

    let createContext answer =
        let board = Array2D.init 5 6 (fun _ _ -> (' ', Gray))
        { 
            Board = board
            Keyboard = keyboard()
            Answer = answer
            Guess = ""
            RemainingTries = 6
            Message = "Good luck!" }

    let takeTurn (context: Context) =
        if WordService.isValidGuess(context.Guess) then
            let attempt = matches context.Guess context.Answer
            let board = context.Board
            attempt |> Seq.iteri (fun i (l, c) ->
                board[i, 6 - context.RemainingTries] <- (l, c)
            )
            
            let keyboard = updateKeyboard context.Keyboard attempt
            let won = winningAttempt attempt

            {context with 
                Board = board
                Keyboard = keyboard
                RemainingTries = if won then 0 else context.RemainingTries - 1
                Message = if won then winningMessage(context.RemainingTries - 1) else ""}
        else
            {context with Message = "Not in Word list"}