Imports Microsoft.VisualBasic.Serialization
Imports Microsoft.VisualBasic.Serialization.BinaryDumping

<Serializable> Public Structure SeedData

    Public Seeds As String()

    Public Overrides Function ToString() As String
        Return Me.GetJson
    End Function

    Public Sub Save(path As String)
        Call Me.Serialize(path)
    End Sub

    Public Shared Function Load(path As String) As SeedData
        Return path.Load(Of SeedData)
    End Function
End Structure
