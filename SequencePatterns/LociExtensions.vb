Imports System.Runtime.CompilerServices
Imports LANS.SystemsBiology.AnalysisTools.SequenceTools.SequencePatterns.Topologically
Imports LANS.SystemsBiology.SequenceModel.NucleotideModels
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq

''' <summary>
''' 将序列特征的搜索结果转换为<see cref="SimpleSegment"/>对象类型
''' </summary>
Public Module LociExtensions

    <Extension>
    Public Function ToLoci(x As PalindromeLoci) As SimpleSegment
        Return New SimpleSegment With {
            .Strand = x.MappingLocation.Strand.GetBriefCode,
            .Start = x.MappingLocation.Left,
            .Ends = x.MappingLocation.Right,
            .SequenceData = x.Palindrome
        }
    End Function

    ''' <summary>
    ''' 对于简单的重复序列而言，正向链上面的重复片段，例如AAGTCT在反向链上面就是AGACTT，总是可以找得到对应的，所以在这里只需要记录下正向链的数据就好了
    ''' </summary>
    ''' <param name="x"></param>
    ''' <param name="start"></param>
    ''' <returns></returns>
    <Extension>
    Public Function ToLoci(x As Topologically.Repeats, start As Integer) As SimpleSegment
        Return New SimpleSegment With {
            .Start = start,
            .Ends = start + x.Length,
            .SequenceData = x.SequenceData,
            .Strand = "+"
        }
    End Function

    <Extension>
    Public Function ToLoci(x As RevRepeats, start As Integer) As SimpleSegment
        Return New SimpleSegment With {
            .Start = start,
            .Ends = start + x.Length,
            .SequenceData = x.SequenceData,
            .Strand = "+"
        }
    End Function

    <Extension>
    Public Function ToLoci(x As Topologically.ImperfectPalindrome) As SimpleSegment
        Return New SimpleSegment With {
            .Start = x.MappingLocation.Left,
            .Ends = x.MappingLocation.Right,
            .Strand = x.MappingLocation.Strand.GetBriefCode,
            .SequenceData = x.Palindrome
        }
    End Function

    <Extension>
    Public Function ToLocis(x As IEnumerable(Of PalindromeLoci)) As SimpleSegment()
        Return x.ToArray(AddressOf ToLoci)
    End Function

    <Extension>
    Public Function ToLocis(x As IEnumerable(Of Topologically.Repeats)) As SimpleSegment()
        Return LinqAPI.Exec(Of SimpleSegment) <=
            From loci As Repeats
            In x
            Select From n As Integer
                   In loci.Locations
                   Select loci.ToLoci(n)
    End Function

    <Extension>
    Public Function ToLocis(x As IEnumerable(Of RevRepeats)) As SimpleSegment()
        Return LinqAPI.Exec(Of SimpleSegment) <=
            From loci As RevRepeats
            In x
            Select From n As Integer
                   In loci.Locations
                   Select loci.ToLoci(n)
    End Function

    <Extension>
    Public Function ToLocis(x As IEnumerable(Of Topologically.ImperfectPalindrome)) As SimpleSegment()
        Return x.ToArray(AddressOf ToLoci)
    End Function
End Module
