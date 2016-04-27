Imports System.Data.Linq.Mapping
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Serialization

Public Class hmmscan

    Public Property version As String
    Public Property query As String
    Public Property HMM As String
    Public Property Querys As Query()

    Public Overrides Function ToString() As String
        Return New With {version, query, HMM}.GetJson
    End Function
End Class

Public Class Query : Inherits ClassObject

    Public Property name As String
    Public Property length As Integer
    Public Property Hits As Hit()
    Public Property uncertain As Hit()

    Public Overrides Function ToString() As String
        Return $"{name}  [L={length}]"
    End Function
End Class

Public Class Hit

    Public Property Full As Score
    Public Property Best As Score
    Public Property exp As Double
    Public Property N As Integer
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

    Public Overrides Function ToString() As String
        Return Me.GetJson
    End Function
End Structure