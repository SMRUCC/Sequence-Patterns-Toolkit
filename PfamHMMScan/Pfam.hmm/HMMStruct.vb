Imports Microsoft.VisualBasic.Language

''' <summary>
''' Structure containing information for an HMM profile retrieved from the PFAM database.
''' </summary>
''' <remarks>
''' http://cn.mathworks.com/help/bioinfo/ref/gethmmprof.html
''' </remarks>
Public Class HMMStruct : Inherits ClassObject

    ''' <summary>
    ''' The protein family name (unique identifier) Of the HMM profile record In the PFAM database.
    ''' </summary>
    ''' <returns></returns>
    Public Property Name As String
    ''' <summary>
    ''' The protein family accession number Of the HMM profile record In the PFAM database.
    ''' </summary>
    ''' <returns></returns>
    Public Property PfamAccessionNumber As String
    ''' <summary>
    ''' Description Of the HMM profile.
    ''' </summary>
    ''' <returns></returns>
    Public Property ModelDescription As String
    ''' <summary>
    ''' The length Of the profile (number Of MATCH states).
    ''' </summary>
    ''' <returns></returns>
    Public Property ModelLength As Integer
    ''' <summary>
    ''' The alphabet used In the model, 'AA' or 'NT'. Note:  AlphaLength Is 20 for 'AA' and 4 for 'NT'.
    ''' </summary>
    ''' <returns></returns>
    Public Property Alphabet As String
    ''' <summary>
    ''' Symbol emission probabilities In the MATCH states.
    ''' The format Is a matrix Of size ModelLength-by-AlphaLength, where Each row corresponds To the emission distribution For a specific MATCH state.
    ''' </summary>
    ''' <returns></returns>
    Public Property MatchEmission As String
    ''' <summary>
    ''' Symbol emission probabilities In the INSERT state.
    ''' The format Is a matrix Of size ModelLength-by-AlphaLength, where Each row corresponds To the emission distribution For a specific INSERT state.
    ''' </summary>
    ''' <returns></returns>
    Public Property InsertEmission As String
    ''' <summary>
    ''' Symbol emission probabilities In the MATCH And INSERT states For the NULL model.
    ''' The format Is a 1-by-AlphaLength row vector.
    ''' Note: NULL probabilities are also known As the background probabilities.
    ''' </summary>
    ''' <returns></returns>
    Public Property NullEmission As String
    ''' <summary>
    ''' BEGIN state transition probabilities.
    ''' Format Is a 1-by-(ModelLength + 1) row vector
    ''' [B->D1 B->M1 B->M2 B->M3 .... B->Mend]
    ''' </summary>
    ''' <returns></returns>
    Public Property BeginX As String
    ''' <summary>
    ''' MATCH state transition probabilities.
    ''' Format Is a 4-by-(ModelLength - 1) matrix
    ''' [ M1->M2 M2->M3 ... M[end-1]->Mend;
    '''   M1->I1 M2->I2 ... M[end-1]->I[end-1];
    '''   M1->D2 M2->D3 ... M[end-1]->Dend;
    '''   M1->E  M2->E  ... M[end-1]->E  ]
    ''' </summary>
    ''' <returns></returns>
    Public Property MatchX As String
    ''' <summary>
    ''' INSERT state transition probabilities.
    ''' Format Is a 2-by-(ModelLength - 1) matrix
    ''' [ I1->M2 I2->M3 ... I[end-1]->Mend;
    '''   I1->I1 I2->I2 ... I[end-1]->I[end-1] ]
    ''' </summary>
    ''' <returns></returns>
    Public Property InsertX As String
    ''' <summary>
    ''' DELETE state transition probabilities.
    ''' Format Is a 2-by-(ModelLength - 1) matrix
    ''' [ D1->M2 D2->M3 ... D[end-1]->Mend ;
    '''   D1->D2 D2->D3 ... D[end-1]->Dend ]
    ''' </summary>
    ''' <returns></returns>
    Public Property DeleteX As String
    ''' <summary>
    ''' Flanking insert states (N And C) used For LOCAL profile alignment.
    ''' Format Is a 2-by-2 matrix
    ''' [N->B  C->T ;
    '''  N->N  C->C]
    ''' </summary>
    ''' <returns></returns>
    Public Property FlankingInsertX As String
    ''' <summary>
    ''' Loop states transition probabilities used for multiple hits alignment.
    ''' Format Is a 2-by-2 matrix
    ''' [E->C  J->B ;
    '''  E->J  J->J]
    ''' </summary>
    ''' <returns></returns>
    Public Property LoopX As String
    ''' <summary>
    ''' Null transition probabilities used To provide scores With log-odds values also For state transitions.
    ''' Format Is a 2-by-1 column vector
    ''' [G->F ; G->G]
    ''' </summary>
    ''' <returns></returns>
    Public Property NullX As String

End Class
