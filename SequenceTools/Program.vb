Imports System.Text.RegularExpressions
Imports LANS.SystemsBiology.SequenceModel.FASTA
Imports Microsoft.VisualBasic.DocumentFormat.Csv
Imports Microsoft.VisualBasic.DocumentFormat.Csv.StorageProvider.Reflection
Imports Microsoft.VisualBasic

Module Program

    Public Function Main() As Integer
        Return GetType(Utilities).RunCLI(App.CommandLine)
    End Function
End Module