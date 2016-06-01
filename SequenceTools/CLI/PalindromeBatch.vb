Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Language.UnixBash
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Parallel

Partial Module Utilities

    <ExportAPI("/Palindrome.BatchTask",
               Usage:="/Palindrome.BatchTask /in <in.DIR> [/num_threads 4 /min 3 /max 20 /out <out.DIR>]")>
    Public Function PalindromeBatchTask(args As CommandLine.CommandLine) As Integer
        Dim inDIR As String = args - "/in"
        Dim min As Integer = args.GetValue("/min", 3)
        Dim max As Integer = args.GetValue("/max", 20)
        Dim out As String = args.GetValue("/out", inDIR.TrimDIR & $"-{min},{max}.Palindrome.Workflow/")
        Dim files As IEnumerable(Of String) = ls - l - r - wildcards("*.fasta", "*.fa", "*.fsa", "*.fna") <= inDIR
        Dim api As String = GetType(Utilities).API(NameOf(PalindromeWorkflow))
        Dim n As Integer = LQuerySchedule.AutoConfig(args.GetValue("/num_threads", 4))
        Dim task As Func(Of String, String) =
            Function(fa) $"{api} /in {fa.CliPath} /min {min} /max {max} /out {out.CliPath} /batch"
        Dim CLI As String() = files.ToArray(task)

        Return App.SelfFolks(CLI, parallel:=n)
    End Function

    <ExportAPI("/Palindrome.Workflow",
               Usage:="/Palindrome.Workflow /in <in.fasta> [/batch /min 3 /max 20 /out <out.DIR>]")>
    <ParameterInfo("/in", False,
                   Description:="This is a single sequence fasta file.")>
    Public Function PalindromeWorkflow(args As CommandLine.CommandLine) As Integer
        Dim [in] As String = args("/in")
        Dim min As Integer = args.GetValue("/min", 3)
        Dim max As Integer = args.GetValue("/max", 20)
        Dim out As String = args.GetValue("/out", [in].TrimFileExt & ".Palindrome.Workflow/")
        Dim isBatch As Boolean = args.GetBoolean("/batch") ' 批量和单独的模式相比，差异只是在保存结果的时候的位置

        If isBatch Then

        Else

        End If
    End Function
End Module
