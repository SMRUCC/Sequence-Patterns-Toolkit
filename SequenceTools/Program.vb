Imports System.Text.RegularExpressions
Imports LANS.SystemsBiology.SequenceModel.FASTA
Imports Microsoft.VisualBasic.DocumentFormat.Csv
Imports Microsoft.VisualBasic.DocumentFormat.Csv.StorageProvider.Reflection
Imports Microsoft.VisualBasic

Module Program

    Public Function Main() As Integer
        'Dim list = "X:\gi.csv".LoadCsv(Of pv)
        'Dim maps = "X:\xcc 8004 whole gene  and annotion .csv".LoadCsv(Of locusMap)

        'For Each x In list
        '    x.locus = Regex.Match(x.Desc, "XC_\d{4}", RegexOptions.IgnoreCase).Value
        '    If String.IsNullOrEmpty(x.locus) Then
        '        Dim LQuery = (From g In maps Where String.Equals(x.ID, g.PID) Select g).FirstOrDefault
        '        If Not LQuery Is Nothing Then
        '            x.locus = LQuery.Synonym

        '        End If
        '    End If
        'Next

        'Call list.SaveTo("x:\dddd.csv")

        Dim a = {1, 2, 3, 4, 5, 6, 7, 8, 9, 0}
        Dim b = {1, 4, 7, 44, 57, -2, 9, 9, 0, 0, 20}

        Dim i = a.Intersection(b, AddressOf s)

        Return GetType(Utilities).RunCLI(App.CommandLine)
    End Function

    Private Function s(n As Integer) As String
        Return CStr(n)
    End Function
End Module

Public Class locusMap
    Public Property PID As String
    Public Property Synonym As String

End Class

Public Class pv
    Public Property locus As String
    <Column("Protein ID")> Public Property ID As String
    <Column("Protein Desc")> Public Property Desc As String
    <Column("Protein Score")> Public Property Score As String
    <Column("Protein Mass")> Public Property Mass As String
    <Column("Protein PI")> Public Property PI As String

    <Column("hfg_1/8004_1")> Public Property hfg_18004_1 As String
    Public Property pValue1 As String
    <Column("hfg_2/8004_2")> Public Property hfg_28004_2 As String
    Public Property pValue2 As String
    <Column("hfg_3/8004_3")> Public Property hfg_38004_3 As String
    Public Property pValue3 As String
    <Column("hfg/8004")> Public Property hfg8004 As String
    Public Property pValue4 As String

End Class