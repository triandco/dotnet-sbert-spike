module WordPieceTokenizer


type WordPieceTokeniserOptions = { 
  Vocab: string seq
  UNKToken: string 
  MaxInputCharPerWord: int
}

module WordPieceTokeniserOptions = 
  [<Literal>]
  let DEFAULT_UNK_TOKEN = "[UNK]"

  [<Literal>]
  let DEFAULT_MAX_INPUT_CHAR_PER_WORD = 100

  let defaultValue () = {
    Vocab = WordPieceTokeniserOptions.DEFAULT_VOCAB.Split("\n") 
    UNKToken = DEFAULT_UNK_TOKEN
    MaxInputCharPerWord = DEFAULT_MAX_INPUT_CHAR_PER_WORD
  }


// WordPieceTokenize translated from an [implementation by hugging transformer](https://github.com/huggingface/transformers/blob/61d3928bfb3029bceb5be3e68ca3d4bf8456758f/src/transformers/models/bert/tokenization_bert.py#L520)
// More about WordPiece tokenizer visit [huggingface](https://huggingface.co/course/chapter6/6?fw=pt)
let tokenize (options: WordPieceTokeniserOptions) text = 

    let processToken (token: string) : string seq =
      if String.length token > options.MaxInputCharPerWord then [options.UNKToken] else
      
      let rec a (startIndex: int) (chars: char seq) : Option<string seq> = 
        let endIndex = Seq.length chars
        if startIndex >= endIndex then Some [] else
        
          let rec b (begining: int) (ending: int) (charss: char seq) : Option<string * int> =
            if begining >= ending then None else
            let substr = charss |> Seq.toArray |> fun e -> e[begining..ending] |> System.String.Concat
            let sub = if begining > 0 then $"##{substr}" else substr
            if options.Vocab |> Seq.contains sub then Some (sub, ending)
            else b begining (ending - 1) charss

          match b startIndex endIndex chars with 
          | Some (substr, ending) ->
             let t = a ending chars |> Option.map (Seq.append [| substr |])
             t
          | None -> None

      a 0 token |> Option.defaultValue [ options.UNKToken ]

    let whitespace_tokenize (text: string) = text.Trim().Split(" ")
    
    text 
    |> whitespace_tokenize
    |> Array.map processToken
    |> Seq.concat