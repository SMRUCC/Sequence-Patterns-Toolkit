Public Class snpsites

    Friend Shared Function generate_snp_sites_generic(ByRef filename As String, output_multi_fasta_file As Integer, output_vcf_file As Integer, output_phylip_file As Integer, ByRef output_filename As String, output_reference As Integer,
        pure_mode As Integer, output_monomorphic As Integer) As Integer
        Dim i As Integer
        GlobalMembersAlignmentMinusfile.detect_snps(filename, pure_mode, output_monomorphic)

        Dim bases_for_snps As String() = New String(GlobalMembersAlignmentMinusfile.get_number_of_snps() - 1) {}

        For i = 0 To GlobalMembersAlignmentMinusfile.get_number_of_snps() - 1
            bases_for_snps(i) = calloc(GlobalMembersAlignmentMinusfile.get_number_of_samples() + 1, 1)
        Next

        GlobalMembersAlignmentMinusfile.get_bases_for_each_snp(filename, bases_for_snps)

        Dim output_filename_base As New String(New Char(FILENAME_MAX - 1) {})
        Dim filename_without_directory As New String(New Char(FILENAME_MAX - 1) {})
        GlobalMembersSnpMinussites.strip_directory_from_filename(filename, filename_without_directory)
        output_filename_base = filename_without_directory.Substring(0, FILENAME_MAX)

        If output_filename IsNot Nothing AndAlso output_filename <> ControlChars.NullChar Then
            output_filename_base = output_filename.Substring(0, FILENAME_MAX)
        End If

        If output_vcf_file <> 0 Then
            Dim vcf_output_filename As New String(New Char(FILENAME_MAX - 1) {})
            vcf_output_filename = output_filename_base.Substring(0, FILENAME_MAX)
            If (output_vcf_file + output_phylip_file + output_multi_fasta_file) > 1 OrElse (output_filename Is Nothing OrElse output_filename = ControlChars.NullChar) Then
                vcf_output_filename += ".vcf"
            End If

            GlobalMembersVcf.create_vcf_file(vcf_output_filename, GlobalMembersAlignmentMinusfile.get_snp_locations(), GlobalMembersAlignmentMinusfile.get_number_of_snps(), bases_for_snps, GlobalMembersAlignmentMinusfile.get_sequence_names(), GlobalMembersAlignmentMinusfile.get_number_of_samples(),
                GlobalMembersAlignmentMinusfile.get_length_of_genome(), GlobalMembersAlignmentMinusfile.get_pseudo_reference_sequence())
        End If


        If output_phylip_file <> 0 Then
            Dim phylip_output_filename As New String(New Char(FILENAME_MAX - 1) {})
            phylip_output_filename = output_filename_base.Substring(0, FILENAME_MAX)
            If (output_vcf_file + output_phylip_file + output_multi_fasta_file) > 1 OrElse (output_filename Is Nothing OrElse output_filename = ControlChars.NullChar) Then
                phylip_output_filename += ".phylip"
            End If
            GlobalMembersPhylibMinusofMinussnpMinussites.create_phylib_of_snp_sites(phylip_output_filename, GlobalMembersAlignmentMinusfile.get_number_of_snps(), bases_for_snps, GlobalMembersAlignmentMinusfile.get_sequence_names(), GlobalMembersAlignmentMinusfile.get_number_of_samples(), output_reference,
                GlobalMembersAlignmentMinusfile.get_pseudo_reference_sequence(), GlobalMembersAlignmentMinusfile.get_snp_locations())
        End If

        If (output_multi_fasta_file) OrElse (output_vcf_file = 0 AndAlso output_phylip_file = 0 AndAlso output_multi_fasta_file = 0) Then
            Dim multi_fasta_output_filename As New String(New Char(FILENAME_MAX - 1) {})
            multi_fasta_output_filename = output_filename_base.Substring(0, FILENAME_MAX)
            If (output_vcf_file + output_phylip_file + output_multi_fasta_file) > 1 OrElse (output_filename Is Nothing OrElse output_filename = ControlChars.NullChar) Then
                multi_fasta_output_filename += ".snp_sites.aln"
            End If
            GlobalMembersFastaMinusofMinussnpMinussites.create_fasta_of_snp_sites(multi_fasta_output_filename, GlobalMembersAlignmentMinusfile.get_number_of_snps(), bases_for_snps, GlobalMembersAlignmentMinusfile.get_sequence_names(), GlobalMembersAlignmentMinusfile.get_number_of_samples(), output_reference,
                GlobalMembersAlignmentMinusfile.get_pseudo_reference_sequence(), GlobalMembersAlignmentMinusfile.get_snp_locations())
        End If

        Return 1
    End Function

    Public Shared Function generate_snp_sites(ByRef filename As String, output_multi_fasta_file As Integer, output_vcf_file As Integer, output_phylip_file As Integer, ByRef output_filename As String) As Integer
        Return GlobalMembersSnpMinussites.generate_snp_sites_generic(filename, output_multi_fasta_file, output_vcf_file, output_phylip_file, output_filename, 0,
            0, 0)
    End Function

    Public Shared Function generate_snp_sites_with_ref(ByRef filename As String, output_multi_fasta_file As Integer, output_vcf_file As Integer, output_phylip_file As Integer, ByRef output_filename As String) As Integer
        Return GlobalMembersSnpMinussites.generate_snp_sites_generic(filename, output_multi_fasta_file, output_vcf_file, output_phylip_file, output_filename, 1,
            0, 0)
    End Function

    Public Shared Function generate_snp_sites_with_ref_pure_mono(ByRef filename As String, output_multi_fasta_file As Integer, output_vcf_file As Integer, output_phylip_file As Integer, ByRef output_filename As String, output_reference As Integer,
        pure_mode As Integer, output_monomorphic As Integer) As Integer
        Return GlobalMembersSnpMinussites.generate_snp_sites_generic(filename, output_multi_fasta_file, output_vcf_file, output_phylip_file, output_filename, output_reference,
            pure_mode, output_monomorphic)
    End Function


    ' Inefficient
    Public Shared Sub strip_directory_from_filename(ByRef input_filename As String, ByRef output_filename As String)
        Dim i As Integer
        Dim end_index As Integer = 0
        Dim last_forward_slash_index As Integer = -1
        For i = 0 To FILENAME_MAX - 1
            If input_filename(i) = "/"c Then
                last_forward_slash_index = i
            End If

            If input_filename(i) = ControlChars.NullChar OrElse input_filename(i) = ControlChars.Lf Then
                end_index = i
                Exit For
            End If
        Next

        Dim current_index As Integer = 0
        For i = last_forward_slash_index + 1 To end_index - 1
            output_filename(current_index) = input_filename(i)
            current_index += 1
        Next
        output_filename(current_index) = ControlChars.NullChar
    End Sub
End Class

