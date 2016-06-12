Imports LANS.SystemsBiology.AnalysisTools.SequenceTools.SequencePatterns.Topologically
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Linq

Namespace Topologically.SimilarityMatches

    ''' <summary>
    ''' 模糊相等的
    ''' </summary>
    Public Module MirrorPalindrome

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="l">目标片段的长度</param>
        ''' <param name="Loci">位点的位置</param>
        ''' <param name="Mirror">进行模糊比较的参考序列</param>
        ''' <param name="Sequence">基因组序列</param>
        ''' <returns></returns>
        Private Function __haveMirror(l As Integer, Loci As Integer, Mirror As String, Sequence As String, cut As Double, maxDist As Integer) As Integer
            Dim mrStart As Integer = Loci + l  ' 左端的起始位置
            Dim ref As Integer() = Mirror.ToArray(AddressOf Asc)

            For i As Integer = 0 To maxDist
                Dim mMirr As String = Mid(Sequence, mrStart, l)
                Dim score As Double = LevenshteinDistance.ComputeDistance(ref, mMirr)?.MatchSimilarity

                If score >= cut Then
                    Return mrStart + l  ' 右端的结束位置
                Else
                    l += 1
                End If
            Next

            Return -1
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Segment"></param>
        ''' <param name="Sequence"></param>
        ''' <param name="maxDist">两个片段之间的最大的距离</param>
        ''' <param name="cut"></param>
        ''' <returns></returns>
        <ExportAPI("Mirrors.Locis.Get")>
        Public Function CreateMirrors(Segment As String, Sequence As String, maxDist As Integer, Optional cut As Double = 0.6) As PalindromeLoci()
            Dim Locations As Integer() = Pattern.Extensions.FindLocation(Sequence, Segment)

            If Locations.IsNullOrEmpty Then
                Return Nothing
            End If

            Dim Mirror As String = New String(Segment.Reverse.ToArray)  ' 这个是目标片段的镜像回文部分，也是需要进行比较的参考序列
            Dim l As Integer = Len(Segment)
            Dim Result = (From loci As Integer
                          In Locations
                          Let ml As Integer = __haveMirror(l, loci, Mirror, Sequence, cut, maxDist)
                          Where ml > -1
                          Select loci,
                              ml).ToArray
            Return Result.ToArray(
                Function(site) New PalindromeLoci With {
                    .Loci = Segment,
                    .Start = site.loci,
                    .PalEnd = site.ml,
                    .Palindrome = Mirror,
                    .MirrorSite = Mirror
                })
        End Function
    End Module
End Namespace