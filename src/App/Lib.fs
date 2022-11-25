module SentenceEmbedding

open System.Linq
open Microsoft.ML.OnnxRuntime
open Microsoft.ML.OnnxRuntime.Tensors

  
type Input = {
  InputIds: DenseTensor<int64>
  AttentionMask: DenseTensor<int64>  
}

let encodeWithModel (modelPath: string) (input: Input) : Tensor<float32>=
  
  use session = new InferenceSession(modelPath)
  let output = 
    [|
      NamedOnnxValue.CreateFromTensor("input_ids", input.InputIds)
      NamedOnnxValue.CreateFromTensor("attention_mask", input.AttentionMask)
    |]
    |> session.Run 

  output.Single().AsTensor<float32>()

let tokenize (_: string seq) : Input =
  raise <| System.NotImplementedException ()
  
