Imports Microsoft.VisualBasic

Namespace Topologically

    Public Module Seeds

        Public Function ExtendSequence(Source As List(Of String), Chars As Char()) As List(Of String)
            Return (From str As String
                    In Source.AsParallel
                    Select Seeds.Combo(str, Chars)).ToArray.MatrixToList
        End Function

        ''' <summary>
        ''' Initialize the nucleotide repeats seeds.(初始化序列片段的搜索种子)
        ''' </summary>
        ''' <param name="chars"></param>
        ''' <param name="Length"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function InitializeSeeds(chars As Char(), Length As Integer) As List(Of String)
            Dim ChunkTemp As List(Of String) =
                New List(Of String) From {""}

            For i As Integer = 1 To Length
                ChunkTemp = ExtendSequence(ChunkTemp, chars)
            Next

            Return (From s As String
                    In ChunkTemp
                    Select s
                    Order By s Descending).ToList
        End Function

        Public Function InitializeSeeds(chars As Char(), length As Integer, sequence As String) As String()
            Dim ChunkTemp As List(Of String) = InitializeSeeds(chars, length)
            Dim LQuery = (From seed As String In ChunkTemp.AsParallel Where InStr(sequence, seed, CompareMethod.Text) > 0 Select seed).ToArray
            Return LQuery
        End Function

        Public Function Combo(Sequence As String, Chars As Char()) As String()
            Dim ChunkList = (From ch As Char
                             In Chars
                             Select Sequence & ch.ToString).ToArray
            Return ChunkList
        End Function
    End Module
End Namespace