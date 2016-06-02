Imports System.Runtime.CompilerServices
Imports LANS.SystemsBiology.SequenceModel

Public Module SNPScan

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nt">可以不经过任何处理，程序在这里会自动使用clustal进行对齐操作</param>
    ''' <returns></returns>
    <Extension>
    Public Function ScanRaw(nt As FASTA.FastaFile) As SNP()

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nt">序列必须都是已经经过clustal对齐了的</param>
    ''' <returns></returns>
    <Extension>
    Public Function Scan(nt As FASTA.FastaFile) As SNP()

    End Function
End Module
