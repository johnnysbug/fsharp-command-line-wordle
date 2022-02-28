namespace FsharpWordle.Core

module WordService =
    open System.IO
    open System

    let private answers = File.ReadAllLines("answers.txt")
    let private guesses = File.ReadAllLines("allowed-guesses.txt")
    let private combined = Seq.append guesses answers

    let randomWord () = 
        answers
        |> Seq.sortBy (fun _ -> Guid.NewGuid()) // get a random-ish word
        |> Seq.head

    let isValidGuess guess =
        combined
        |> Seq.contains guess