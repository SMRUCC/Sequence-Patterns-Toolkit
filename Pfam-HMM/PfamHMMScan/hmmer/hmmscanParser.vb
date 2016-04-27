Imports System.Runtime.CompilerServices
Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic.Text
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Linq

Public Module hmmscanParser

    Public Function LoadDoc(path As String) As hmmscan
        Dim buf As String() = IO.File.ReadAllLines(path)
        Dim i As Integer
        Dim head As String() = buf.ReadHead(i, AddressOf UntilBlank)
        Dim blocks As IEnumerable(Of String()) = buf.Skip(i).Split("//")

        Return New hmmscan With {
            .version = head(1),
            .Querys = QueryParser(blocks).ToArray,
            .query = Mid(head(5), 22).Trim,
            .HMM = Mid(head(6), 22).Trim
        }
    End Function

    Private Iterator Function QueryParser(source As IEnumerable(Of String())) As IEnumerable(Of Query)
        For Each block As String() In source
            Yield block.QueryParser
        Next
    End Function

    Const inclusion As String = "------ inclusion threshold ------"
    Const NoHits As String = "[No hits detected that satisfy reporting thresholds]"

    <Extension>
    Private Function QueryParser(buf As String()) As Query
        Dim query As String = buf(Scan0)
        Dim len As Integer = Regex.Match(query, "L=\d+", RegexICSng).Value.Split("="c).Last

        If buf.Lookup(NoHits) <> -1 Then
            Return New Query With {
                .name = Mid(query, 7).Trim,
                .length = len
            }
        End If

        Dim fields As Integer() = buf(4).CrossFields
        Dim hits As New List(Of Hit)
        Dim offset As Integer = 5
        Dim s As String = ""

        Do While Not buf.Read(offset).ShadowCopy(s).IsBlank AndAlso
            InStr(s, inclusion) = 0
            hits += s.HitParser(fields)
        Loop

        Dim uhits As New List(Of Hit)

        Do While Not buf.Read(offset).ShadowCopy(s).IsBlank
            uhits += s.HitParser(fields)
        Loop

        Return New Query With {
            .name = Mid(query, 7).Trim,
            .length = len,
            .Hits = hits.ToArray,
            .uncertain = uhits.ToArray
        }
    End Function

    <Extension>
    Private Function HitParser(line As String, fields As Integer()) As Hit
        Dim buf As String() = line.FieldParser(fields)
        Dim s1 As New Score(buf(1), buf(3), buf(5))
        Dim s2 As New Score(buf(7), buf(9), buf(11))
        Dim model As String = Trim(buf(17))
        Dim describ As String = Trim(buf(19))
        Dim N As Integer = CInt(Val(Trim(buf(15))))
        Dim exp As Double = Val(Trim(buf(13)))

        Return New Hit With {
            .Full = s1,
            .Best = s2,
            .exp = exp,
            .N = N,
            .Model = model,
            .Description = describ
        }
    End Function
End Module
