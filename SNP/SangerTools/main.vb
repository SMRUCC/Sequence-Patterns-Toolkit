
'Public NotInheritable Class GlobalMembersMain
'	Private Sub New()
'	End Sub
'    '
'    '	 *  Wellcome Trust Sanger Institute
'    '	 *  Copyright (C) 2013  Wellcome Trust Sanger Institute
'    '	 *  
'    '	 *  This program is free software; you can redistribute it and/or
'    '	 *  modify it under the terms of the GNU General Public License
'    '	 *  as published by the Free Software Foundation; either version 3
'    '	 *  of the License, or (at your option) any later version.
'    '	 *  
'    '	 *  This program is distributed in the hope that it will be useful,
'    '	 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
'    '	 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    '	 *  GNU General Public License for more details.
'    '	 *  
'    '	 *  You should have received a copy of the GNU General Public License
'    '	 *  along with this program; if not, write to the Free Software
'    '	 *  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
'    '	 


'    Friend Shared Sub print_usage()
'		Console.Write("Usage: snp-sites [-rmvpcbhV] [-o output_filename] <file>" & vbLf)
'		Console.Write("This program finds snp sites from a multi FASTA alignment file." & vbLf)
'		Console.Write(" -r     output internal pseudo reference sequence" & vbLf)
'		Console.Write(" -m     output a multi fasta alignment file (default)" & vbLf)
'		Console.Write(" -v     output a VCF file" & vbLf)
'		Console.Write(" -p     output a phylip file" & vbLf)
'		Console.Write(" -o STR specify an output filename" & vbLf)
'		Console.Write(" -c     only output columns containing exclusively ACGT" & vbLf)
'		Console.Write(" -b     output monomorphic sites, used for BEAST" & vbLf)
'		Console.Write(" -h     this help message" & vbLf)
'		Console.Write(" -V     print version and exit" & vbLf)
'		Console.Write(" <file> input alignment file which can optionally be gzipped" & vbLf & vbLf)

'		Console.Write("Example: creating files for BEAST" & vbLf)
'		Console.Write(" snp-sites -cb -o outputfile.aln inputfile.aln" & vbLf & vbLf)

'		Console.Write("If you use this program, please cite:" & vbLf)
'		Console.Write("""SNP-sites: rapid efficient extraction of SNPs from multi-FASTA alignments""," & vbLf)
'		Console.Write("Andrew J. Page, Ben Taylor, Aidan J. Delaney, Jorge Soares, Torsten Seemann, Jacqueline A. Keane, Simon R. Harris," & vbLf)
'		Console.Write("Microbial Genomics 2(4), (2016). http://dx.doi.org/10.1099/mgen.0.000056" & vbLf)

'	End Sub

'	Friend Shared Sub print_version()
'        'Console.Write("{0} {1}" & vbLf, DefineConstants.PROGRAM_NAME, PACKAGE_VERSION)
'    End Sub

'	Private Shared Sub Main(argc As Integer, args As String())
'		Dim multi_fasta_filename As String() = {""}
'		Dim output_filename As String() = {""}

'		Dim c As Integer
'		Dim index As Integer
'		Dim output_multi_fasta_file As Integer = 0
'		Dim output_vcf_file As Integer = 0
'		Dim output_phylip_file As Integer = 0
'		Dim output_reference As Integer = 0
'		Dim pure_mode As Integer = 0
'		Dim output_monomorphic As Integer = 0

'		While (InlineAssignHelper(c, getopt(argc, args, "mvrbpco:V"))) <> -1
'			Select Case c
'				Case "m"C
'					output_multi_fasta_file = 1
'					Exit Select
'				Case "v"C
'					output_vcf_file = 1
'					Exit Select
'				Case "V"C
'					GlobalMembersMain.print_version()

'                Case "p"C
'					output_phylip_file = 1
'					Exit Select
'				Case "r"C
'					output_reference = 1
'					Exit Select
'				Case "c"C
'					pure_mode = 1
'					Exit Select
'				Case "b"C
'					output_monomorphic = 1
'					Exit Select
'				Case "o"C
'					output_filename = optarg.Substring(0, FILENAME_MAX)
'					Exit Select
'				Case "h"C
'					GlobalMembersMain.print_usage()
'					Environment.[Exit](0)
'				Case Else

'                    output_multi_fasta_file = 1
'					Exit Select
'			End Select
'		End While


'		If optind < argc Then
'			' check to see if the input alignment file exists
'			If access(args(optind), F_OK) = -1 Then
'				fprintf(stderr, "ERROR: cannot access input alignment file '%s'" & vbLf, args(optind))
'				fflush(stderr)
'				Environment.[Exit](1)
'			End If

'			multi_fasta_filename = Convert.ToString(args(optind)).Substring(0, FILENAME_MAX)

'			If pure_mode <> 0 OrElse output_monomorphic <> 0 Then
'				GlobalMembersSnpMinussites.generate_snp_sites_with_ref_pure_mono(multi_fasta_filename, output_multi_fasta_file, output_vcf_file, output_phylip_file, output_filename, output_reference, _
'					pure_mode, output_monomorphic)
'			ElseIf output_reference <> 0 Then
'				GlobalMembersSnpMinussites.generate_snp_sites_with_ref(multi_fasta_filename, output_multi_fasta_file, output_vcf_file, output_phylip_file, output_filename)
'			Else
'				GlobalMembersSnpMinussites.generate_snp_sites(multi_fasta_filename, output_multi_fasta_file, output_vcf_file, output_phylip_file, output_filename)
'			End If
'		Else
'			GlobalMembersMain.print_usage()
'		End If

'		Environment.[Exit](0)
'	End Sub
'End Class

