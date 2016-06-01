Imports LANS.SystemsBiology.AnalysisTools.SequenceTools.SequencePatterns
Imports LANS.SystemsBiology.AnalysisTools.SequenceTools.SequencePatterns.Topologically
Imports LANS.SystemsBiology.SequenceModel
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Language.UnixBash
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Parallel

Partial Module Utilities

    <ExportAPI("/Palindrome.BatchTask",
               Usage:="/Palindrome.BatchTask /in <in.DIR> [/num_threads 4 /min 3 /max 20 /min-appears 2 /cutoff <0.6> /max-dist <1000 (bp)> /partitions <-1> /out <out.DIR>]")>
    Public Function PalindromeBatchTask(args As CommandLine.CommandLine) As Integer
        Dim inDIR As String = args - "/in"
        Dim min As Integer = args.GetValue("/min", 3)
        Dim max As Integer = args.GetValue("/max", 20)
        Dim out As String = args.GetValue("/out", inDIR.TrimDIR & $"-{min},{max}.Palindrome.Workflow/")
        Dim files As IEnumerable(Of String) = ls - l - r - wildcards("*.fasta", "*.fa", "*.fsa", "*.fna") <= inDIR
        Dim api As String = GetType(Utilities).API(NameOf(PalindromeWorkflow))
        Dim n As Integer = LQuerySchedule.AutoConfig(args.GetValue("/num_threads", 4))
        Dim cutoff As Double = args.GetValue("/cutoff", 0.6)
        Dim maxDist As Integer = args.GetValue("/max-dist", 1000)
        Dim parts As Integer = args.GetValue("/partitions", -1)
        Dim minAp As Integer = args.GetValue("/min-appears", 2)
        Dim task As Func(Of String, String) =
            Function(fa) _
                $"{api} /in {fa.CliPath} /min {min} /max {max} /min-appears {minAp} /out {out.CliPath} /cutoff {cutoff} /max-dist {maxDist} /partitions {parts} /batch"
        Dim CLI As String() = files.ToArray(task)

        Return App.SelfFolks(CLI, parallel:=n)
    End Function

    <ExportAPI("/Palindrome.Workflow",
               Usage:="/Palindrome.Workflow /in <in.fasta> [/batch /min-appears 2 /min 3 /max 20 /cutoff <0.6> /max-dist <1000 (bp)> /partitions <-1> /out <out.DIR>]")>
    <ParameterInfo("/in", False,
                   Description:="This is a single sequence fasta file.")>
    Public Function PalindromeWorkflow(args As CommandLine.CommandLine) As Integer
        Dim [in] As String = args("/in")
        Dim min As Integer = args.GetValue("/min", 3)
        Dim max As Integer = args.GetValue("/max", 20)
        Dim out As String = args.GetValue("/out", [in].TrimFileExt & ".Palindrome.Workflow/")
        Dim isBatch As Boolean = args.GetBoolean("/batch") ' 批量和单独的模式相比，差异只是在保存结果的时候的位置
        Dim nt As New FASTA.FastaToken([in])
        Dim minAp As Integer = args.GetValue("/min-appears", 2)

        Dim mirrorPalindrome = Topologically.SearchMirror(nt, min, max)   ' 镜像回文
        Dim repeats = RepeatsSearchAPI.SearchRepeats(nt, min, max, minAp)
        Dim rev = RepeatsSearchAPI.SearchReversedRepeats(nt, min, max, minAp)

        Dim repeatsViews = RepeatsView.TrimView(Topologically.Repeats.CreateDocument(repeats)).Trim(min, max, minAp)  ' 简单重复
        Dim revViews = RevRepeatsView.TrimView(rev).Trim(min, max, minAp)     ' 反向重复

        Dim palindrome = Topologically.SearchPalindrome(nt, min, max)  ' 简单回文

        Dim cutoff As Double = args.GetValue("/cutoff", 0.6)
        Dim maxDist As Integer = args.GetValue("/max-dist", 1000)
        Dim parts As Integer = args.GetValue("/partitions", -1)
        Dim imPalSearch As New Topologically.Imperfect(nt, min, max, cutoff, maxDist, parts)
        Call imPalSearch.InvokeSearch()
        Dim imperfectPalindrome = imPalSearch.ResultSet   ' 非完全回文

        If isBatch Then

        Else

        End If
    End Function
End Module
