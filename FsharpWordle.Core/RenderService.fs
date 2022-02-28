namespace FsharpWordle.Core

module RenderService =
    open System
    open Domain

    let private getColor (color) =
        match color with
        | Green -> ConsoleColor.DarkGreen
        | Yellow -> ConsoleColor.DarkYellow
        | Gray -> ConsoleColor.DarkGray
    
    let private drawCell (text, color) =
        Console.Write(" ")
        Console.BackgroundColor <- getColor (color)
        Console.ForegroundColor <- if color = Yellow || color = Green then ConsoleColor.Black else ConsoleColor.White
        Console.Write($" {text} ")
        Console.ResetColor()
        Console.Write(" ")

    let drawBoard (board: (char * MatchColor)[,], message: string) =
        Console.Clear()
        Console.WriteLine("  F# Commandline Wordle ")
        Console.WriteLine()
        
        for y = 0 to Array2D.length2 board - 1 do
            for x = 0 to Array2D.length1 board - 1 do
                drawCell board[x, y]
    
            Console.WriteLine()
            Console.WriteLine()
    
        Console.WriteLine()
        Console.WriteLine(message)