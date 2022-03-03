# F# Wordle

Just a fun project to help learn F# by puzzling through [Wordle](https://www.nytimes.com/games/wordle/index.html) via the command line.

Game loop logic inspiration came from [CosX's](https://github.com/CosX) [snake.ai.fs project](https://github.com/CosX/snake.ai.fs)

---
## Requires
- .NET 6.0
- F# 6
---

## Building
From solution root, execute
```
dotnet build
```

## Running
From solution root, execute 
```
dotnet run --project ./FsharpWordle.Console/FsharpWordle.Console.fsproj
```
or from the `FsharpWordle.Console` folder, execute
```
dotnet run
```

## Instructions for playing
Based on the official [Wordle](https://www.nytimes.com/games/wordle/index.html) game. 

Once the application launches, guess a five letter word by typing from your keyboard. The on screen keyboard is simply there to indicate letters used in previous guesses. When you have typed in your guess, press the `Enter` or `Return` key on your keyboard. If you make a mistake with your word, you may press `Backspace` (or `Delete` on Mac) to erase one letter at a time.

You have six tries to guess the word. A new random word is generated every time the game is launched. This differs from the official game, where as a new word is generated each day.

### Matching
After submitting your guess:
- Green letters indicate that letter matched a letter in the word and is in the correct position
- Yellow letters indicate the letter matched matched a letter in the word, but it is not in the correct position
- Gray letters indicate that the letter is not in the word at all


---
## Examples of game in play
<p align="center">
<img src="https://user-images.githubusercontent.com/1800439/156499944-709539b1-104b-4832-9636-2345e3844609.png" width="33%"/>
<img src="https://user-images.githubusercontent.com/1800439/156500265-1aa87666-7f71-4fb3-9842-8ebdf0041bba.png" width="33%"/>
<img src="https://user-images.githubusercontent.com/1800439/156501097-79c17f4e-10cc-44de-9062-434587d0bdbb.png" width="33%"/>

</p>


## Troubleshooting
If the game isn't rendering correctly, try adjusting the size of your console or terminal window. The ideal size is greater than 41 characters wide and roughly 24 lines tall
