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
        keyboard.FirstRow |> Seq.iter (fun k -> drawCell k)
        Console.WriteLine()
        Console.WriteLine()
        Console.Write("  ")
        keyboard.SecondRow |> Seq.iter (fun k -> drawCell k)
        Console.WriteLine()
        Console.WriteLine()
        Console.Write("       ")
        keyboard.ThirdRow |> Seq.iter (fun k -> drawCell k)
        Console.WriteLine()


    let drawBoard (board: (char * Color)[,], message: string) =
        Console.Clear()
        Console.WriteLine("              F# Commandline Wordle ")
        Console.WriteLine()
        
        for y = 0 to Array2D.length2 board - 1 do
            Console.Write("           ")
            for x = 0 to Array2D.length1 board - 1 do
                drawCell board[x, y]
    
            Console.WriteLine()
            Console.WriteLine()
    
        Console.WriteLine($"             {message}")
        Console.WriteLine()