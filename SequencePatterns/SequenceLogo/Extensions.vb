Imports System.Runtime.CompilerServices
Imports ScannerMotif = SMRUCC.genomics.Analysis.SequenceTools.SequencePatterns.Motif.Motif

Namespace SequenceLogo

    Public Module Extensions

        <Extension>
        Public Function CreateDrawingModel(motif As ScannerMotif) As DrawingModel
            Return New DrawingModel With {
                .Residues = motif _
                    .region _
                    .Select(Function(r)
                                Return New Residue With {
                                    .Alphabets = r.frequency _
                                        .Select(Function(b)
                                                    Return New Alphabet With {
                                                        .Alphabet = b.Key,
                                                        .RelativeFrequency = b.Value
                                                    }
                                                End Function) _
                                        .ToArray,
                                    .Position = r.index
                                }
                            End Function) _
                    .ToArray
            }
        End Function
    End Module
End Namespace