namespace GogApi.DotNet.UnitTests

module DiscriminatedUnionHelper =
    open Microsoft.FSharp.Reflection

    let GetAllUnionCases<'T>() =
        FSharpType.GetUnionCases(typeof<'T>)
        |> Seq.map (fun x -> FSharpValue.MakeUnion(x, Array.zeroCreate(x.GetFields().Length)) :?> 'T)
