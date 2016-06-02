Imports System.Diagnostics
Imports System.IO
Imports FILE = System.IO.StreamWriter

Public Module GlobalMembersVcf

    '
    '	 *  Wellcome Trust Sanger Institute
    '	 *  Copyright (C) 2013  Wellcome Trust Sanger Institute
    '	 *  
    '	 *  This program is free software; you can redistribute it and/or
    '	 *  modify it under the terms of the GNU General Public License
    '	 *  as published by the Free Software Foundation; either version 3
    '	 *  of the License, or (at your option) any later version.
    '	 *  
    '	 *  This program is distributed in the hope that it will be useful,
    '	 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
    '	 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    '	 *  GNU General Public License for more details.
    '	 *  
    '	 *  You should have received a copy of the GNU General Public License
    '	 *  along with this program; if not, write to the Free Software
    '	 *  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
    '	 




    '
    '	 *  Wellcome Trust Sanger Institute
    '	 *  Copyright (C) 2013  Wellcome Trust Sanger Institute
    '	 *  
    '	 *  This program is free software; you can redistribute it and/or
    '	 *  modify it under the terms of the GNU General Public License
    '	 *  as published by the Free Software Foundation; either version 3
    '	 *  of the License, or (at your option) any later version.
    '	 *  
    '	 *  This program is distributed in the hope that it will be useful,
    '	 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
    '	 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    '	 *  GNU General Public License for more details.
    '	 *  
    '	 *  You should have received a copy of the GNU General Public License
    '	 *  along with this program; if not, write to the Free Software
    '	 *  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
    '	 

    Public Sub output_vcf_header(vcf_file_pointer As File, ByRef sequence_names As String(), number_of_samples As Integer, length_of_genome As UInteger)
        Dim i As Integer
        vcf_file_pointer.Write("##fileformat=VCFv4.1" & vbLf)
        vcf_file_pointer.Write("##contig=<ID=1,length=%i>" & vbLf, CInt(length_of_genome))
        vcf_file_pointer.Write("##FORMAT=<ID=GT,Number=1,Type=String,Description=""Genotype"">" & vbLf)
        vcf_file_pointer.Write("#CHROM" & vbTab & "POS" & vbTab & "ID" & vbTab & "REF" & vbTab & "ALT" & vbTab & "QUAL" & vbTab & "FILTER" & vbTab & "INFO" & vbTab & "FORMAT")

        For i = 0 To number_of_samples - 1
            vcf_file_pointer.Write(vbTab & "%s", sequence_names(i))
        Next
        vcf_file_pointer.Write(vbLf)
    End Sub

    Public Sub create_vcf_file(ByRef filename As String, snp_locations As Integer(), number_of_snps As Integer, ByRef bases_for_snps As String(), ByRef sequence_names As String(), number_of_samples As Integer,
        length_of_genome As UInteger, ByRef pseudo_reference_sequence As String)

        Using vcf_file_pointer As FILE = New StreamWriter(New FileStream(filename, FileMode.OpenOrCreate))
            GlobalMembersVcf.output_vcf_header(vcf_file_pointer, sequence_names, number_of_samples, CInt(length_of_genome))
            GlobalMembersVcf.output_vcf_snps(vcf_file_pointer, bases_for_snps, snp_locations, number_of_snps, number_of_samples, pseudo_reference_sequence)
        End Using
    End Sub
    Public Sub output_vcf_snps(vcf_file_pointer As File, ByRef bases_for_snps As String(), snp_locations As Integer(), number_of_snps As Integer, number_of_samples As Integer, ByRef pseudo_reference_sequence As String)
        Dim i As Integer
        For i = 0 To number_of_snps - 1
            GlobalMembersVcf.output_vcf_row(vcf_file_pointer, bases_for_snps(i), snp_locations(i), number_of_samples, pseudo_reference_sequence)
        Next
    End Sub

    Public Sub output_vcf_row(vcf_file_pointer As File, ByRef bases_for_snp As String, snp_location As Integer, number_of_samples As Integer, ByRef pseudo_reference_sequence As String)
        Dim reference_base As Char = pseudo_reference_sequence(snp_location)
        If reference_base = ControlChars.NullChar Then
            StdErr.Write("Couldnt get the reference base for coordinate {0}" & vbLf, snp_location)
            Environment.[Exit](1)
        End If

        ' Chromosome
        vcf_file_pointer.Write("1" & vbTab)

        ' Position
        vcf_file_pointer.Write((CInt(snp_location) + 1) & vbTab)

        'ID
        vcf_file_pointer.Write("." & vbTab)

        ' REF
        vcf_file_pointer.Write(reference_base & vbTab)

        ' ALT
        ' Need to look through list and find unique characters


        'ORIGINAL LINE: sbyte * alt_bases = alternative_bases(reference_base, bases_for_snp, number_of_samples);
        Dim alt_bases As Char = GlobalMembersVcf.alternative_bases(reference_base, bases_for_snp, number_of_samples)

        'ORIGINAL LINE: sbyte * alternative_bases_string = format_alternative_bases(alt_bases);
        Dim alternative_bases_string As Char = GlobalMembersVcf.format_alternative_bases(alt_bases)
        vcf_file_pointer.Write(alternative_bases_string & vbTab)

        ' QUAL
        vcf_file_pointer.Write("." & vbTab)

        ' FILTER
        vcf_file_pointer.Write("." & vbTab)

        ' INFO
        vcf_file_pointer.Write("." & vbTab)

        ' FORMAT
        vcf_file_pointer.Write("GT" & vbTab)

        ' Bases for each sample
        GlobalMembersVcf.output_vcf_row_samples_bases(vcf_file_pointer, reference_base, alt_bases, bases_for_snp, number_of_samples)

        vcf_file_pointer.Write(vbLf)

    End Sub

    Public Sub output_vcf_row_samples_bases(vcf_file_pointer As StreamWriter, reference_base As Char, ByRef alt_bases As String, ByRef bases_for_snp As String, number_of_samples As Integer)
        Dim i As Integer
        Dim format As String

        For i = 0 To number_of_samples - 1
            format = GlobalMembersVcf.format_allele_index(bases_for_snp(i), reference_base, alt_bases)
            vcf_file_pointer.Write(format)

            If i + 1 <> number_of_samples Then
                vcf_file_pointer.Write(vbTab)
            End If
        Next
    End Sub

    Public Function alternative_bases(reference_base As Char, ByRef bases_for_snp As String, number_of_samples As Integer) As String
        Dim i As Integer
        Dim num_alt_bases As Integer = 0
        Dim alt_bases As Char() = New Char(MAXIMUM_NUMBER_OF_ALT_BASES) {}
        For i = 0 To number_of_samples - 1
            Dim current_base As Char = bases_for_snp(i)
            If current_base <> reference_base Then
                If GlobalMembersAlignmentMinusfile.is_unknown(current_base) <> 0 Then
                    ' VCF spec only allows ACGTN* for alts
                    current_base = "*"c
                End If

                If GlobalMembersVcf.check_if_char_in_string(alt_bases, current_base, num_alt_bases) = 0 Then
                    If num_alt_bases >= MAXIMUM_NUMBER_OF_ALT_BASES Then
                        App.StdErr.WriteLine("Unexpectedly large number of alternative bases found between sequences.  Please check input file is not corrupted" & vbLf & vbLf)
                        App.[Exit](1)
                    End If
                    alt_bases(num_alt_bases) = current_base
                    num_alt_bases += 1
                End If
            End If
        Next
        alt_bases(num_alt_bases) = ControlChars.NullChar
        Return alt_bases
    End Function

    Public Function format_alternative_bases(ByRef alt_bases As String) As String
        Dim number_of_alt_bases As Integer = alt_bases.Length
        Dim formatted_alt_bases As Char()

        Debug.Assert(number_of_alt_bases < DefineConstants.MAXIMUM_NUMBER_OF_ALT_BASES)

        If number_of_alt_bases = 0 Then
            formatted_alt_bases = New Char(2) {}
            formatted_alt_bases(0) = "."c
            Return formatted_alt_bases
        Else

            formatted_alt_bases = New Char(number_of_alt_bases * 2) {}
            Dim i As Integer
            formatted_alt_bases(0) = alt_bases(0)
            For i = 1 To number_of_alt_bases - 1
                formatted_alt_bases(i * 2 - 1) = ","c
                formatted_alt_bases(i * 2) = alt_bases(i)
            Next
            Return formatted_alt_bases
        End If
    End Function

    Public Function format_allele_index(base As Char, reference_base As Char, ByRef alt_bases As String) As String
        Dim length_of_alt_bases As Integer = alt_bases.Length
        Debug.Assert(length_of_alt_bases < 100)

        Dim result As SByte
        Dim index As Integer

        If GlobalMembersAlignmentMinusfile.is_unknown(base) <> 0 Then
            base = "*"c
        End If

        If reference_base = base Then
            result = "0"
        Else
            result = "."
            For index = 1 To length_of_alt_bases
                If alt_bases(index - 1) = base Then
                    result = String.Format("{0:D}", index)
                    Exit For
                End If
            Next
        End If
        Return result
    End Function

    Public Function check_if_char_in_string(ByRef search_string As String, target_char As Char, search_string_length As Integer) As Integer
        Dim i As Integer
        For i = 0 To search_string_length - 1
            If search_string(i) = target_char Then
                Return 1
            End If
        Next
        Return 0
    End Function
End Module