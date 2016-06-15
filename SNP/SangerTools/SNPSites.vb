Imports System.Runtime.CompilerServices
Imports LANS.SystemsBiology.SequenceModel.FASTA
Imports Microsoft.VisualBasic.Linq

''' <summary>
''' SNP functions entry point at here
''' </summary>
Public Module SNPSites

    Private Function __snpSitesGeneric(ByRef filename As String,
                                       output_multi_fasta_file As Integer,
                                       output_vcf_file As Integer,
                                       output_phylip_file As Integer,
                                       ByRef output_filename As String,
                                       output_reference As Integer,
                                       pure_mode As Integer,
                                       output_monomorphic As Integer) As Integer

        Return New FastaFile(filename).SNPSitesGeneric(
            output_multi_fasta_file,
            output_vcf_file,
            output_phylip_file,
            output_filename,
            output_reference,
            pure_mode,
            output_monomorphic)
    End Function

    <Extension>
    Private Function SNPSitesGeneric(fasta As FastaFile,
                                     output_multi_fasta_file As Integer,
                                     output_vcf_file As Integer,
                                     output_phylip_file As Integer,
                                     ByRef output_filename As String,
                                     output_reference As Integer,
                                     pure_mode As Integer,
                                     output_monomorphic As Integer) As Integer

        AlignmentMinusfile.DetectSNPs(fasta, pure_mode, output_monomorphic)

        Dim bases_for_snps As Char()() = New Char(AlignmentMinusfile.number_of_snps - 1)() {}

        AlignmentMinusfile.GetBasesForEachSNP(fasta, bases_for_snps)

        Dim output_filename_base As New String(New Char(FILENAME_MAX - 1) {})
        Dim filename_without_directory As New String(New Char(FILENAME_MAX - 1) {})

        output_filename_base = filename_without_directory.Substring(0, FILENAME_MAX)

        If output_filename IsNot Nothing AndAlso output_filename <> ControlChars.NullChar Then
            output_filename_base = output_filename.Substring(0, FILENAME_MAX)
        End If

        Dim SNPsBases As String() = bases_for_snps.ToArray(Function(x) New String(x))

        If output_vcf_file <> 0 Then
            Dim vcf_output_filename As New String(New Char(FILENAME_MAX - 1) {})
            vcf_output_filename = output_filename_base.Substring(0, FILENAME_MAX)
            If (output_vcf_file + output_phylip_file + output_multi_fasta_file) > 1 OrElse (output_filename Is Nothing OrElse output_filename = ControlChars.NullChar) Then
                vcf_output_filename += ".vcf"
            End If

            Vcf.create_vcf_file(vcf_output_filename,
                                AlignmentMinusfile.snp_locations,
                                AlignmentMinusfile.number_of_snps,
                                SNPsBases,
                                AlignmentMinusfile.sequence_names,
                                AlignmentMinusfile.number_of_samples,
                                AlignmentMinusfile.length_of_genome,
                                AlignmentMinusfile.pseudo_reference_sequence)
        End If


        If output_phylip_file <> 0 Then
            Dim phylip_output_filename As New String(New Char(FILENAME_MAX - 1) {})
            phylip_output_filename = output_filename_base.Substring(0, FILENAME_MAX)
            If (output_vcf_file + output_phylip_file + output_multi_fasta_file) > 1 OrElse (output_filename Is Nothing OrElse output_filename = ControlChars.NullChar) Then
                phylip_output_filename += ".phylip"
            End If

            PhylibMinusofMinussnpMinussites.PhylibOfSNPSites(
                phylip_output_filename,
                AlignmentMinusfile.number_of_snps,
                SNPsBases,
                AlignmentMinusfile.sequence_names,
                AlignmentMinusfile.number_of_samples,
                output_reference,
                AlignmentMinusfile.pseudo_reference_sequence,
                AlignmentMinusfile.snp_locations)
        End If

        If (output_multi_fasta_file) OrElse (output_vcf_file = 0 AndAlso output_phylip_file = 0 AndAlso output_multi_fasta_file = 0) Then
            Dim multi_fasta_output_filename As New String(New Char(FILENAME_MAX - 1) {})
            multi_fasta_output_filename = output_filename_base.Substring(0, FILENAME_MAX)
            If (output_vcf_file + output_phylip_file + output_multi_fasta_file) > 1 OrElse (output_filename Is Nothing OrElse output_filename = ControlChars.NullChar) Then
                multi_fasta_output_filename += ".snp_sites.aln"
            End If

            FastaMinusofMinussnpMinussites.CreateFastaOfSNPSites(
                multi_fasta_output_filename,
                AlignmentMinusfile.number_of_snps,
                SNPsBases,
                AlignmentMinusfile.sequence_names,
                AlignmentMinusfile.number_of_samples,
                output_reference,
                AlignmentMinusfile.pseudo_reference_sequence,
                AlignmentMinusfile.snp_locations)
        End If

        Return 1
    End Function

    ''' <summary>
    ''' SNPs from the input alignment fasta file
    ''' </summary>
    ''' <param name="filename">input alignment file</param>
    ''' <param name="output_multi_fasta_file"></param>
    ''' <param name="output_vcf_file"></param>
    ''' <param name="output_phylip_file"></param>
    ''' <param name="output_filename"></param>
    ''' <returns></returns>
    Public Function OutputSNPSites(ByRef filename As String,
                                   output_multi_fasta_file As Integer,
                                   output_vcf_file As Integer,
                                   output_phylip_file As Integer,
                                   ByRef output_filename As String) As Integer

        Return __snpSitesGeneric(filename,
                                 output_multi_fasta_file,
                                 output_vcf_file,
                                 output_phylip_file,
                                 output_filename, 0, 0, 0)
    End Function

    ''' <summary>
    ''' SNPs from the input alignment fasta file
    ''' </summary>
    ''' <param name="filename">input alignment file</param>
    ''' <param name="output_multi_fasta_file"></param>
    ''' <param name="output_vcf_file"></param>
    ''' <param name="output_phylip_file"></param>
    ''' <param name="output_filename"></param>
    ''' <returns></returns>
    Public Function OutputSNPSitesWithRef(ByRef filename As String,
                                          output_multi_fasta_file As Integer,
                                          output_vcf_file As Integer,
                                          output_phylip_file As Integer,
                                          ByRef output_filename As String) As Integer

        Return __snpSitesGeneric(filename,
                                 output_multi_fasta_file,
                                 output_vcf_file,
                                 output_phylip_file,
                                 output_filename, 1, 0, 0)
    End Function

    ''' <summary>
    ''' SNPs from the input alignment fasta file
    ''' </summary>
    ''' <param name="filename">input alignment file</param>
    ''' <param name="output_multi_fasta_file"></param>
    ''' <param name="output_vcf_file"></param>
    ''' <param name="output_phylip_file"></param>
    ''' <param name="output_filename"></param>
    ''' <param name="output_reference"></param>
    ''' <param name="pure_mode"></param>
    ''' <param name="output_monomorphic"></param>
    ''' <returns></returns>
    Public Function OutputSNPSitesWithRefPureMono(ByRef filename As String,
                                                  output_multi_fasta_file As Integer,
                                                  output_vcf_file As Integer,
                                                  output_phylip_file As Integer,
                                                  ByRef output_filename As String,
                                                  output_reference As Integer,
                                                  pure_mode As Integer,
                                                  output_monomorphic As Integer) As Integer

        Return __snpSitesGeneric(filename,
                                 output_multi_fasta_file,
                                 output_vcf_file,
                                 output_phylip_file,
                                 output_filename,
                                 output_reference,
                                 pure_mode,
                                 output_monomorphic)
    End Function
End Module

