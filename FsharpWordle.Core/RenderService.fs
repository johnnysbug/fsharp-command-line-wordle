namespace FsharpWordle.Core

module RenderService =
    open System
    open Domain

    let private getColor (color) =
        match color with
        | Green -> ConsoleColor.DarkGreen
        | Yellow -> ConsoleColor.DarkYellow
        | Gray -> ConsoleColor.DarkGray
        | LightGray -> ConsoleColor.Gray
    
    let private drawCell (text, color) =
        Console.Write(" ")
        Console.BackgroundColor <- getColor (color)
        Console.ForegroundColor <- if color = Gray then ConsoleColor.White else ConsoleColor.Black
        Console.Write($" {text} ")
        Console.ResetColor()
        Console.Write("")

    let drawKeyboard keyboard =
        keyboard.Keys
        |> Array.iteri (fun i row -> 
            if i = 1 then Console.Write("  ")
            if i = 2 then Console.Write("       ")
            row |> Array.iter (fun cell -> drawCell cell)
            Console.WriteLine()
            Console.WriteLine())
        Console.WriteLine()

    let drawBoard (board: (char * Color)[,], message: string) =
        Console.Clear()
        let playableWidth = 41
        let title = "F# Commandline Wordle"
        let titleLength = title.Length
        let titlePadding = (playableWidth / 2) - titleLength / 2
        [1..titlePadding] |> Seq.iter (fun _ -> Console.Write(" "))

        Console.WriteLine($"{title}")
        Console.WriteLine()
        
        for y = 0 to Array2D.length2 board - 1 do
            Console.Write("          ")
            for x = 0 to Array2D.length1 board - 1 do
                drawCell board[x, y]
    
            Console.WriteLine()
            Console.WriteLine()
        
        let messageLength = message.Length
        let padding = (playableWidth / 2) - messageLength / 2
        [1..padding] |> Seq.iter (fun _ -> Console.Write(" "))
        Console.WriteLine($"{message}")
        Console.WriteLine()