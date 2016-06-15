Imports LANS.SystemsBiology.AnalysisTools.SequenceTools.SNP
Imports LANS.SystemsBiology.SequenceModel.FASTA
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection

Partial Module Utilities

    <ExportAPI("/SNP",
               Usage:="/SNP /in <nt.fasta> [/ref 0 /pure /monomorphic]")>
    Public Function SNP(args As CommandLine) As Integer
        Dim [in] As String = args - "/in"
        Dim pure As Boolean = args.GetBoolean("/pure")
        Dim monomorphic As Boolean = args.GetBoolean("/monomorphic")
        Dim nt As New FastaFile([in])
        Dim ref As Integer = args.GetInt32("/ref")

        Call nt.ScanSNPs(ref, pure, monomorphic)

        Return 0
    End Function
End Module
