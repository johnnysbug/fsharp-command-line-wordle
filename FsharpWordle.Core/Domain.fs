namespace FsharpWordle.Core

module Domain =
    type MatchColor = Green|Yellow|Gray
    
    type Context = {
        Board: (char * MatchColor) [,];
        Answer: string;
        Guess: string;
        RemainingTries: int;
        Message: string;
    }