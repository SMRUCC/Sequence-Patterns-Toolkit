
''' <summary>
''' Retrieve hidden Markov model (HMM) profile from PFAM database
''' </summary>
Public Module DBI

    ''' <summary>
    ''' Retrieve hidden Markov model (HMM) profile from PFAM database
    ''' </summary>
    ''' <param name="PFAMName">String specifying a protein family name (unique identifier) of an HMM profile record in the PFAM database. For example, '7tm_2'.</param>
    ''' <returns></returns>
    Public Function GetHMMprof(PFAMName As String) As HMMStruct

    End Function

    ''' <summary>
    ''' Retrieve hidden Markov model (HMM) profile from PFAM database
    ''' </summary>
    ''' <param name="PFAMNumber">
    ''' Integer specifying a protein family number of an HMM profile record in the PFAM database. For example, 2 is the protein family number for the protein family 'PF00002'.
    ''' </param>
    ''' <returns></returns>
    Public Function GetHMMprof(PFAMNumber As Integer) As HMMStruct

    End Function
End Module
