Imports Microsoft.VisualBasic.Serialization

''' <summary>
''' # hmmsearch :: search profile(s) against a sequence database
''' </summary>
Public Class hmmsearch

    ''' <summary>
    ''' # hmmsearch :: search profile(s) against a sequence database
    ''' # HMMER 3.1b1 (May 2013); http://hmmer.org/
    ''' # Copyright (C) 2013 Howard Hughes Medical Institute.
    ''' # Freely distributed under the GNU General Public License (GPLv3).
    ''' </summary>
    ''' <returns></returns>
    Public Property version As String
    ''' <summary>
    ''' query HMM file
    ''' </summary>
    ''' <returns></returns>
    Public Property HMM As String
    ''' <summary>
    ''' target sequence database
    ''' </summary>
    ''' <returns></returns>
    Public Property source As String
    Public Property Queries As PfamQuery()

    Public Overrides Function ToString() As String
        Return New With {version, HMM, source}.GetJson
    End Function
End Class

Public Class PfamQuery
    Public Property Query As String
    Public Property MLen As Integer
    Public Property Accession As String
    Public Property describ As String
    Public Property hits As Score()
    Public Property uncertain As Score()
    Public Property alignments As AlignmentHit()

    Public Overrides Function ToString() As String
        Return Query
    End Function
End Class

Public Class Score
    Public Property Full As hmmscan.Score
    Public Property Best As hmmscan.Score
    Public Property exp As Double
    Public Property N As Integer
    Public Property locus As String
    Public Property describ As String

    Public Overrides Function ToString() As String
        Return locus
    End Function
End Class

Public Class AlignmentHit

    Public Property locus As String
    Public Property hits As hmmscan.Align()

    Public Overrides Function ToString() As String
        Return locus
    End Function
End Class