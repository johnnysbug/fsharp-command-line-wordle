namespace FsharpWordle.Core

module MatchingService =
    open Domain
    
    let private convert (word: string) =
        word.ToCharArray()
        |> Array.toList
        |> List.indexed
        |> List.map (fun (i, c) -> { 
            Index = i
            Value = c
            Color = Gray })
    
    let matches (guess: string) (answer: string) : Letter list =
        let cg = convert guess
        let ca = convert answer
        let greens = 
            cg
            |> List.filter (fun x -> guess[x.Index] = answer[x.Index])
            |> List.map (fun x -> { x with Color = Green })

        let glg = cg |> List.filter (fun g -> not <| List.exists (fun l -> g.Index = l.Index) greens)
        let alg = ca |> List.filter (fun a -> not <| List.exists (fun l -> a.Index = l.Index) greens)
    
        let rec notGreens (guess: Letter list) (answer: Letter list) (results: Letter list) : Letter list =
            match guess with
            | [] -> results
            | letter :: remaining -> 
                match List.tryFindIndex (fun l -> l.Value = letter.Value) answer with
                | Some i -> 
                    let remainingAnswer = List.removeAt i answer
                    notGreens remaining remainingAnswer ({ letter with Color = Yellow } :: results)
                | None -> notGreens remaining answer (letter :: results) 
        notGreens glg alg greens |> List.sortBy (fun l -> l.Index)