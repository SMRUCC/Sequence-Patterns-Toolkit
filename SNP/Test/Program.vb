Imports LANS.SystemsBiology.AnalysisTools.SequenceTools.SNP
Imports LANS.SystemsBiology.SequenceModel.FASTA
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Serialization

Module Program

    Sub Main()




        Dim ss = "%s+%s+%s".xFormat <= {"sd", "98", "00"}

        Call New FastaFile("F:\Sequence-Patterns-Toolkit\data\SNP\test.txt").ScanSNPs(0).GetJson.SaveTo("F:\Sequence-Patterns-Toolkit\data\SNP\test.args.json")


        Dim result = SNPScan.ScanRaw("F:\Sequence-Patterns-Toolkit\data\SNP\LexA.fasta")
    End Sub
End Module
