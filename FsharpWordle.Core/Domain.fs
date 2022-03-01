namespace FsharpWordle.Core

module Domain =
    type Color = Green|Yellow|Gray|LightGray

    type Keyboard = {
        FirstRow: (char * Color) [];
        SecondRow: (char * Color) [];
        ThirdRow: (char * Color) [];}
    
    type Context = {
        Board: (char * Color) [,];
        Keyboard: Keyboard;
        Answer: string;
        Guess: string;
        RemainingTries: int;
        Message: string;}