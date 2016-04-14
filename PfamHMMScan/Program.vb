Module Program

    Public Function Main() As Integer

        Dim ddd = HMMParserAPI.LoadDoc("G:\GCModeller\PfamFamily\Pfam-A.hmm\Pfam-A.hmm").ToArray
        Dim sss = Stockholm.DocParser("G:\GCModeller\PfamFamily\Pfam-A.hmm\Pfam-A.hmm.dat").ToArray
        Dim gggg = ActiveSite.LoadStream("G:\GCModeller\PfamFamily\Pfam-A.hmm\active_site.dat").ToArray

        Return GetType(CLI).RunCLI(App.CommandLine)
    End Function
End Module
