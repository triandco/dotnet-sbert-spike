module WordPieceTokenizer

type WordPieceTokeniserOptions = { 
  Vocab: string
  UNKToken: string
}

let MAX_INPUT_CHAR_PER_WORD = 100

let whitespace_tokenize (text: string) = text.Trim().Split(" ")

// Tokenizes a piece of text into its word pieces. This uses a greedy longest-match-first algorithm to perform
// tokenization using the given vocabulary.
// For example, `input = "unaffable"` wil return as output `["un", "##aff", "##able"]`.
// Args:
//     text: A single token or whitespace separated tokens. This should have already been passed through *BasicTokenizer*.
// Returns   :
//     A list of wordpiece tokens.
let tokenize (options: WordPieceTokeniserOptions) text = 

    let processToken (token: string) : string seq =
      if String.length token > MAX_INPUT_CHAR_PER_WORD then [options.UNKToken] else
      
      let rec a (startIndex: int) (chars: char seq) : Option<string seq> = 
        let endIndex = Seq.length chars
        
        let rec b (begining: int) (ending: int) (charss: char seq) : Option<string> =
          if begining >= ending then None else
          let substr = charss |> Seq.toArray |> fun e -> e[begining..ending] |> System.String.Concat
          let sub = if begining <= 0 then substr else $"##{substr}"
          if options.Vocab.Contains(sub) then Some sub
          else b begining (ending - 1) charss

        match b startIndex endIndex chars with 
        | Some substr ->
          a endIndex chars
          |> Option.map (fun e -> e |> Seq.append [| substr |])
        | None -> None
        
      a 0 token |> Option.defaultValue [ options.UNKToken ]

    text 
    |> whitespace_tokenize
    |> Array.map processToken
    |> Seq.concat