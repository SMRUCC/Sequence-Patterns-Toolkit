Imports LANS.SystemsBiology.ComponentModel.Loci
Imports LANS.SystemsBiology.ComponentModel.Loci.Abstract
Imports LANS.SystemsBiology.SequenceModel
Imports LANS.SystemsBiology.SequenceModel.NucleotideModels

Namespace Topologically

    Public Class PalindromeLoci : Inherits Contig
        Implements I_PolymerSequenceModel
        Implements ILoci

        Public Property Loci As String Implements I_PolymerSequenceModel.SequenceData
        Public Property Start As Long Implements ILoci.Left
        Public Property Palindrome As String
        Public Property PalEnd As Integer
        Public Property MirrorSite As String

        Public ReadOnly Property Mirror As Integer
            Get
                Return Start + Len(Loci)
            End Get
        End Property

        Public ReadOnly Property Length As Integer
            Get
                Return Len(Loci)
            End Get
        End Property

        Public Shared Function SelectSite(sites As IEnumerable(Of PalindromeLoci)) As PalindromeLoci
            Dim LQuery = (From site As PalindromeLoci
                          In sites
                          Select site
                          Order By site.Length Descending).FirstOrDefault
            Return LQuery
        End Function

        Protected Overrides Function __getMappingLoci() As NucleotideLocation
            Return New NucleotideLocation(Start, PalEnd)
        End Function

        Public Overloads Shared Function GetMirror(seq As String) As String
            Return New String(seq.Reverse.ToArray)
        End Function

        Public Shared Function GetPalindrome(seq As String) As String
            seq = GetMirror(seq)
            seq = NucleicAcid.Complement(seq)
            Return seq
        End Function
    End Class
End Namespace