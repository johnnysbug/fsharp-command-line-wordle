namespace FsharpWordle.Core

module Domain =
    open System

    type Color = Green|Yellow|Gray|LightGray
    type Letter = { Index: int; Value: char; Color: Color }
    type Keyboard = { Keys: Letter array array; }
    
    type Context = {
        Board: Letter [,];
        Keyboard: Keyboard;
        Answer: string;
        Guess: string;
        KeyPressed: Option<ConsoleKeyInfo>;
        RemainingTries: int;
        Message: string;}