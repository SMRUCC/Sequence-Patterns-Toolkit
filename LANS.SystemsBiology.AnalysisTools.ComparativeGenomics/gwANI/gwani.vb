Imports Microsoft.VisualBasic.Terminal.STDIO
Imports LANS.SystemsBiology.SequenceModel.FASTA
Imports Microsoft.VisualBasic.Language.C

Public NotInheritable Class GlobalMembersGwani

    '     *  Wellcome Trust Sanger Institute
    '     *  Copyright (C) 2016  Wellcome Trust Sanger Institute
    '     *  
    '     *  This program is free software; you can redistribute it and/or
    '     *  modify it under the terms of the GNU General Public License
    '     *  as published by the Free Software Foundation; either version 3
    '     *  of the License, or (at your option) any later version.
    '     *  
    '     *  This program is distributed in the hope that it will be useful,
    '     *  but WITHOUT ANY WARRANTY; without even the implied warranty of
    '     *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    '     *  GNU General Public License for more details.
    '     *  
    '     *  You should have received a copy of the GNU General Public License
    '     *  along with this program; if not, write to the Free Software
    '     *  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

    Public Shared Function get_length_of_genome() As Integer
        Return length_of_genome
    End Function
    Public Shared Function get_number_of_samples() As Integer
        Return number_of_samples
    End Function
    Public Shared Function get_sequence_names() As String()
        Return sequence_names
    End Function

    Public Shared Sub check_input_file_and_calc_dimensions(ByRef filename As String)
        number_of_samples = 0
        length_of_genome = 0

        sequence_names = New String(DefineConstants.DEFAULT_NUM_SAMPLES - 1) {}

        For Each seq As FastaToken In New FastaFile(filename)
            If number_of_samples = 0 Then
                length_of_genome = seq.Length
            ElseIf length_of_genome <> seq.Length Then
                printf("Alignment %s contains sequences of unequal length. Expected length is %i but got %i in sequence %s" & vbLf & vbLf, filename, CInt(length_of_genome), CInt(seq.Length), seq.Title)

                Environment.[Exit](1)
            End If

            ' The sample name is initially set to a large number but make sure this can be increased dynamically

            If number_of_samples >= DefineConstants.DEFAULT_NUM_SAMPLES Then
            End If
            sequence_names(number_of_samples) = seq.Title

            number_of_samples += 1
        Next

        ' First pass of the file get the length of the alignment, number of samples and sample names
    End Sub

    Public Shared Sub calculate_and_output_gwani(ByRef filename As String)
        GlobalMembersGwani.check_input_file_and_calc_dimensions(filename)
        GlobalMembersGwani.print_header()

        Dim i As Integer
        Dim j As Integer
        For i = 0 To number_of_samples - 1
            Console.Write("{0}", sequence_names(i))
            Dim similarity_percentage As Double()
            similarity_percentage = New Double(number_of_samples) {}
            GlobalMembersGwani.calc_gwani_between_a_sample_and_everything_afterwards(filename, i, similarity_percentage)

            For j = 0 To number_of_samples - 1
                If similarity_percentage(j) < 0 Then
                    Console.Write(vbTab & "-")
                Else
                    Console.Write(vbTab & "{0:f}", similarity_percentage(j))
                End If
            Next
            Console.Write(vbLf)

        Next
    End Sub
    Public Shared Sub print_header()
        Dim i As Integer
        For i = 0 To number_of_samples - 1
            Console.Write(vbTab & "{0}", sequence_names(i))
        Next
        Console.Write(vbLf)
    End Sub
    Public Shared Sub calc_gwani_between_a_sample_and_everything_afterwards(ByRef filename As String, comparison_index As Integer, similarity_percentage As Double())
        Dim current_index As Integer = 0
        Dim i As Integer
        Dim bases_in_common As Integer
        Dim length_without_gaps As Integer
        Dim comparison_sequence As String

        For Each seq As FastaToken In New FastaFile(filename)

            If current_index < comparison_index Then
                similarity_percentage(current_index) = -1
            ElseIf current_index = comparison_index Then
                similarity_percentage(current_index) = 100
                For i = 0 To length_of_genome - 1
                    'standardise the input so that case doesnt matter and replace unknowns with single type
                    comparison_sequence = ChangeCharacter(comparison_sequence, i, Char.ToUpper(seq.SequenceData(i)))
                    If IsUnknown(comparison_sequence(i)) <> 0 Then
                        comparison_sequence = ChangeCharacter(comparison_sequence, i, "N"c)
                    End If
                Next
            Else
                bases_in_common = 0
                length_without_gaps = length_of_genome
                For i = 0 To length_of_genome - 1

                    If comparison_sequence(i) = "N"c OrElse IsUnknown(seq.SequenceData(i)) <> 0 Then
                        length_without_gaps -= 1
                    ElseIf comparison_sequence(i) = Char.ToUpper(seq.SequenceData(i)) AndAlso IsUnknown(seq.SequenceData(i)) = 0 Then
                        bases_in_common += 1
                    End If
                Next
                If length_without_gaps > 0 Then
                    similarity_percentage(current_index) = (bases_in_common * 100.0) / length_without_gaps
                Else
                    similarity_percentage(current_index) = 0

                End If
            End If
            current_index += 1
        Next
    End Sub

    Public Shared Sub calc_gwani_between_a_sample_and_everything_afterwards_memory(ByRef comparison_sequence As Char()(), comparison_index As Integer, similarity_percentage As Double())
        Dim current_index As Integer = 0
        Dim i As Integer
        Dim j As Integer
        Dim bases_in_common As Integer
        Dim length_without_gaps As Integer

        For j = 0 To number_of_samples - 1
            If current_index < comparison_index Then
                similarity_percentage(current_index) = -1
            ElseIf current_index = comparison_index Then
                similarity_percentage(current_index) = 100
            Else
                bases_in_common = 0
                length_without_gaps = length_of_genome
                For i = 0 To length_of_genome - 1

                    If comparison_sequence(comparison_index)(i) = "N"c OrElse comparison_sequence(j)(i) = "N"c Then
                        length_without_gaps -= 1
                    ElseIf comparison_sequence(comparison_index)(i) = comparison_sequence(j)(i) Then
                        bases_in_common += 1
                    End If
                Next
                If length_without_gaps > 0 Then
                    similarity_percentage(current_index) = (bases_in_common * 100.0) / length_without_gaps
                Else
                    similarity_percentage(current_index) = 0

                End If
            End If
            current_index += 1
        Next
    End Sub
    Public Shared Sub fast_calculate_gwani(ByRef filename As String)
        GlobalMembersGwani.check_input_file_and_calc_dimensions(filename)
        GlobalMembersGwani.print_header()

        ' initialise space to store entire genome
        Dim i As Integer
        Dim j As Integer
        Dim comparison_sequence As Char()()
        comparison_sequence = New Char(GlobalMembersGwani.get_number_of_samples())() {}

        ' Store all sequences in a giant array - eek

        For Each seq As FastaToken In New FastaFile(filename)
            comparison_sequence(i) = seq.Title
            i += 1
        Next

        For j = 0 To number_of_samples - 1
            For i = 0 To length_of_genome - 1
                'standardise the input so that case doesnt matter and replace unknowns with single type
                comparison_sequence(j)(i) = Char.ToUpper(comparison_sequence(j)(i))
                If IsUnknown(comparison_sequence(j)(i)) <> 0 Then
                    comparison_sequence(j)(i) = "N"c
                End If
            Next
        Next

        For i = 0 To number_of_samples - 1
            Console.Write("{0}", sequence_names(i))
            Dim similarity_percentage As Double()
            similarity_percentage = New Double(number_of_samples) {}

            GlobalMembersGwani.calc_gwani_between_a_sample_and_everything_afterwards_memory(comparison_sequence, i, similarity_percentage)

            For j = 0 To number_of_samples - 1
                If similarity_percentage(j) < 0 Then
                    Console.Write(vbTab & "-")
                Else
                    Console.Write(vbTab & "{0:f}", similarity_percentage(j))
                End If
            Next
            Console.Write(vbLf)
        Next
    End Sub

    Private Shared length_of_genome As Integer
    Private Shared number_of_samples As Integer
    Private Shared sequence_names As String()

    Partial Friend NotInheritable Class DefineConstants
        Private Sub New()
        End Sub
        Public Const KS_SEP_SPACE As Integer = 0
        Public Const KS_SEP_TAB As Integer = 1
        Public Const KS_SEP_LINE As Integer = 2
        Public Const KS_SEP_MAX As Integer = 2
        Public Const DEFAULT_NUM_SAMPLES As Integer = 65536
        Public Const PROGRAM_NAME As String = "panito"
    End Class
End Class
