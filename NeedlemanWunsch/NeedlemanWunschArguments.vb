Imports System
Imports System.Collections.Generic
Imports System.Text

''' <summary>
''' Base class for the Needleman-Wunsch Algorithm
''' Bioinformatics 1, WS 15/16
''' Dr. Kay Nieselt and Alexander Seitz
''' </summary>
Public Class NeedlemanWunschArguments

    Private aligned1 As New List(Of String)
    Private aligned2 As New List(Of String)

    ''' <summary>
    ''' get numberOfAlignments </summary>
    ''' <returns> numberOfAlignments </returns>
    Public ReadOnly Property NumberOfAlignments As Integer
        Get
            Return aligned1.Count
        End Get
    End Property

    ''' <summary>
    ''' get gap open penalty </summary>
    ''' <returns> gap open penalty </returns>
    Public Property GapPenalty As Integer = 1

    ''' <summary>
    ''' get match score </summary>
    ''' <returns> match score </returns>

    Public Property MatchScore As Integer = 1

    ''' <summary>
    ''' get mismatch score </summary>
    ''' <returns> mismatch score </returns>
    Public Property MismatchScore As Integer = -1

    ''' <summary>
    ''' get sequence 1 </summary>
    ''' <returns>  sequence 1 </returns>
    Public Property Sequence1 As String

    ''' <summary>
    ''' get sequence 2cted int max (int a, int b, int c) {
    '''    return Math.max(a, Math.max(b, c)); </summary>
    ''' <returns> sequence 2 </returns>
    Public Property Sequence2 As String

    ''' <summary>
    ''' get aligned version of sequence 1 </summary>
    ''' <param name="i"> </param>
    ''' <returns>  aligned sequence 1 </returns>
    Public Function getAligned1(i As Integer) As String
        Return aligned1(i)
    End Function

    ''' <summary>
    ''' set aligned sequence 1 </summary>
    ''' <param name="aligned1"> </param>
    Protected Friend Sub addAligned1(aligned1 As String)
        Me.aligned1.Add(aligned1)
    End Sub

    ''' <summary>
    ''' get aligned version of sequence 2 </summary>
    ''' <param name="i"> </param>
    ''' <returns> aligned sequence 2 </returns>
    Public Function getAligned2(i As Integer) As String
        Return aligned2(i)
    End Function

    ''' <summary>
    ''' set aligned sequence 2 </summary>
    ''' <param name="aligned2"> </param>
    Protected Friend Sub addAligned2(aligned2 As String)
        Me.aligned2.Add(aligned2)
    End Sub

    ''' <summary>
    ''' get computed score </summary>
    ''' <returns> score </returns>
    Public Property Score As Integer

    ''' <summary>
    ''' if char a is equal to char b
    ''' return the match score
    ''' else return mismatch score
    ''' </summary>
    Protected Friend Function match(a As Char, b As Char) As Integer
        If a = b Then Return MatchScore
        Return MismatchScore
    End Function

    ''' <summary>
    ''' return the maximum of a, b and c </summary>
    ''' <param name="a"> </param>
    ''' <param name="b"> </param>
    ''' <param name="c">
    ''' @return </param>
    Protected Friend Function max(a As Integer, b As Integer, c As Integer) As Integer
        Return Math.Max(a, Math.Max(b, c))
    End Function
    ''' <summary>
    ''' reverse a string sequence </summary>
    ''' <param name="seq"> </param>
    ''' <returns> seq in reverse order </returns>
    Protected Friend Function reverse(seq As String) As String
        Dim buf As New StringBuilder(seq)
        Return buf.Reverse().ToString()
    End Function
End Class