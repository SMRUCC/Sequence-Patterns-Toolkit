Module Program

    Public Function Main() As Integer
        Dim sss = Stockholm.DocParser("G:\GCModeller\PfamFamily\Pfam-A.hmm\Pfam-A.hmm.dat").ToArray

        Return GetType(CLI).RunCLI(App.CommandLine)
    End Function
End Module
