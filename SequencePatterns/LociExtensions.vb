Imports System.Runtime.CompilerServices
Imports LANS.SystemsBiology.AnalysisTools.SequenceTools.SequencePatterns.Topologically
Imports LANS.SystemsBiology.SequenceModel.NucleotideModels
Imports Microsoft.VisualBasic.Linq

''' <summary>
''' 将序列特征的搜索结果转换为<see cref="SimpleSegment"/>对象类型
''' </summary>
Public Module LociExtensions

    <Extension>
    Public Function ToLoci(x As PalindromeLoci) As SimpleSegment

    End Function

    <Extension>
    Public Function ToLoci(x As Topologically.Repeats) As SimpleSegment

    End Function

    <Extension>
    Public Function ToLoci(x As RevRepeats) As SimpleSegment

    End Function

    <Extension>
    Public Function ToLoci(x As Topologically.ImperfectPalindrome) As SimpleSegment

    End Function

    <Extension>
    Public Function ToLocis(x As IEnumerable(Of PalindromeLoci)) As SimpleSegment()
        Return x.ToArray(AddressOf ToLoci)
    End Function

    <Extension>
    Public Function ToLocis(x As IEnumerable(Of Topologically.Repeats)) As SimpleSegment()
        Return x.ToArray(AddressOf ToLoci)
    End Function

    <Extension>
    Public Function ToLocis(x As IEnumerable(Of RevRepeats)) As SimpleSegment()
        Return x.ToArray(AddressOf ToLoci)
    End Function

    <Extension>
    Public Function ToLocis(x As IEnumerable(Of Topologically.ImperfectPalindrome)) As SimpleSegment()
        Return x.ToArray(AddressOf ToLoci)
    End Function
End Module
