namespace FsharpWordle.Core

module KeyboardService =
    open Domain

    let keyboard = 
        let keys = [|
            [| 'q';'w';'e';'r';'t';'y';'u';'i';'o';'p' |] |> Array.map (fun c -> (c, LightGray))
            [| 'a';'s';'d';'f';'g';'h';'j';'k';'l' |] |> Array.map (fun c -> (c, LightGray))
            [| 'z';'x';'c';'v';'b';'n';'m' |] |> Array.map (fun c -> (c, LightGray))
        |]
        { Keys = keys }

    let updateKeyboard (keyboard: Keyboard) (guess: seq<(char * Color)>) =
        let keys = keyboard.Keys
        guess 
        |> Seq.iter (fun (key, color) ->
            for x = 0 to 2 do
                for y = 0 to keys[x].Length - 1 do
                    if fst (keys[x][y]) = key then 
                        keys[x][y] <- (key, color))
        { keyboard with Keys = keys }