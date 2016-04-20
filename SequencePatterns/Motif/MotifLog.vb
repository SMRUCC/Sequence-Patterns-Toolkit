Imports System.Web.Script.Serialization
Imports LANS.SystemsBiology.SequenceModel.NucleotideModels
Imports Microsoft.VisualBasic.DocumentFormat.Csv.StorageProvider.Reflection

''' <summary>
''' 简单的Motif位点
''' </summary>
Public Class MotifLog : Inherits SimpleSegment
    Public Property Family As String
    Public Property BiologicalProcess As String
    Public Property Regulog As String
    Public Property Taxonomy As String
    Public Property ATGDist As Integer
    Public Property Location As String

    ''' <summary>
    ''' 当前的这个位点对象是否是在启动子区
    ''' </summary>
    ''' <returns></returns>
    <ScriptIgnore>
    <Ignored>
    Public ReadOnly Property InPromoterRegion As Boolean
        Get
            If Location Is Nothing Then
                Return False
            End If
            Return InStr(Location, "promoter", CompareMethod.Text) > 0
        End Get
    End Property

    Sub New()
    End Sub

    Sub New(loci As MotifLog)
        Call MyBase.New(loci)

        Family = loci.Family
        BiologicalProcess = loci.BiologicalProcess
        Regulog = loci.Regulog
        Taxonomy = loci.Taxonomy
        ATGDist = loci.ATGDist
        Location = loci.Location
    End Sub

    Sub New(loci As SimpleSegment)
        Call MyBase.New(loci)
    End Sub
End Class