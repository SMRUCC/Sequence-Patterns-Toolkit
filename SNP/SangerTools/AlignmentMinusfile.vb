Imports Microsoft.VisualBasic.Language.C
Imports System.Diagnostics
Imports System.IO
Imports FILE = System.IO.StreamWriter
Imports System.Runtime.CompilerServices
Imports LANS.SystemsBiology.SequenceModel.FASTA
Imports Microsoft.VisualBasic.Terminal

Public Module AlignmentMinusfile

    Public number_of_samples As Integer
    Public number_of_snps As Integer
    Public sequence_names As String()
    Public snp_locations As Integer()
    Public pseudo_reference_sequence As Char()
    Public length_of_genome As Integer

    Public Sub GetBasesForEachSNP(ByRef filename As String, ByRef bases_for_snps As Char()())
        Call New FastaFile(filename).GetBasesForEachSNP(bases_for_snps)
    End Sub

    <Extension>
    Public Sub GetBasesForEachSNP(fasta As FastaFile, ByRef bases_for_snps As Char()())
        Dim sequence_number As Integer = 0
        Dim length_of_genome_found As UInteger = 0

        For Each fa As FastaToken In fasta
            Dim seq As Char() = fa.SequenceData.ToCharArray

            If sequence_number = 0 Then
                length_of_genome_found = fa.Length
            End If
            For i As Integer = 0 To number_of_snps - 1
                bases_for_snps(i)(sequence_number) = Char.ToUpper(seq(snp_locations(i)))
            Next

            If seq.Length <> length_of_genome_found Then
                Dim msg As String = STDIO.Format(UnEqualLength, fasta.FileName, CInt(length_of_genome_found), CInt(seq.Length), fa.Title)
                StdErr.Write(msg)
                Environment.[Exit](1)
            End If

            sequence_number += 1
        Next
    End Sub

    Const UnEqualLength As String = "Alignment %s contains sequences of unequal length. Expected length is %i but got %i in sequence %s" & vbLf & vbLf

    ''' <summary>
    ''' Detection of the SNP sites based on a set of fasta sequence.
    ''' </summary>
    ''' <param name="filename">The input fasta sequence.</param>
    ''' <param name="pure_mode"></param>
    ''' <param name="output_monomorphic"></param>
    Public Sub DetectSNPs(ByRef filename As String, pure_mode As Integer, output_monomorphic As Integer)
        Call New FastaFile(filename).DetectSNPs(pure_mode, output_monomorphic)
    End Sub

    ''' <summary>
    ''' Detection of the SNP sites based on a set of fasta sequence.
    ''' </summary>
    ''' <param name="fasta">The input fasta sequence.</param>
    ''' <param name="pure_mode"></param>
    ''' <param name="output_monomorphic"></param>
    ''' 
    <Extension>
    Public Sub DetectSNPs(fasta As FastaFile, pure_mode As Integer, output_monomorphic As Integer)
        Dim first_sequence As String = Nothing

        number_of_snps = 0
        number_of_samples = 0
        length_of_genome = 0
        sequence_names = New String(DefineConstants.DEFAULT_NUM_SAMPLES - 1) {}

        For Each fa As FastaToken In fasta

            If number_of_samples = 0 Then
                length_of_genome = fa.Length
                Memset(first_sequence, "N"c, length_of_genome)
                Memset(pseudo_reference_sequence, "N"c, length_of_genome)
            End If

            Dim seq As Char() = fa.SequenceData.ToCharArray

            For i As Integer = 0 To length_of_genome - 1
                If first_sequence(i) = "#"c Then
                    Continue For
                End If

                If first_sequence(i) = "N"c AndAlso IsUnknown(fa.SequenceData(i)) = 0 Then
                    first_sequence = CString.ChangeCharacter(first_sequence, i, Char.ToUpper(seq(i)))
                    pseudo_reference_sequence(i) = Char.ToUpper(seq(i))
                End If

                ' in pure mode we only want /ACGT/i, if any other base is found the whole column is excluded
                If pure_mode <> 0 AndAlso IsPure(seq(i)) = 0 Then
                    first_sequence = CString.ChangeCharacter(first_sequence, i, "#"c)
                    Continue For
                End If

                If first_sequence(i) <> ">"c AndAlso IsUnknown(seq(i)) = 0 AndAlso first_sequence(i) <> "N"c AndAlso first_sequence(i) <> Char.ToUpper(seq(i)) Then
                    first_sequence = CString.ChangeCharacter(first_sequence, i, ">"c)
                End If
            Next

            If number_of_samples >= DefineConstants.DEFAULT_NUM_SAMPLES Then
            End If

            sequence_names(number_of_samples) = fa.Title
            number_of_samples += 1
        Next

        For i = 0 To length_of_genome - 1
            If first_sequence(i) = ">"c OrElse (output_monomorphic <> 0 AndAlso first_sequence(i) <> "#"c) Then
                number_of_snps += 1
            End If
        Next

        If number_of_snps = 0 Then
            StdErr.Write(NoSNPs & vbLf)
            StdErr.Flush()
            Environment.[Exit](1)
        End If

        Dim current_snp_index As Integer = 0

        For i = 0 To length_of_genome - 1
            If first_sequence(i) = ">"c OrElse (output_monomorphic <> 0 AndAlso first_sequence(i) <> "#"c) Then
                snp_locations(current_snp_index) = i
                current_snp_index += 1
            End If
        Next
    End Sub

    Const NoSNPs As String = "Warning: No SNPs were detected so there is nothing to output."
End Module
