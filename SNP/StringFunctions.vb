'----------------------------------------------------------------------------------------
'	Copyright © 2006 - 2012 Tangible Software Solutions Inc.
'	This class can be used by anyone provided that the copyright notice remains intact.
'
'	This class provides the ability to simulate various classic C string functions
'	which don't have exact equivalents in the .NET Framework.
'----------------------------------------------------------------------------------------
Friend NotInheritable Class StringFunctions
	Private Sub New()
	End Sub
	'------------------------------------------------------------------------------------
	'	This method allows replacing a single character in a string, to help convert
	'	C++ code where a single character in a character array is replaced.
	'------------------------------------------------------------------------------------
	Friend Shared Function ChangeCharacter(sourcestring As String, charindex As Integer, changechar As Char) As String
		Return (If(charindex > 0, sourcestring.Substring(0, charindex), "")) & changechar.ToString() & (If(charindex < sourcestring.Length - 1, sourcestring.Substring(charindex + 1), ""))
	End Function

	'------------------------------------------------------------------------------------
	'	This method simulates the classic C string function 'isxdigit' (and 'iswxdigit').
	'------------------------------------------------------------------------------------
	Friend Shared Function IsXDigit(character As Char) As Boolean
		If Char.IsDigit(character) Then
			Return True
		ElseIf "ABCDEFabcdef".IndexOf(character) > -1 Then
			Return True
		Else
			Return False
		End If
	End Function

	'------------------------------------------------------------------------------------
	'	This method simulates the classic C string function 'strchr' (and 'wcschr').
	'------------------------------------------------------------------------------------
	Friend Shared Function StrChr(stringtosearch As String, chartofind As Char) As String
		Dim index As Integer = stringtosearch.IndexOf(chartofind)
		If index > -1 Then
			Return stringtosearch.Substring(index)
		Else
			Return Nothing
		End If
	End Function

	'------------------------------------------------------------------------------------
	'	This method simulates the classic C string function 'strrchr' (and 'wcsrchr').
	'------------------------------------------------------------------------------------
	Friend Shared Function StrRChr(stringtosearch As String, chartofind As Char) As String
		Dim index As Integer = stringtosearch.LastIndexOf(chartofind)
		If index > -1 Then
			Return stringtosearch.Substring(index)
		Else
			Return Nothing
		End If
	End Function

	'------------------------------------------------------------------------------------
	'	This method simulates the classic C string function 'strstr' (and 'wcsstr').
	'------------------------------------------------------------------------------------
	Friend Shared Function StrStr(stringtosearch As String, stringtofind As String) As String
		Dim index As Integer = stringtosearch.IndexOf(stringtofind)
		If index > -1 Then
			Return stringtosearch.Substring(index)
		Else
			Return Nothing
		End If
	End Function

	'------------------------------------------------------------------------------------
	'	This method simulates the classic C string function 'strtok' (and 'wcstok').
	'	Note that the .NET string 'Split' method cannot be used to simulate 'strtok' since
	'	it doesn't allow changing the delimiters between each token retrieval.
	'------------------------------------------------------------------------------------
	Private Shared activestring As String
	Private Shared activeposition As Integer
	Friend Shared Function StrTok(stringtotokenize As String, delimiters As String) As String
		If stringtotokenize IsNot Nothing Then
			activestring = stringtotokenize
			activeposition = -1
		End If

		'the stringtotokenize was never set:
		If activestring Is Nothing Then
			Return Nothing
		End If

		'all tokens have already been extracted:
		If activeposition = activestring.Length Then
			Return Nothing
		End If

		'bypass delimiters:
		activeposition += 1
		While activeposition < activestring.Length AndAlso delimiters.IndexOf(activestring(activeposition)) > -1
			activeposition += 1
		End While

		'only delimiters were left, so return null:
		If activeposition = activestring.Length Then
			Return Nothing
		End If

		'get starting position of string to return:
		Dim startingposition As Integer = activeposition

		'read until next delimiter:
		Do
			activeposition += 1
		Loop While activeposition < activestring.Length AndAlso delimiters.IndexOf(activestring(activeposition)) = -1

		Return activestring.Substring(startingposition, activeposition - startingposition)
	End Function
End Class
