Imports System.Runtime.CompilerServices
Imports LANS.SystemsBiology.AnalysisTools
Imports LANS.SystemsBiology.SequenceModel
Imports LANS.SystemsBiology.SequenceModel.Patterns

Public Module SNPScan

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nt">
    ''' The file path of the input nt fasta sequence file.
    ''' </param>
    ''' <returns></returns>
    Public Function ScanRaw(nt As String) As SNP()
        Return ScanRaw(New FASTA.FastaFile(nt))
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nt">可以不经过任何处理，程序在这里会自动使用clustal进行对齐操作</param>
    ''' <returns></returns>
    <Extension>
    Public Function ScanRaw(nt As FASTA.FastaFile) As SNP()
        Dim clustal As ClustalOrg.Clustal =
            ClustalOrg.Clustal.CreateSession
        nt = clustal.Align(nt)
        Return nt.Scan(ref:=Scan0)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nt">序列必须都是已经经过clustal对齐了的</param>
    ''' <returns></returns>
    <Extension>
    Public Function Scan(nt As FASTA.FastaFile, ref As Integer) As SNP()
        Dim pwm As PatternModel = PatternsAPI.Frequency(nt)
    End Function
End Module
