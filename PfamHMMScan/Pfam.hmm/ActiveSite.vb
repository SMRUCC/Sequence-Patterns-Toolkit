Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Microsoft.VisualBasic.Serialization

''' <summary>
''' active_site.dat
''' </summary>
Public Structure ActiveSite : Implements sIdEnumerable

    Public Property ID As String Implements sIdEnumerable.Identifier
    Public Property RE As Dictionary(Of String, RE)
    Public Property AL As Alignment()

    Public Overrides Function ToString() As String
        Return Me.GetJson
    End Function

    Public Shared Iterator Function LoadStream(path As String) As IEnumerable(Of ActiveSite)
        Dim lines As String() = path.ReadAllLines
        Dim tokens As IEnumerable(Of String()) = lines.Split("//")

        For Each token As String() In tokens
            Yield StreamParser(token)
        Next
    End Function

    Public Shared Function StreamParser(stream As String()) As ActiveSite

    End Function
End Structure

Public Structure RE : Implements sIdEnumerable
    Public Property ID As String Implements sIdEnumerable.Identifier
    Public Property Value As Integer()

    Public Overrides Function ToString() As String
        Return Me.GetJson
    End Function
End Structure

Public Structure Alignment : Implements sIdEnumerable
    Public Property ID As String Implements sIdEnumerable.Identifier
    Public Property MAL As String

    Public Overrides Function ToString() As String
        Return Me.GetJson
    End Function
End Structure