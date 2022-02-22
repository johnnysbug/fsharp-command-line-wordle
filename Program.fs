open System.IO
open System

let word =
    File.ReadAllLines("words.txt")
    |> Array.filter (fun w -> w.Length = 5)
    |> Seq.sortBy (fun _ -> Guid.NewGuid()) // small trick to get a random-ish word
    |> Seq.head

let mutable results = [||]

let matchMap index (letter: char) (word: string) =
    match word.IndexOf(letter) with
    | wordIndex when wordIndex = index -> "🟩"
    | wordIndex when wordIndex > -1 -> 
        match word.IndexOf(letter, index) with
        | wordIndex when wordIndex = index -> "🟩"
        | _ -> "🟨"
    | _ -> "⬛️"

let draw ((guess: string), (word: string)) =
    let blocks = 
        guess
        |> Seq.distinct
        |> Seq.mapi (fun i l -> matchMap i l word)
        |> String.concat ""
    blocks.PadRight(8, "⬛️"[0])

let check (guess: string) (word: string) =
    match guess with
    | _ when guess = word -> 
        results <- Array.append results [| "🟩🟩🟩🟩🟩" |]
        printfn "You win!"
        true
    | _ when word.IndexOfAny (guess.ToCharArray()) > -1 -> 
        results <- Array.append results [| draw(guess, word) |]
        false
    | _ -> 
        results <- Array.append results [| "⬛️⬛️⬛️⬛️⬛️" |]
        false

printfn "Guess the word!"

let mutable didWin = false
while not didWin do
    didWin <- check (Console.ReadLine()) word 
    results |> Array.iter (printfn "%s")