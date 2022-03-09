namespace FsharpWordle.Core

module KeyboardService =
    open Domain

    let private toLetter c color = { 
        Index = 0 
        Value = c 
        Color = color }

    let keyboard = 
        let keys = [|
            [| 'q';'w';'e';'r';'t';'y';'u';'i';'o';'p' |] |> Array.map (fun c -> toLetter c LightGray)
            [| 'a';'s';'d';'f';'g';'h';'j';'k';'l' |] |> Array.map (fun c -> toLetter c LightGray)
            [| 'z';'x';'c';'v';'b';'n';'m' |] |> Array.map (fun c -> toLetter c LightGray)
        |]
        { Keys = keys }

    let updateKeyboard (keyboard: Keyboard) (guess: Letter array) =
        let keys = keyboard.Keys
        guess 
        |> Array.iter (fun l ->
            for x = 0 to 2 do
                for y = 0 to keys[x].Length - 1 do
                    if (keys[x][y]).Value = l.Value then 
                        keys[x][y] <- l)
        { keyboard with Keys = keys }