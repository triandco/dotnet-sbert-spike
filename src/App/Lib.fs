namespace Library

open System.Linq
open Microsoft.ML
open Microsoft.ML.Data
open Microsoft.ML.Transforms.Onnx
open Microsoft.ML.OnnxRuntime
open Microsoft.ML.OnnxRuntime.Tensors


module Test = 

  // run model with a sample input
  let run (file_path: string) =

    let inputIds  = DenseTensor<int64> [| 01; 7592; 2088;  102 |]
    let attentionMask  = DenseTensor<int64> [| 01; 7592; 2088;  102 |]

    let input = [|
      NamedOnnxValue.CreateFromTensor("input_ids", inputIds)
      NamedOnnxValue.CreateFromTensor("attention_mask", attentionMask)
    |]

    use session = new InferenceSession(
      file_path, 
      new SessionOptions(
        EnableMemoryPattern=true, 
        EnableCpuMemArena=true, 
        LogSeverityLevel=OrtLoggingLevel.ORT_LOGGING_LEVEL_VERBOSE))
    
    let result = session.Run input 

    printf "%A" result
