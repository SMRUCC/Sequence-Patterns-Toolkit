Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.DocumentFormat.Csv.DocumentStream.File
Imports LANS.SystemsBiology.Assembly.NCBI.GenBank.Extensions
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Microsoft.VisualBasic
Imports LANS.SystemsBiology.AnalysisTools.SequenceTools.Pattern
Imports LANS.SystemsBiology.SequenceModel.FASTA.Reflection
Imports System.Drawing
Imports LANS.SystemsBiology.SequenceModel.FASTA
Imports LANS.SystemsBiology.SequenceModel
Imports LANS.SystemsBiology.AnalysisTools.SequenceTools.SequencePatterns
Imports LANS.SystemsBiology.Assembly.NCBI.GenBank

''' <summary>
''' Sequence Utilities
''' </summary>
''' <remarks></remarks>
''' 
<PackageNamespace("Seqtools.Utilities.CLI",
                  Category:=APICategories.CLI_MAN,
                  Description:="Sequence operation utilities",
                  Publisher:="xie.guigang@gmail.com")>
Public Module Utilities

    <ExportAPI("-321", Info:="Polypeptide sequence 3 letters to 1 lettes sequence.", Usage:="-321 /in <sequence.txt> [/out <out.fasta>]")>
    Public Function PolypeptideBriefs(args As CommandLine) As Integer
        Dim [In] As String = args.GetString("/in")
        Dim Sequence As String = FileIO.FileSystem.ReadAllText([In]).Replace(vbCr, "").Replace(vbLf, "")
        Dim AA As New List(Of String)
        For i As Integer = 0 To Sequence.Length - 3 Step 3
            Call AA.Add(Mid(Sequence, i + 1, 3))
        Next
        Dim LQuery = New String((From a As String In AA Select Polypeptides.Abbreviate(a.ToUpper).First).ToArray)
        Dim MW As Double = CalcMW_Polypeptide(LQuery)
        Dim Fasta As New FastaToken With {.SequenceData = LQuery, .Attributes = {$"MW={MW}"}}
        Dim out As String = args("/out")
        If String.IsNullOrEmpty(out) Then
            out = [In] & ".fasta"
        End If
        Call Fasta.SaveAsOneLine(out)
        Return 0
    End Function

    <ExportAPI("-complement", Usage:="-reverse -i <input_fasta> [-o <output_fasta>]")>
    Public Function Complement(argvs As CommandLine) As Integer
        Dim InputFasta As String = argvs("-i")
        Dim OutputFasta As String = argvs("-o")

        If String.IsNullOrEmpty(InputFasta) Then
            Call Console.WriteLine("No fasta sequence was input!")
        ElseIf Not FileIO.FileSystem.FileExists(InputFasta) Then
            Call Console.WriteLine("Fasta file ""{0}"" is not exisist on your filesystem!", InputFasta)
        Else
            If String.IsNullOrEmpty(OutputFasta) Then
                Try
                    Dim FileInfo = FileIO.FileSystem.GetFileInfo(InputFasta)
                    OutputFasta = String.Format("{0}/{1}_reverse.fsa", FileInfo.Directory.FullName, FileInfo.Name)
                    Call FastaFile.Read(InputFasta).Complement.Save(OutputFasta)
                Catch ex As Exception
                    Call Console.WriteLine(ex.ToString)
                    Return -1
                End Try

                Return 0
            End If
        End If

        Return -1
    End Function

    <ExportAPI("-reverse", Usage:="-reverse -i <input_fasta> [-o <output_fasta>]")>
    Public Function Reverse(argvs As CommandLine) As Integer
        Dim InputFasta As String = argvs("-i")
        Dim OutputFasta As String = argvs("-o")

        If String.IsNullOrEmpty(InputFasta) Then
            Call Console.WriteLine("No fasta sequence was input!")
        ElseIf Not FileIO.FileSystem.FileExists(InputFasta) Then
            Call Console.WriteLine("Fasta file ""{0}"" is not exisist on your filesystem!", InputFasta)
        Else
            If String.IsNullOrEmpty(OutputFasta) Then
                Try
                    Dim FileInfo = FileIO.FileSystem.GetFileInfo(InputFasta)
                    OutputFasta = String.Format("{0}/{1}_reverse.fsa", FileInfo.Directory.FullName, FileInfo.Name)
                    Call FastaFile.Read(InputFasta).Reverse.Save(OutputFasta)
                Catch ex As Exception
                    Call Console.WriteLine(ex.ToString)
                    Return -1
                End Try

                Return 0
            End If
        End If

        Return -1
    End Function

    ''' <summary>
    ''' Using the regular expression to search the motif pattern on a target nucleotide sequence.(使用正则表达式搜索目标序列)
    ''' </summary>
    ''' <param name="argvs"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <ExportAPI("-pattern_search", Info:="Parsing the sequence segment from the sequence source using regular expression.",
        Usage:="-pattern_search -i <file_name> -p <regex_pattern>[ -o <output_directory> -f <format:fsa/gbk>]",
        Example:="-pattern_search -i ~/xcc8004.txt -p TTA{3}N{1,2} -f fsa")>
    <Reflection.ParameterInfo("-i",
        Description:="The sequence input data source file, it can be a fasta or genbank file.",
        Example:="~/Desktop/xcc8004.txt")>
    <Reflection.ParameterInfo("-p",
        Description:="This switch specific the regular expression pattern for search the sequence segment,\n" &
                     "for more detail information about the regular expression please read the user manual.",
        Example:="N{1,5}TA")>
    <Reflection.ParameterInfo("-o", True,
        Description:="Optional, this switch value specific the output directory for the result data, default is user Desktop folder.",
        Example:="~/Documents/")>
    <Reflection.ParameterInfo("-f", True,
        Description:="Optional, specific the input file format for the sequence reader, default value is FASTA sequence file.\n" &
                     " fsa - The input sequence data file is a FASTA format file;\n" &
                     " gbk - The input sequence data file is a NCBI genbank flat file.",
        Example:="fsa")>
    Public Function PatternSearchA(argvs As CommandLine) As Integer
        Dim Format As String = argvs("-f")
        Dim Input As String = argvs("-i")
        Dim OutputFolder As String = argvs("-o")
        Dim FASTA As FASTA.FastaFile
        Dim pattern As String = argvs("-p").Replace("N", "[ATGCU]")

        If String.IsNullOrEmpty(OutputFolder) Then
            OutputFolder = My.Computer.FileSystem.SpecialDirectories.Desktop
        End If

        If String.IsNullOrEmpty(Format) OrElse String.Equals("fsa", Format, StringComparison.OrdinalIgnoreCase) Then 'fasta sequence
            FASTA = FastaFile.Read(File:=Input)
        Else 'gbk format
            Dim GbkFile As GBFF.File = GBFF.File.Read(Path:=Input)
            FASTA = GbkFile.ExportProteins
        End If

        Dim File As String = IO.Path.GetFileNameWithoutExtension(Input)
        Dim Csv = SequenceTools.Pattern.PatternSearch.Match(Seq:=FASTA, pattern:=pattern)
        Dim Complement = SequenceTools.Pattern.PatternSearch.Match(Seq:=FASTA.Complement, pattern:=pattern)
        Dim Reverse = SequenceTools.Pattern.PatternSearch.Match(Seq:=FASTA.Reverse, pattern:=pattern)
        Dim ComplementReverse = SequenceTools.Pattern.PatternSearch.Match(Seq:=FASTA.Complement.Reverse, pattern:=pattern)

        Call Csv.Insert(rowId:=-1, Row:={"Match pattern:=", pattern})
        Call Complement.Insert(rowId:=-1, Row:={"Match pattern:=", pattern})
        Call Reverse.Insert(rowId:=-1, Row:={"Match pattern:=", pattern})
        Call ComplementReverse.Insert(rowId:=-1, Row:={"Match pattern:=", pattern})

        Call FileIO.FileSystem.CreateDirectory(OutputFolder)
        Call Csv.Save(OutputFolder & "/" & File & ".csv", False)
        Call Complement.Save(OutputFolder & "/" & File & "_complement.csv", False)
        Call Reverse.Save(OutputFolder & "/" & File & "_reversed.csv", False)
        Call ComplementReverse.Save(OutputFolder & "/" & File & "_complement_reversed.csv", False)

        Return 0
    End Function

    <ExportAPI("/logo", Usage:="/logo /in <clustal.fasta> [/out <out.png>]")>
    Public Function SequenceLogo(args As CommandLine) As Integer
        Dim [in] As String = args - "/in"
        Dim out As String = args.GetValue("/out", [in].TrimFileExt & ".logo.png")
        Dim fa As New FastaFile([in])
        Dim logo As Image = SequencePatterns.SequenceLogo.DrawFrequency(fa)
        Return logo.SaveAs(out, ImageFormats.Png)
    End Function

    <ExportAPI("--Drawing.ClustalW",
           Usage:="--Drawing.ClustalW /in <align.fasta> [/out <out.png> /dot.Size 10]")>
    Public Function DrawClustalW(args As CommandLine) As Integer
        Dim inFile As String = args("/in")
        Dim out As String = args.GetValue("/out", inFile.TrimFileExt & ".png")
        Dim aln As New FASTA.FastaFile(inFile)
        Call ClustalVisual.SetDotSize(args.GetValue("/dot.size", 10))
        Dim res As Image = ClustalVisual.InvokeDrawing(aln)
        Return res.SaveAs(out, ImageFormats.Png)
    End Function
End Module
