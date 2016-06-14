Imports System.Runtime.CompilerServices
Imports LANS.SystemsBiology.AnalysisTools.SequenceTools.SequencePatterns
Imports LANS.SystemsBiology.AnalysisTools.SequenceTools.SequencePatterns.Topologically
Imports LANS.SystemsBiology.SequenceModel.FASTA
Imports LANS.SystemsBiology.SequenceModel.NucleotideModels
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.DocumentFormat.Csv
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Language.UnixBash
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Parallel
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
               Usage:="/Mirrors.Group /in <mirrors.Csv> [/batch /fuzzy <-1> /out <out.DIR>]")>
    <ParameterInfo("/fuzzy", True,
                   Description:="-1 means group sequence by string equals compared, and value of 0-1 means using string fuzzy compare.")>
    Public Function MirrorGroups(args As CommandLine) As Integer
        Dim [in] As String = args("/in")
        Dim outDIR As String = args.GetValue("/out", [in].TrimFileExt)
        Dim data As PalindromeLoci() = [in].LoadCsv(Of PalindromeLoci)
        Dim cut As Double = args.GetValue("/fuzzy", -1.0R)
        Dim batch As Boolean = args.GetBoolean("/batch")

        If cut > 0 Then
            For Each g As GroupResult(Of PalindromeLoci, String) In data.FuzzyGroups(
                Function(x) x.Loci, cut, parallel:=Not batch)

                Dim fa As FastaToken() =
                    LinqAPI.Exec(Of FastaToken) <= From x As PalindromeLoci In g.Group Select x.__lociFa
                Dim path As String = $"{outDIR}/{g.Tag}.fasta"

                Call New FastaFile(fa).Save(path, Encodings.ASCII)
            Next
        Else
            For Each g In (From x As PalindromeLoci In data Select x Group x By x.Loci Into Group)
                Dim fa As FastaToken() =
                    LinqAPI.Exec(Of FastaToken) <= From x As PalindromeLoci In g.Group Select x.__lociFa
                Dim path As String = $"{outDIR}/{g.Loci}.fasta"

                Call New FastaFile(fa).Save(path, Encodings.ASCII)
            Next
        End If

        Return 0
    End Function

    ''' <summary>
    ''' Converts the mirror palindrome site into a fasta sequence
    ''' </summary>
    ''' <param name="x"></param>
    ''' <returns></returns>
    <Extension>
    Private Function __lociFa(x As PalindromeLoci) As FastaToken
        Dim uid As String = x.MappingLocation.ToString.Replace(" ", "_")
        Dim atrs As String() = {uid, x.Loci}

        Return New FastaToken With {
            .Attributes = atrs,
            .SequenceData = x.Loci & x.Palindrome
        }
    End Function

    <ExportAPI("/Mirrors.Group.Batch",
               Usage:="/Mirrors.Group.Batch /in <mirrors.DIR> [/fuzzy <-1> /out <out.DIR> /num_threads <-1>]")>
    Public Function MirrorGroupsBatch(args As CommandLine) As Integer
        Dim inDIR As String = args - "/in"
        Dim CLI As New List(Of String)
        Dim fuzzy As String = args.GetValue("/fuzzy", "-1")
        Dim num_threads As Integer = args.GetValue("/num_threads", -1)
        Dim task As Func(Of String, String) =
            Function(path) _
                $"{GetType(Utilities).API(NameOf(MirrorGroups))} /in {path.CliPath} /batch /fuzzy {fuzzy}"

        For Each file As String In ls - l - r - wildcards("*.csv") <= inDIR
            CLI += task(file)
        Next

        Return App.SelfFolks(CLI, LQuerySchedule.AutoConfig(num_threads))
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