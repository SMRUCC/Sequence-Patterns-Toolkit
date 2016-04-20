Imports LANS.SystemsBiology.AnalysisTools.NBCR.Extensions.MEME_Suite.ComponentModel
Imports LANS.SystemsBiology.AnalysisTools.NBCR.Extensions.MEME_Suite.DocumentFormat
Imports Microsoft.VisualBasic.Linq.Extensions

Namespace SequenceLogo

    Public Class DrawingModel

        Public Property Residues As Residue()
        Public Property En As Double
        Public Property ModelsId As String

        Public Overrides Function ToString() As String
            Return ModelsId
        End Function

        ''' <summary>
        ''' The approximation for the small-sample correction, en, Is given by
        '''     en = 1/ln2 x (s-1)/2n
        ''' 
        ''' </summary>
        ''' <param name="s">s Is 4 For nucleotides, 20 For amino acids</param>
        ''' <param name="n">n Is the number Of sequences In the alignment</param>
        ''' <returns></returns>
        Public Shared Function Calculates_En(s As Integer, n As Integer) As Double
            Dim result As Double = 1 / Math.Log(2)
            result *= (s - 1) / 2 * n
            Return result
        End Function

        Public Shared Function CreateObject(LDM As Analysis.MotifScans.AnnotationModel) As DrawingModel
            Dim residues = LDM.PspMatrix.ToArray(Function(x) PspVectorToResidue(x))
            Dim model As DrawingModel = New DrawingModel With {
                .ModelsId = LDM.ToString,
                .Residues = residues.AddHandle.ToArray
            }
            Return model
        End Function

        Public Shared Function GenerateFromMEMEMotif(Motif As MEME.LDM.Motif) As DrawingModel
            Dim Alphabets As Char() = If(Motif.NtMolType,
                ColorSchema.NucleotideSchema.Keys.ToArray,
                ColorSchema.ProteinSchema.Keys.ToArray)
            Dim En As Double = Calculates_En(s:=Alphabets.Count, n:=Motif.Sites.Length)
            Dim rsd = (From residue As MotifPM
                       In Motif.PspMatrix
                       Select PspVectorToResidue(residue)).ToArray.AddHandle.ToArray
            Dim Model As DrawingModel = New DrawingModel With {
                .Residues = rsd,
                .En = En,
                .ModelsId = Motif.uid
            }

            Return Model
        End Function

        Private Shared Function PspVectorToResidue(x As MotifPM) As Residue
            Dim Residue As Residue = New Residue With {
                .Alphabets = {
                    New Alphabet With {.Alphabet = "A"c, .RelativeFrequency = x.A},
                    New Alphabet With {.Alphabet = "T"c, .RelativeFrequency = x.T},
                    New Alphabet With {.Alphabet = "G"c, .RelativeFrequency = x.G},
                    New Alphabet With {.Alphabet = "C"c, .RelativeFrequency = x.C}
                },
                .Bits = x.Bits  ' = SequenceLogo.Residue.CalculatesBits(Residue, En, NtMol:=Alphabets.Count = 4)
            }
            '   Residue.Bits = x.Bits

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