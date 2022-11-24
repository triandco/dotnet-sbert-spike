namespace Library

open System.Linq
open Microsoft.ML
open Microsoft.ML.Data
open Microsoft.ML.Transforms.Onnx


type OnnxInput() =
  [<
    ColumnName("input_ids");
    OnnxSequenceType(typedefof<int64 seq>)
  >]
  member val InputIds: int64 seq seq = [[]] with get, set

  [<
    ColumnName("attention_mask");
    OnnxSequenceType(typedefof<int64 seq>)
  >]
  member val AttentionMasks: int64 seq seq = [[]] with get, set

type OnnxOutput() =
  [<
    ColumnName("last_hidden_state");
    OnnxSequenceType(typedefof<float32 seq>) 
   >]
  member val LastHiddenState: float32 seq seq = [[]] with get, set


module Test = 

  let get_prediction_pipeline (file_path:string) (mlContext: MLContext) =
    
    let mutable options = OnnxOptions()
    options.InputColumns <- [|  "input_ids"; "attention_mask" |]
    options.OutputColumns <- [| "last_hidden_state" |]
    options.ModelFile <- file_path
    
    let onnxPredictionPipeline = mlContext.Transforms.ApplyOnnxModel options

    Enumerable.Empty<OnnxInput> ()
    |> mlContext.Data.LoadFromEnumerable
    |> onnxPredictionPipeline.Fit;



  let run file_path =

    let mlContext = MLContext()
    let engine = 
      get_prediction_pipeline file_path mlContext
      |> mlContext.Model.CreatePredictionEngine<OnnxInput, OnnxOutput>
    
    let exampleInput = OnnxInput()
    exampleInput.InputIds <- [[ 101; 7592; 2088;  102 ]]
    exampleInput.AttentionMasks <- [[ 1;1;1;1]]
    let outcome = engine.Predict exampleInput
    
    printfn "%A" outcome.LastHiddenState
