Imports LANS.SystemsBiology.AnalysisTools.SequenceTools.SequencePatterns
Imports LANS.SystemsBiology.SequenceModel.NucleotideModels
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.DocumentFormat.Csv

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
               Usage:="")>
    Public Function ConvertMirrors(args As CommandLine) As Integer

    End Function
End Module