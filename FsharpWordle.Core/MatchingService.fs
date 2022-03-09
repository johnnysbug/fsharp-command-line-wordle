namespace FsharpWordle.Core

module MatchingService =
    open Domain
    
    let private convert (word: string) =
        word.ToCharArray()
        |> Array.indexed
        |> Array.map (fun (i, c) -> { 
            Index = i
            Value = c
            Color = Gray })

    let matches (guess: string) (answer: string) : Letter array =
        let cg = convert guess
        let ca = convert answer
        let greens = 
            cg
            |> Array.filter (fun x -> guess[x.Index] = answer[x.Index])
            |> Array.map (fun x -> { x with Color = Green })

        let glg = cg |> Array.filter (fun g -> not <| Array.exists (fun l -> g.Index = l.Index) greens)
        let alg = ca |> Array.filter (fun a -> not <| Array.exists (fun l -> a.Index = l.Index) greens)

        let rec notGreens (guess: Letter array) (answer: Letter array) (results: Letter array) : Letter array =
            match guess with
            | [||] -> results
            | arr -> 
                let letter, remaining = Array.head arr, Array.tail arr
                match Array.tryFindIndex (fun l -> l.Value = letter.Value) answer with
                | Some i -> 
                    notGreens remaining (Array.removeAt i answer) (Array.append [| { letter with Color = Yellow } |] results)
                | None -> 
                    notGreens remaining answer (Array.append [| letter |] results)
        notGreens glg alg greens |> Array.sortBy (fun l -> l.Index)