module Tests

open Xunit
open FsharpWordle.Core.Domain
open FsharpWordle.Core

type TestData () =    
    let values : seq<obj[]>  =
        seq {
            yield [|"straw"; "straw"; [('s', Green); ('t', Green); ('r', Green); ('a', Green); ('w', Green)]|]
            yield [|"straw"; "train"; [('s', Gray); ('t', Yellow); ('r', Yellow); ('a', Yellow); ('w', Gray)]|]
            yield [|"straw"; "mulch"; [('s', Gray); ('t', Gray); ('r', Gray); ('a', Gray); ('w', Gray)]|]
            yield [|"class"; "smart"; [('c', Gray); ('l', Gray); ('a', Green); ('s', Yellow); ('s', Gray)]|]
            yield [|"smart"; "class"; [('s', Yellow); ('m', Gray); ('a', Green); ('r', Gray); ('t', Gray)]|]
            yield [|"quick"; "vivid"; [('q', Gray); ('u', Gray); ('i', Yellow); ('c', Gray); ('k', Gray)]|]
            yield [|"silly"; "lilly"; [('s', Gray); ('i', Green); ('l', Green); ('l', Green); ('y', Green)]|]
            yield [|"lilly"; "silly"; [('l', Gray); ('i', Green); ('l', Green); ('l', Green); ('y', Green)]|]
            yield [|"buddy"; "added"; [('b', Gray); ('u', Gray); ('d', Green); ('d', Yellow); ('y', Gray)]|]
            yield [|"added"; "buddy"; [('a', Gray); ('d', Yellow); ('d', Green); ('e', Gray); ('d', Gray)]|]
            yield [|"abate"; "eager"; [('a', Yellow); ('b', Gray); ('a', Gray); ('t', Gray); ('e', Yellow)]|]
            yield [|"eager"; "abate"; [('e', Yellow); ('a', Yellow); ('g', Gray); ('e', Gray); ('r', Gray)]|]
        }

    interface seq<obj[]> with
        member _.GetEnumerator () = values.GetEnumerator()
        member _.GetEnumerator () =
            values.GetEnumerator() :> System.Collections.IEnumerator

[<Fact>]
let ``WordService returns a five letter word`` () =
    let expectedLength = 5
    let word = WordService.randomWord()
    let actualLength = word.Length
    Assert.Equal(expectedLength, actualLength)

[<Fact>]
let ``WordService returns a different word each time`` () =
    let firstWord = WordService.randomWord()
    let secondWord = WordService.randomWord()
    Assert.NotEqual<string>(firstWord, secondWord)
    
[<Theory>]
[<ClassData(typeof<TestData>)>]
let ``MatchingService produces correct matches`` (guess: string, answer: string, matches: seq<char * Color>) =
    let actualMatches = MatchingService.matches guess answer
    let expectedMatches = 
        matches 
        |> Seq.mapi (fun i (l, c) -> { 
            Index = i 
            Value = l
            Color = c})
        |> Seq.toList
    Assert.Equal<List<Letter>>(expectedMatches, actualMatches)
        