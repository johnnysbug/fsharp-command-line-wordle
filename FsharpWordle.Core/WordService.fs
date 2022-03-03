namespace FsharpWordle.Core

module WordService =
    open System.IO
    open System.Reflection
    open System

    let private readFile fileName =
        let assembly = Assembly.GetExecutingAssembly()
        let resourceName = $"FsharpWordle.Core.Resources.{fileName}"
        using (assembly.GetManifestResourceStream resourceName) (fun stream ->
            using (new StreamReader(stream)) (fun reader -> 
                reader.ReadToEnd().Split(Environment.NewLine)))

    let private answers = readFile "answers.txt"
    let private guesses = readFile "allowed-guesses.txt"
    let private combined = Seq.append guesses answers

    let randomWord () = 
        answers
        |> Seq.sortBy (fun _ -> Guid.NewGuid()) // get a random-ish word
        |> Seq.head

    let isValidGuess guess =
        combined
        |> Seq.contains guess

    let isLongEnough (guess: string) =
        guess.Length = 5