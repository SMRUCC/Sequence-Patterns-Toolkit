Imports System.Runtime.CompilerServices
Imports LANS.SystemsBiology.SequenceModel.FASTA
Imports Microsoft.VisualBasic.Linq

Namespace Motif

    Public Class Motif
        Public Property PWM As ResidueSite()

    End Class

    Public Module PWM

        Public Function FromMla(fa As FastaFile) As Motif
            Dim f = LANS.SystemsBiology.BioAssemblyExtensions.Frequency(fa)
            Dim n As Integer = fa.NumberOfFasta
            Dim base As Integer = If(fa.First.IsProtSource, 20, 4)
            Dim en As Double = (1 / Math.Log(2)) * ((base - 1) / (2 * n))
            Dim H = f.ToArray(Function(x) x.Value.__hi)
            Dim PWM = (From x In f.Values.SeqIterator Select __residue(x.obj, H(x.Pos), en, base, x.Pos)).ToArray
            Return New Motif With {.PWM = PWM}
        End Function

        ''' <summary>
        '''
        ''' </summary>
        ''' <param name="f">ATGC</param>
        ''' <param name="h"></param>
        ''' <param name="en"></param>
        ''' <param name="n"></param>
        ''' <returns></returns>
        Private Function __residue(f As Dictionary(Of Char, Double), h As Double, en As Double, n As Integer, i As Integer) As ResidueSite
            Dim R As Double = Math.Log(n, 2) - (h + en)
            Dim ATGC As Double() = {f("A"c), f("T"c), f("G"c), f("C"c)}
            Return New ResidueSite With {
                .Bits = R,
                .PWM = ATGC,
                .Site = i
            }
        End Function

        <Extension>
        Private Function __hi(f As Dictionary(Of Char, Double)) As Double
            Dim h As Double = f.Values.Sum(Function(n) n * Math.Log(n, 2))
            h = 0 - h
            Return h
        End Function
    End Module
End Namespace