Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports LANS.SystemsBiology.SequenceModel.FASTA
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Microsoft.VisualBasic
Imports LANS.SystemsBiology.SequenceModel.FASTA.Reflection
Imports LANS.SystemsBiology.SequenceModel

<[PackageNamespace]("SequenceTools",
                    Category:=APICategories.ResearchTools,
                    Description:="Sequence search tools and sequence operation tools",
                    Publisher:="xie.guigang@gmail.com")>
Public Module ShellScriptAPI

    <ExportAPI("search.title_keyword")>
    Public Function SearchByTitleKeyword(fasta As FastaFile, Keyword As String) As LANS.SystemsBiology.SequenceModel.FASTA.FastaFile
        Dim LQuery = (From fa As FastaToken In fasta
                      Where InStr(fa.Title, Keyword, CompareMethod.Binary) > 0
                      Select fa).ToArray
        Return LQuery
    End Function

    <ExportAPI("reverse")>
    Public Function Reverse(fasta As LANS.SystemsBiology.SequenceModel.FASTA.FastaFile) As LANS.SystemsBiology.SequenceModel.FASTA.FastaFile
        Return fasta.Reverse
    End Function

    <ExportAPI("reverse")>
    Public Function Reverse(fasta As LANS.SystemsBiology.SequenceModel.FASTA.FastaToken) As LANS.SystemsBiology.SequenceModel.FASTA.FastaFile
        Return fasta.Reverse
    End Function

    <ExportAPI("Read.Fasta")>
    Public Function ReadFile(file As String) As LANS.SystemsBiology.SequenceModel.FASTA.FastaFile
        Return LANS.SystemsBiology.SequenceModel.FASTA.FastaFile.Read(file)
    End Function

    <ExportAPI("write.fasta")>
    Public Function WriteFile(fasta As LANS.SystemsBiology.SequenceModel.FASTA.FastaFile, file As String) As Boolean
        Call fasta.Save(file)
        Return 0
    End Function

    <ExportAPI("get_fasta")>
    Public Function GetObject(fasta As LANS.SystemsBiology.SequenceModel.FASTA.FastaFile, index As Integer) As LANS.SystemsBiology.SequenceModel.FASTA.FastaToken
        Return fasta.Item(index)
    End Function

    <ExportAPI("get_sequence")>
    Public Function GetSequenceData(fsa As LANS.SystemsBiology.SequenceModel.FASTA.FastaToken) As String
        Return fsa.SequenceData
    End Function

    ''' <summary>
    ''' 使用正则表达式搜索目标序列
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <ExportAPI("-Pattern_Search", Info:="Parsing the sequence segment from the sequence source using regular expression.",
        Usage:="-pattern_search fasta <fasta_object> pattern <regex_pattern> output <output_directory>")>
    <ParameterInfo("-p",
        Description:="This switch specific the regular expression pattern for search the sequence segment,\n" &
                     "for more detail information about the regular expression please read the user manual.",
        Example:="N{1,5}TA")>
    <ParameterInfo("-o", True,
        Description:="Optional, this switch value specific the output directory for the result data, default is user Desktop folder.",
        Example:="~/Documents/")>
    Public Function PatternSearchA(fasta As LANS.SystemsBiology.SequenceModel.FASTA.FastaFile, pattern As String, output As String) As Integer
        pattern = pattern.Replace("N", "[ATGCU]")

        If String.IsNullOrEmpty(output) Then
            output = My.Computer.FileSystem.SpecialDirectories.Desktop
        End If

        Dim Csv = LANS.SystemsBiology.AnalysisTools.SequenceTools.Pattern.Match(Seq:=fasta, pattern:=pattern)
        Dim Complement = LANS.SystemsBiology.AnalysisTools.SequenceTools.Pattern.Match(Seq:=fasta.Complement(), pattern:=pattern)
        Dim Reverse = LANS.SystemsBiology.AnalysisTools.SequenceTools.Pattern.Match(Seq:=fasta.Reverse, pattern:=pattern)

        Call Csv.Insert(rowId:=-1, Row:={"Match pattern:=", pattern})
        Call Complement.Insert(rowId:=-1, Row:={"Match pattern:=", pattern})
        Call Reverse.Insert(rowId:=-1, Row:={"Match pattern:=", pattern})

        Call Csv.Save(output & "/sequence.csv", False)
        Call Complement.Save(output & "/sequence_complement.csv", False)
        Call Reverse.Save(output & "/sequence_reversed.csv", False)

        Return 0
    End Function

    <ExportAPI("Search")>
    Public Function PatternSearch(Fasta As LANS.SystemsBiology.SequenceModel.FASTA.FastaToken, Pattern As String) As Pattern.SegLoci()

    End Function

    <ExportAPI("Loci.Find.Location", Info:="Found out all of the loci site on the target sequence.")>
    Public Function FindLocation(Sequence As I_PolymerSequenceModel, Loci As String) As Integer()
        Return FindLocation(Sequence.SequenceData, Loci)
    End Function

    ''' <summary>
    '''
    ''' </summary>
    ''' <param name="Sequence"></param>
    ''' <param name="Loci"></param>
    ''' <returns></returns>
    ''' <remarks>这个位置查找函数是OK的</remarks>
    <ExportAPI("Loci.Find.Location", Info:="Found out all of the loci site on the target sequence.")>
    Public Function FindLocation(Sequence As String, Loci As String) As Integer()
        Dim Locis = New List(Of Integer)
        Dim p As Integer = 1

        Do While True
            p = InStr(Start:=p, String1:=Sequence, String2:=Loci)
            If p > 0 Then
                Call Locis.Add(p)
                p += 1
            Else
                Exit Do
            End If
        Loop

        Return Locis.ToArray
    End Function

    <ExportAPI("loci.match.location")>
    Public Function MatchLocation(Sequence As String, Loci As String, Optional cutoff As Double = 0.65) As Topologically.SimilarityMatches.LociMatchedResult()
        Return Topologically.SimilarityMatches.MatchLociLocations(Sequence, Loci, Len(Loci) / 3, Len(Loci) * 5, cutoff)
    End Function

    <ExportAPI("Align")>
    Public Function Align(query As FastaToken, subject As FastaToken, Optional cost As Double = 0.7) As AlignmentResult
        Return New AlignmentResult(query, subject, cost)
    End Function
End Module
