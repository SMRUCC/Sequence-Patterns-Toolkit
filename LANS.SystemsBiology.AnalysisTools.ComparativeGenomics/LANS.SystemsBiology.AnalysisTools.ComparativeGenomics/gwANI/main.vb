
'Public NotInheritable Class GlobalMembersMain
'    Private Sub New()
'    End Sub
'    '
'    '     *  Wellcome Trust Sanger Institute
'    '     *  Copyright (C) 2016  Wellcome Trust Sanger Institute
'    '     *  
'    '     *  This program is free software; you can redistribute it and/or
'    '     *  modify it under the terms of the GNU General Public License
'    '     *  as published by the Free Software Foundation; either version 3
'    '     *  of the License, or (at your option) any later version.
'    '     *  
'    '     *  This program is distributed in the hope that it will be useful,
'    '     *  but WITHOUT ANY WARRANTY; without even the implied warranty of
'    '     *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    '     *  GNU General Public License for more details.
'    '     *  
'    '     *  You should have received a copy of the GNU General Public License
'    '     *  along with this program; if not, write to the Free Software
'    '     *  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
'    '     


'    Friend Shared Sub print_usage()
'        Console.Write("Usage: panito [-hV] <file>" & vbLf)
'        Console.Write("This program calculates the genome wide ANI for a multiFASTA alignment." & vbLf)
'        Console.Write(" -h" & vbTab & vbTab & "this help message" & vbLf)
'        Console.Write(" -V" & vbTab & vbTab & "print version and exit" & vbLf)
'        Console.Write(" <file>" & vbTab & vbTab & "input alignment file which can optionally be gzipped" & vbLf)
'    End Sub

'    Friend Shared Sub print_version()
'        Console.Write("{0} {1}" & vbLf, DefineConstants.PROGRAM_NAME, PACKAGE_VERSION)
'    End Sub

'    Private Shared Sub Main(argc As Integer, args As String())
'        Dim multi_fasta_filename As String() = {""}
'        Dim output_filename As String() = {""}

'        Dim c As Integer
'        Dim index As Integer
'        Dim output_multi_fasta_file As Integer = 0

'        While (InlineAssignHelper(c, getopt(argc, args, "ho:V"))) <> -1
'            Select Case c
'                Case "V"c
'                    GlobalMembersMain.print_version()
'            
'                Case "o"c
'                    output_filename = optarg.Substring(0, FILENAME_MAX)
'                    Exit Select
'                Case "h"c
'                    GlobalMembersMain.print_usage()
'                Case Else
'                 '                    output_multi_fasta_file = 1
'                    Exit Select
'            End Select
'        End While

'        If optind < argc Then
'            multi_fasta_filename = Convert.ToString(args(optind)).Substring(0, FILENAME_MAX)
'            'calculate_and_output_gwani(multi_fasta_filename);
'            GlobalMembersGwani.fast_calculate_gwani(multi_fasta_filename)
'        Else
'            GlobalMembersMain.print_usage()
'        End If

'    End Sub
'End Class

