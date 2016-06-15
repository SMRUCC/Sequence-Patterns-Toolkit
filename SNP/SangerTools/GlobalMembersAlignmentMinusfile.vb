Imports Microsoft.VisualBasic.Language.C
Imports System.Diagnostics
Imports System.IO
Imports FILE = System.IO.StreamWriter
Imports System.Runtime.CompilerServices

Public Class GlobalMembersAlignmentMinusfile

    Public Shared number_of_samples As Integer
    Public Shared number_of_snps As Integer
    Public Shared sequence_names As String()
    Public Shared snp_locations As Integer()
    Public Shared pseudo_reference_sequence As String
    Public Shared length_of_genome As Integer

    Public Shared Sub get_bases_for_each_snp(ByRef filename As String, ByRef bases_for_snps As String())
        'Dim l As Integer
        'Dim i As Integer = 0
        'Dim sequence_number As Integer = 0
        'Dim length_of_genome_found As UInteger = 0


        'While (InlineAssignHelper(l, GlobalMembersAlignmentMinusfile.kseq_read(seq))) >= 0
        '    If sequence_number = 0 Then
        '        length_of_genome_found = seq.seq.l
        '    End If
        '    For i = 0 To number_of_snps - 1
        '        bases_for_snps(i)(sequence_number) = Char.ToUpper(DirectCast(seq.seq.s, String)(snp_locations(i)))
        '    Next

        '    If seq.seq.l <> length_of_genome_found Then
        '        StdErr.Write("Alignment %s contains sequences of unequal length. Expected length is %i but got %i in sequence %s" & vbLf & vbLf, filename, CInt(length_of_genome_found), CInt(seq.seq.l), seq.name.s)
        '        Environment.[Exit](1)
        '    End If
        '    sequence_number += 1
        'End While
    End Sub

    Public Shared Sub detect_snps(ByRef filename As String, pure_mode As Integer, output_monomorphic As Integer)
        'Dim i As Integer
        'Dim l As Integer
        'number_of_snps = 0
        'number_of_samples = 0
        'length_of_genome = 0
        'Dim first_sequence As String

        'sequence_names = New String(DefineConstants.DEFAULT_NUM_SAMPLES - 1) {}

        'While (GlobalMembersAlignmentMinusfile.kseq_read(seq).shadowscopy(l))) >= 0
        '    If number_of_samples = 0 Then
        '        length_of_genome = seq.seq.l

        '        'C++ TO C# CONVERTER TODO TASK: The memory management function 'memset' has no equivalent in C#:
        '        Memset(first_sequence, "N"c, length_of_genome)
        '        'C++ TO C# CONVERTER TODO TASK: The memory management function 'memset' has no equivalent in C#:
        '        Memset(pseudo_reference_sequence, "N"c, length_of_genome)
        '    End If
        '    For i = 0 To length_of_genome - 1
        '        If first_sequence(i) = "#"c Then
        '            Continue For
        '        End If

        '        If first_sequence(i) = "N"c AndAlso GlobalMembersAlignmentMinusfile.is_unknown(seq.seq.s(i)) = 0 Then
        '            first_sequence = CString.ChangeCharacter(first_sequence, i, Char.ToUpper(seq.seq.s(i)))
        '            pseudo_reference_sequence(i) = Char.ToUpper(seq.seq.s(i))
        '        End If

        '        ' in pure mode we only want /ACGT/i, if any other base is found the whole column is excluded
        '        If pure_mode <> 0 AndAlso GlobalMembersAlignmentMinusfile.is_pure(seq.seq.s(i)) = 0 Then
        '            first_sequence = CString.ChangeCharacter(first_sequence, i, "#"c)
        '            Continue For
        '        End If

        '        If first_sequence(i) <> ">"c AndAlso GlobalMembersAlignmentMinusfile.is_unknown(seq.seq.s(i)) = 0 AndAlso first_sequence(i) <> "N"c AndAlso first_sequence(i) <> Char.ToUpper(seq.seq.s(i)) Then
        '            first_sequence = CString.ChangeCharacter(first_sequence, i, ">"c)
        '        End If
        '    Next

        '    'C++ TO C# CONVERTER TODO TASK: The memory management function 'realloc' has no equivalent in C#:
        '    '	 sequence_names = realloc(sequence_names, (number_of_samples + 1) * sizeof(string));
        '    If number_of_samples >= DefineConstants.DEFAULT_NUM_SAMPLES Then
        '    End If
        '    'C++ TO C# CONVERTER TODO TASK: The memory management function 'calloc' has no equivalent in C#:
        '    '   sequence_names[number_of_samples] = calloc(DefineConstants.MAX_SAMPLE_NAME_SIZE,sizeof(sbyte));
        '    sequence_names(number_of_samples) = seq.name.s

        '    number_of_samples += 1
        'End While

        'For i = 0 To length_of_genome - 1
        '    If first_sequence(i) = ">"c OrElse (output_monomorphic <> 0 AndAlso first_sequence(i) <> "#"c) Then
        '        number_of_snps += 1
        '    End If
        'Next

        'If number_of_snps = 0 Then
        '    StdErr.Write("Warning: No SNPs were detected so there is nothing to output." & vbLf)
        '    StdErr.Flush()
        '    Environment.[Exit](1)
        'End If

        'Dim current_snp_index As Integer = 0

        'For i = 0 To length_of_genome - 1
        '    If first_sequence(i) = ">"c OrElse (output_monomorphic <> 0 AndAlso first_sequence(i) <> "#"c) Then
        '        snp_locations(current_snp_index) = i
        '        current_snp_index += 1
        '    End If
        'Next

        'Return
    End Sub
End Class
