Imports System.Data.Linq.Mapping
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Serialization

Namespace hmmscan

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
        Public Property Alignments As Alignment()

        Public Overrides Function ToString() As String
            Return $"{name}  [L={length}]"
        End Function

        Public Function GetTable() As ScanTable()
            Return Hits.ToList(Function(x) New ScanTable(Me, x, "!"c)) +
            From x As Hit
            In uncertain.SafeQuery
            Select New ScanTable(Me, x, "?"c)
        End Function
    End Class

    Public Class ScanTable

        ''' <summary>
        ''' !?
        ''' </summary>
        ''' <returns></returns>
        Public Property rank As Char
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

        Sub New(query As Query, hit As Hit, ranks As Char)
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
            rank = ranks
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

    Public Class Alignment

        Public Property model As String
        Public Property describ As String
        Public Property Aligns As Align()

        Public Overrides Function ToString() As String
            Return $"{model}  {describ}"
        End Function
    End Class

    Public Class Align

        <Column(Name:="#")> Public Property rank As String
        Public Property score As Double
        Public Property bias As Double
        <Column(Name:="c-Evalue")> Public Property cEvalue As Double
        <Column(Name:="i-Evalue")> Public Property iEvalue As Double
        Public Property hmmfrom As Integer
        <Column(Name:="hmm To")> Public Property hmmTo As Integer
        Public Property alifrom As Integer
        <Column(Name:="ali To")> Public Property aliTo As Integer
        Public Property envfrom As Integer
        <Column(Name:="env To")> Public Property envTo As Integer
        Public Property acc As Double

        Friend Sub New(buf As String())
            rank = (buf(1) & buf(2)).Trim
            score = Val(buf(3).Trim)
            bias = Val(buf(5).Trim)
            cEvalue = Val(buf(7).Trim)
            iEvalue = Val(buf(9).Trim)
            hmmfrom = CInt(Val(buf(11).Trim))
            hmmTo = CInt(Val(buf(13).Trim))
            alifrom = CInt(Val(buf(15).Trim))
            aliTo = CInt(Val(buf(17).Trim))
            envfrom = CInt(Val(buf(19).Trim))
            envTo = CInt(Val(buf(21).Trim))
            acc = Val(buf(23).Trim)
        End Sub

        Sub New()
        End Sub

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function
    End Class
End Namespace