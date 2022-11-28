module WordPieceTokenizerTests

open Xunit
open WordPieceTokenizer

[<Fact>]
let ``My test`` () =
  let input = "unaffable"
  let expected = Set.ofList ["una"; "##af"; "##ffa"; "##able"]

  printf "running heck mate"
  input 
  |> tokenize (WordPieceTokeniserOptions.defaultValue ())
  |> Set.ofSeq
  |> fun value -> Assert.StrictEqual(expected, value)

