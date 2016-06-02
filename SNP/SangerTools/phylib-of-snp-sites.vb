Public NotInheritable Class GlobalMembersPhylibMinusofMinussnpMinussites
	Private Sub New()
	End Sub
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






	Public Shared Sub create_phylib_of_snp_sites(ByRef filename As String, number_of_snps As Integer, ByRef bases_for_snps As String(), ByRef sequence_names As String(), number_of_samples As Integer, output_reference As Integer, _
		ByRef pseudo_reference_sequence As String, snp_locations As Integer())
		Dim phylip_file_pointer As FILE
		Dim sample_counter As Integer
		Dim snp_counter As Integer

		phylip_file_pointer = fopen(filename, "w")

		If output_reference = 1 Then
			fprintf(phylip_file_pointer, "%d %d" & vbLf, number_of_samples + 1, number_of_snps)
			fprintf(phylip_file_pointer, "pseudo_reference_sequence" & vbTab)
			For snp_counter = 0 To number_of_snps - 1
				fputc(pseudo_reference_sequence(snp_locations(snp_counter)), phylip_file_pointer)
			Next
			fprintf(phylip_file_pointer, vbLf)
		Else
			fprintf(phylip_file_pointer, "%d %d" & vbLf, number_of_samples, number_of_snps)
		End If

		For sample_counter = 0 To number_of_samples - 1
			' sequence_name can be more than 10 (relaxed phylib format) and contain [\w\s]
			'TODO check for illegal characters [^\w\s]
			fprintf(phylip_file_pointer, "%s" & vbTab, sequence_names(sample_counter))

			For snp_counter = 0 To number_of_snps - 1
				fputc(bases_for_snps(snp_counter)(sample_counter), phylip_file_pointer)
			Next
			fprintf(phylip_file_pointer, vbLf)
		Next

		fclose(phylip_file_pointer)
	End Sub
End Class

