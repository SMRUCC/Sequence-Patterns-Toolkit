Imports LANS.SystemsBiology.AnalysisTools.SequenceTools.SequencePatterns.Motif
Imports LANS.SystemsBiology.SequenceModel
Imports Microsoft.VisualBasic.Linq.Extensions

Namespace SequenceLogo

    Public Interface ILogoResidue : Inherits IPatternProvider
        ReadOnly Property Bits As Double
        Default ReadOnly Property Probability(c As Char) As Double
    End Interface

    Public Class DrawingModel

        Public Property Residues As Residue()
        Public Property En As Double
        Public Property ModelsId As String

        Public Overrides Function ToString() As String
            Return ModelsId
        End Function

        Public Shared Function AAResidue(x As ILogoResidue) As Residue
            Dim Residue As Residue = New Residue With {
                .Alphabets = ColorSchema.AA.ToArray(
                    Function(r) New Alphabet With {
                        .Alphabet = r,
                        .RelativeFrequency = x(r)}),
                .Bits = x.Bits
            }

            Return Residue
        End Function

        Public Shared Function NTResidue(x As ILogoResidue) As Residue
            Dim Residue As Residue = New Residue With {
                .Alphabets = {
                    New Alphabet With {.Alphabet = "A"c, .RelativeFrequency = x("A"c)},
                    New Alphabet With {.Alphabet = "T"c, .RelativeFrequency = x("T"c)},
                    New Alphabet With {.Alphabet = "G"c, .RelativeFrequency = x("G"c)},
                    New Alphabet With {.Alphabet = "C"c, .RelativeFrequency = x("C"c)}
                },
                .Bits = x.Bits
            }

            Return Residue
        End Function

        ''' <summary>
        ''' ## Get information content profile from PWM
        ''' </summary>
        ''' <param name="pwm"></param>
        ''' <returns></returns>
        Public Shared Function pwm2ic(pwm As DrawingModel) As Double()
            Dim npos As Integer = pwm.Residues.First.Alphabets.Length
            Dim ic As Double() = New Double(npos - 1) {}
            For i As Integer = 0 To npos - 1
                Dim idx As Integer = i
                ic(i) = 2 + Sum(pwm.Residues.ToArray(Of Double)(
                                Function(x) If(x.Alphabets(idx).RelativeFrequency > 0, x.Alphabets(idx).RelativeFrequency * Math.Log(x.Alphabets(idx).RelativeFrequency, 2), 0)))
            Next

            Return ic
        End Function
    End Class
End Namespace