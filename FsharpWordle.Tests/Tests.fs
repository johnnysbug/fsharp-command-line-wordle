module Tests

open Xunit
open FsharpWordle.Core.Domain
open FsharpWordle.Core.WordService
open FsharpWordle.Core

[<Fact>]
let ``WordService returns a five letter word`` () =
    let expectedLength = 5
    let word = randomWord()
    let actualLength = word.Length
    Assert.Equal(expectedLength, actualLength)

[<Fact>]
let ``WordService returns a different word each time`` () =
    let firstWord = randomWord()
    let secondWord = randomWord()
    Assert.NotEqual<string>(firstWord, secondWord)

type TestData () =    
    let values : seq<obj[]>  =
        seq {
            yield [|"straw"; "straw"; [Green; Green; Green; Green; Green]|]
            yield [|"straw"; "train"; [Gray; Yellow; Yellow; Yellow; Gray]|]
            yield [|"straw"; "mulch"; [Gray; Gray; Gray; Gray; Gray]|]
            yield [|"class"; "smart"; [Gray; Gray; Green; Yellow; Gray]|]
            yield [|"smart"; "class"; [Yellow; Gray; Green; Gray; Gray]|]
            yield [|"quick"; "vivid"; [Gray; Gray; Yellow; Gray; Gray]|]
            yield [|"dildo"; "vivid"; [Yellow; Green; Gray; Gray; Gray]|]
            yield [|"silly"; "lilly"; [Gray; Green; Green; Green; Green]|]
            yield [|"lilly"; "silly"; [Gray; Green; Green; Green; Green]|]
            yield [|"buddy"; "added"; [Gray; Gray; Green; Yellow; Gray]|]
            yield [|"added"; "buddy"; [Gray; Yellow; Green; Gray; Gray]|]
            yield [|"abate"; "eager"; [Yellow; Gray; Gray; Gray; Yellow]|]
            yield [|"eager"; "abate"; [Yellow; Yellow; Gray; Gray; Gray]|]
        }
    interface seq<obj[]> with
        member _.GetEnumerator () = values.GetEnumerator()
        member _.GetEnumerator () =
            values.GetEnumerator() :> System.Collections.IEnumerator

[<Theory>]
[<ClassData(typeof<TestData>)>]
let ``GameService produces correct matches`` (guess: string, answer: string, matches: seq<MatchColor>) =
    let actualMatches = GameService.matches guess answer
    Assert.Equal<seq<MatchColor>>(matches, actualMatches)
