Imports System.Runtime.CompilerServices
Imports LANS.SystemsBiology.AnalysisTools.SequenceTools.SequencePatterns.SequenceLogo
Imports LANS.SystemsBiology.SequenceModel.FASTA
Imports LANS.SystemsBiology.SequenceModel.Patterns
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq

Namespace Motif

    Public Class MotifPWM : Inherits ClassObject
        Implements IIterator(Of ResidueSite)

        Public Property PWM As ResidueSite()
        Public Property Alphabets As Char()

        Public Iterator Function GetEnumerator() As IEnumerator(Of ResidueSite) Implements IIterator(Of ResidueSite).GetEnumerator
            For Each x As ResidueSite In PWM
                Yield x
            Next
        End Function

        Public Iterator Function IGetEnumerator() As IEnumerator Implements IIterator(Of ResidueSite).IGetEnumerator
            Yield GetEnumerator()
        End Function

        Public Shared Function NT_PWM(sites As IEnumerable(Of ResidueSite)) As MotifPWM
            Return New MotifPWM With {
                .Alphabets = ColorSchema.NT.ToArray,
                .PWM = sites.ToArray
            }
        End Function

        Public Shared Function AA_PWM(sites As IEnumerable(Of ResidueSite)) As MotifPWM
            Return New MotifPWM With {
                .Alphabets = ColorSchema.AA,
                .PWM = sites.ToArray
            }
        End Function
    End Class

    Public Module PWM

        ''' <summary>
        ''' 从Clustal比对结果之中生成PWM用于SequenceLogo的绘制
        ''' </summary>
        ''' <param name="fa"></param>
        ''' <returns></returns>
        Public Function FromMla(fa As FastaFile) As MotifPWM
            Dim f As PatternModel = PatternsAPI.Frequency(fa)
            Dim n As Integer = fa.NumberOfFasta
            Dim base As Integer = If(fa.First.IsProtSource, 20, 4)
            Dim en As Double = (1 / Math.Log(2)) * ((base - 1) / (2 * n))
            Dim H As Double() = f.Residues.ToArray(Function(x) x.Alphabets.__hi)
            Dim PWM = (From x In f.Residues.SeqIterator Select __residue(x.obj.Alphabets, H(x.Pos), en, base, x.Pos)).ToArray

            If base = 20 Then
                Return MotifPWM.AA_PWM(PWM)
            Else
                Return MotifPWM.NT_PWM(PWM)
            End If
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