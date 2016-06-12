Imports LANS.SystemsBiology.AnalysisTools.SequenceTools.SequencePatterns
Imports LANS.SystemsBiology.AnalysisTools.SequenceTools.SequencePatterns.Topologically
Imports LANS.SystemsBiology.SequenceModel.NucleotideModels
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.DocumentFormat.Csv
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Language.UnixBash

Partial Module Utilities

    ''' <summary>
    ''' 自动根据文件的头部进行转换
    ''' </summary>
    ''' <param name="args"></param>
    ''' <returns></returns>
    <ExportAPI("/SimpleSegment.AutoBuild",
               Usage:="/SimpleSegment.AutoBuild /in <locis.csv> [/out <out.csv>]")>
    Public Function ConvertsAuto(args As CommandLine) As Integer
        Dim [in] As String = args("/in")
        Dim out As String = args.GetValue("/out", [in].TrimFileExt & ".Locis.Csv")
        Dim df As DocumentStream.File = DocumentStream.File.Load([in])
        Dim result As SimpleSegment() = df.ConvertsAuto
        Return result.SaveTo(out).CLICode
    End Function

    <ExportAPI("/SimpleSegment.Mirrors",
               Usage:="/SimpleSegment.Mirrors /in <in.csv> [/out <out.csv>]")>
    Public Function ConvertMirrors(args As CommandLine) As Integer
        Dim [in] As String = args - "/in"
        Dim out As String = args.GetValue("/out", [in].TrimFileExt & ".SimpleSegments.Csv")
        Dim data As PalindromeLoci() = [in].LoadCsv(Of PalindromeLoci)
        Dim sites As SimpleSegment() = data.ToArray(AddressOf MirrorsLoci)

        Return sites.SaveTo(out).CLICode
    End Function

    <ExportAPI("/SimpleSegment.Mirrors.Batch",
             Usage:="/SimpleSegment.Mirrors.Batch /in <in.DIR> [/out <out.DIR>]")>
    Public Function ConvertMirrorsBatch(args As CommandLine) As Integer
        Dim [in] As String = args - "/in"
        Dim out As String = args.GetValue("/out", [in].TrimDIR & ".SimpleSegments/")

        For Each file As String In ls - l - r - wildcards("*.csv") <= [in]
            Dim data As PalindromeLoci() = file.LoadCsv(Of PalindromeLoci)
            Dim path As String = $"{out}/{file.BaseName}.Csv"
            Dim sites As SimpleSegment() = data.ToArray(AddressOf MirrorsLoci)

            Call sites.SaveTo(path)
        Next

        Return 0
    End Function
End Module