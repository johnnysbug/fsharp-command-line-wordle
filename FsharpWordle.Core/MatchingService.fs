namespace FsharpWordle.Core

module MatchingService =
    open Domain

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
            else seq { (i, g, Gray) })
        |> Seq.append (matchingPairs |> Seq.map(fun ((i, c), _) -> seq { (i, c, Green) }))
        |> Seq.concat
        |> Seq.distinct
        |> Seq.sortBy (fun (i, _, _) -> i)
        |> Seq.map (fun (_, l, c) -> (l, c))