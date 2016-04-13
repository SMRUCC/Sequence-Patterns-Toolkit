Imports Microsoft.VisualBasic.Linq

Public Class Stockholm

    ''' <summary>
    ''' Identifier
    ''' </summary>
    ''' <returns></returns>
    Public Property ID As String
    ''' <summary>
    ''' Pfam accession ID
    ''' </summary>
    ''' <returns></returns>
    Public Property AC As String
    ''' <summary>
    ''' Definition
    ''' </summary>
    ''' <returns></returns>
    Public Property DE As String
    Public Property GA As Double()
    ''' <summary>
    ''' Type
    ''' </summary>
    ''' <returns></returns>
    Public Property TP As String
    Public Property ML As Integer
    Public Property CL As String
    Public Property NE As String

    Public Shared Iterator Function DocParser(path As String) As IEnumerable(Of Stockholm)
        Dim lines As String() = path.ReadAllLines
        Dim tokens As IEnumerable(Of String()) = lines.Split("//")

        VBDebugger.Mute = True

        For Each token As String() In tokens
            Dim hash As Dictionary(Of String, String) = __hash(token)
            Dim x As New Stockholm

            x.AC = hash.TryGetValue(NameOf(x.AC))
            x.CL = hash.TryGetValue(NameOf(x.CL))
            x.DE = hash.TryGetValue(NameOf(x.DE))

            Dim tmp As String = hash.TryGetValue(NameOf(x.GA))
            If Not String.IsNullOrEmpty(tmp) Then
                x.GA = Strings.Split(tmp, ";").ToArray(Function(s) Val(s), where:=Function(s) Not s.IsBlank)
            End If

            x.ID = hash.TryGetValue(NameOf(x.ID))
            x.ML = Scripting.CTypeDynamic(Of Integer)(hash.TryGetValue(NameOf(x.ML)))
            x.NE = hash.TryGetValue(NameOf(x.NE))
            x.TP = hash.TryGetValue(NameOf(x.TP))

            Yield x
        Next

        VBDebugger.Mute = False
    End Function

    Private Shared Function __hash(token As String()) As Dictionary(Of String, String)

    End Function
End Class
