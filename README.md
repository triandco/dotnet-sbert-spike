# Introduction
This repository is an experiment to get [distilbert-base-tas-b](https://huggingface.co/sentence-transformers/msmarco-distilbert-base-tas-b) exported as onnx model to run in a dotnet project. The model was exported using the [🤗transformer guide](https://huggingface.co/docs/transformers/serialization). A pre-generated model can be [downloaded here](https://drive.google.com/file/d/1nTyKTDcbzMPvH_ewloaYGBMpGZ51alF9/view?usp=sharing).

# Running the project
Since the model is large, it is stored separately from the project.

1. Download [model.onnx](https://drive.google.com/file/d/1nTyKTDcbzMPvH_ewloaYGBMpGZ51alF9/view?usp=sharing)
1. Copy model.onnx to src/App
1. Run 
```powershell
cd src/App
dotnet restore
dotnet build
dotnet run
```