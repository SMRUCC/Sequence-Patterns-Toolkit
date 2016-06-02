Imports System.Runtime.CompilerServices
Imports LANS.SystemsBiology.AnalysisTools
Imports LANS.SystemsBiology.SequenceModel
Imports LANS.SystemsBiology.SequenceModel.Patterns

Public Module SNPScan

    ReadOnly clustal As ClustalOrg.Clustal =
        ClustalOrg.Clustal.CreateSession

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nt">
    ''' The file path of the input nt fasta sequence file.
    ''' </param>
    ''' <returns></returns>
    Public Function ScanRaw(nt As String) As SNP()
        Return __scanRaw(nt)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nt">可以不经过任何处理，程序在这里会自动使用clustal进行对齐操作</param>
    ''' <returns></returns>
    <Extension>
    Public Function ScanRaw(nt As FASTA.FastaFile) As SNP()
        Dim tmp As String = App.GetAppSysTempFile(".fasta")
        Call nt.Save(tmp, Encodings.ASCII)
        Return __scanRaw(tmp)
    End Function

    Private Function __scanRaw([in] As String) As SNP()
        Dim nt As FASTA.FastaFile = clustal.MultipleAlignment([in])
        Return nt.Scan(refInd:=Scan0)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nt">序列必须都是已经经过clustal对齐了的</param>
    ''' <returns></returns>
    <Extension>
    Public Function Scan(nt As FASTA.FastaFile, refInd As Integer) As SNP()
        Dim pwm As PatternModel = PatternsAPI.Frequency(nt)
        Dim ref As FASTA.FastaToken = nt(refInd)
        Dim var As Double() = pwm.GetVariation(ref, 0.2)

    End Function
End Module
