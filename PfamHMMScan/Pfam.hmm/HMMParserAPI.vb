Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Linq

Public Module HMMParserAPI

    Public Iterator Function LoadDoc(path As String) As IEnumerable(Of HMMParser)
        Dim reader As BufferedStream = New BufferedStream(path, maxBufferSize:=1024 * 1024 * 128)
        Dim last As List(Of String) = New List(Of String)

        Do While Not reader.EndRead
            Dim lines As String() = reader.BufferProvider
            Dim blocks As String()() = lines.Split("//").ToArray

            blocks(Scan0) = last + blocks(Scan0)
            last = blocks.Last.ToList

            For Each block As String() In blocks.Take(blocks.Length - 1)
                Yield StreamParser(block)
            Next
        Loop
    End Function

    Public Function StreamParser(stream As String()) As HMMParser
        Dim fields As New Dictionary(Of String, String)
        Dim i As Integer
        Dim pos As Integer
        Dim s As String
        Dim key As String

        For i = 1 To stream.Length - 1
            s = stream(i)
            pos = InStr(s, " ")
            key = s.Substring(0, pos - 1)

            If String.Equals(key, "STATS") Then
                Exit For
            End If

            s = s.Substring(pos).Trim
            fields += New HashValue With {
                .Identifier = key,
                .value = s
            }
        Next

        Dim stats As STATS = STATSParser(stream(i), stream(i + 1), stream(i + 2))
        i += 3
        i += 2

        Dim blocks As String()() = stream.Skip(i).Split(3)

        Return New HMMParser With {
            .STATS = stats,
            .COMPO = New COMPO With {
                .Nodes = blocks.ToArray(Function(block) NodeParser(block))
            }
        }
    End Function

    Public Function NodeParser(block As String()) As Node
        Dim m As String = block(0)
        Dim i As String = block(1)
        Dim s As String = block(2)
    End Function

    Public Function STATSParser(msv As String, viterbi As String, forwards As String) As STATS
        msv = msv.Substring(16).Trim
        viterbi = viterbi.Substring(21).Trim
        forwards = forwards.Substring(21).Trim

        Return New STATS With {
            .MSV = msv.Split.ToArray(Function(s) Val(s)),
            .VITERBI = viterbi.Split.ToArray(Function(s) Val(s)),
            .FORWARD = forwards.Split.ToArray(Function(s) Val(s))
        }
    End Function
End Module
