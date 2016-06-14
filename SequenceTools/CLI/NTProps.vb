Imports LANS.SystemsBiology.AnalysisTools.SequenceTools.SequencePatterns
Imports LANS.SystemsBiology.AnalysisTools.SequenceTools.SequencePatterns.Topologically
Imports LANS.SystemsBiology.SequenceModel.FASTA
Imports LANS.SystemsBiology.SequenceModel.NucleotideModels
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.DocumentFormat.Csv
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Language.UnixBash
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Parallel.Linq

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

    ''' <summary>
    ''' 对位点进行分组操作方便进行MEME分析
    ''' </summary>
    ''' <param name="args"></param>
    ''' <returns></returns>
    <ExportAPI("/Mirrors.Group",
               Usage:="/Mirrors.Group /in <mirrors.Csv> [/out <out.DIR>]")>
    Public Function MirrorGroups(args As CommandLine) As Integer
        Dim [in] As String = args("/in")
        Dim outDIR As String = args.GetValue("/out", [in].TrimFileExt)
        Dim data As PalindromeLoci() = [in].LoadCsv(Of PalindromeLoci)

        For Each g In (From x As PalindromeLoci In data Select x Group x By x.Loci Into Group)
            Dim fa As FastaToken() =
                LinqAPI.Exec(Of FastaToken) <= From x As PalindromeLoci
                                               In g.Group
                                               Let uid As String = x.MappingLocation.ToString.NormalizePathString(True).Replace(" ", "_")
                                               Let atrs As String() = {uid, x.Loci}
                                               Select New FastaToken With {
                                                   .Attributes = atrs,
                                                   .SequenceData = x.Loci & x.Palindrome
                                               }
            Dim path As String = $"{outDIR}/{g.Loci}.fasta"

            Call New FastaFile(fa).Save(path, Encodings.ASCII)
        Next

        Return 0
    End Function

    <ExportAPI("/Mirrors.Group.Batch",
               Usage:="/Mirrors.Group.Batch /in <mirrors.DIR> [/out <out.DIR>]")>
    Public Function MirrorGroupsBatch(args As CommandLine) As Integer
        Dim inDIR As String = args - "/in"
        Dim CLI As New List(Of String)
        Dim task As Func(Of String, String) =
            Function(path) $"{GetType(Utilities).API(NameOf(MirrorGroups))} /in {path.CliPath}"

        For Each file As String In ls - l - r - wildcards("*.csv") <= inDIR
            CLI += task(file)
        Next

        Return App.SelfFolks(CLI, LQuerySchedule.CPU_NUMBER)
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