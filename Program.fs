open System.IO
open System

let word =
    File.ReadAllLines("words.txt")
    |> Array.filter (fun w -> w.Length = 5)
    |> Seq.sortBy (fun _ -> Guid.NewGuid())
    |> Seq.head

let mutable results = [||]

let draw ((guess: string), (word: string)) =
    guess
        |> Seq.mapi (fun i g -> 
            match word.IndexOf(g) with
            | w when w = i -> "🟩"
            | w when w > -1 -> 
                match word.IndexOf(g, i) with
                | w when w = i -> "🟩"
                | _ -> "🟨"
            | _ -> "⬛️")
        |> String.concat ""

let didWin (guess: string) (word: string) =
    match guess with
    | _ when guess = word -> 
        results <- Array.append results [| "🟩🟩🟩🟩🟩" |]
        false
    | _ when word.IndexOfAny (guess.ToCharArray()) > -1 -> 
        results <- Array.append results [| draw(guess, word) |]
        true
    | _ -> 
        results <- Array.append results [| "⬛️⬛️⬛️⬛️⬛️" |]
        true

printfn "Guess the word!"

let mutable tryAgain = true
while tryAgain do
    tryAgain <- didWin (Console.ReadLine()) word
    results |> Array.iter (printfn "%s")