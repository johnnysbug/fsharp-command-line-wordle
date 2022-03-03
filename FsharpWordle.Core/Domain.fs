namespace FsharpWordle.Core

module Domain =
    open System

    type Color = Green|Yellow|Gray|LightGray

    type Keyboard = {
        Keys: (char * Color) array array;}
    
    type Context = {
        Board: (char * Color) [,];
        Keyboard: Keyboard;
        Answer: string;
        Guess: string;
        KeyPressed: Option<ConsoleKeyInfo>;
        RemainingTries: int;
        Message: string;}