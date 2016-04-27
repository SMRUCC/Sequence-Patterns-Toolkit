Imports System.Data.Linq.Mapping
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Serialization

''' <summary>
''' hmmscan :: search sequence(s) against a profile database
''' </summary>
Public Class hmmscan

    ''' <summary>
    ''' HMMER 3.1b1 (May 2013); http://hmmer.org/
    ''' </summary>
    ''' <returns></returns>
    Public Property version As String
    ''' <summary>
    ''' query sequence file
    ''' </summary>
    ''' <returns></returns>
    Public Property query As String
    ''' <summary>
    ''' target HMM database
    ''' </summary>
    ''' <returns></returns>
    Public Property HMM As String
    Public Property Querys As Query()

    Public Overrides Function ToString() As String
        Return New With {version, query, HMM}.GetJson
    End Function

    Public Function GetTable() As ScanTable()
        Return LinqAPI.Exec(Of ScanTable) <= From query As Query
                                             In Querys
                                             Select result = query.GetTable
    End Function
End Class

''' <summary>
''' Scores for complete sequence (score includes all domains)
''' </summary>
Public Class Query : Inherits ClassObject

    Public Property name As String
    Public Property length As Integer
    Public Property Hits As Hit()
    ''' <summary>
    ''' ------ inclusion threshold ------
    ''' </summary>
    ''' <returns></returns>
    Public Property uncertain As Hit()

    Public Overrides Function ToString() As String
        Return $"{name}  [L={length}]"
    End Function

    Public Function GetTable() As ScanTable()
        Return Hits.ToList(Function(x) New ScanTable(Me, x)) + From x As Hit In uncertain Select New ScanTable(Me, x)
    End Function
End Class

Public Class ScanTable
    Public Property name As String
    Public Property len As Integer
    <Column(Name:="full.E-value")> Public Property FullEvalue As Double
    <Column(Name:="full.score")> Public Property FullScore As Double
    <Column(Name:="full.bias")> Public Property FullBias As Double
    <Column(Name:="best.E-value")> Public Property BestEvalue As Double
    <Column(Name:="best.score")> Public Property BestScore As Double
    <Column(Name:="best.bias")> Public Property BestBias As Double
    Public Property exp As Double
    Public Property N As Integer
    Public Property model As String
    Public Property describ As String

    Sub New()
    End Sub

    Sub New(query As Query, hit As Hit)
        name = query.name
        len = query.length
        FullEvalue = hit.Full.Evalue
        FullScore = hit.Full.score
        FullBias = hit.Full.bias
        BestBias = hit.Best.bias
        BestEvalue = hit.Best.Evalue
        BestScore = hit.Best.score
        exp = hit.exp
        N = hit.N
        model = hit.Model
        describ = hit.Description
    End Sub

    Public Overrides Function ToString() As String
        Return Me.GetJson
    End Function
End Class

Public Class Hit

    ''' <summary>
    ''' --- full sequence ---
    ''' </summary>
    ''' <returns></returns>
    Public Property Full As Score
    ''' <summary>
    ''' --- best 1 domain ---
    ''' </summary>
    ''' <returns></returns>
    Public Property Best As Score

#Region "-#dom-"

    Public Property exp As Double
    Public Property N As Integer
#End Region

    Public Property Model As String
    Public Property Description As String

    Public Overrides Function ToString() As String
        Return Me.GetJson
    End Function
End Class

Public Structure Score

    <Column(Name:="E-value")> Public Property Evalue As Double
    Public Property score As Double
    Public Property bias As Double

    Sub New(buf As String())
        Call Me.New(buf(0), buf(1), buf(2))
    End Sub

    Sub New(e As String, s As String, b As String)
        Evalue = Val(e.Trim)
        score = Val(s.Trim)
        bias = Val(b.Trim)
    End Sub

    Public Overrides Function ToString() As String
        Return Me.GetJson
    End Function
End Structure