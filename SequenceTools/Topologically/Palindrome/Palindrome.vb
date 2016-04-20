Imports LANS.SystemsBiology.SequenceModel
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.DocumentFormat.Csv
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Microsoft.VisualBasic.Linq.Extensions
Imports Microsoft.VisualBasic
Imports LANS.SystemsBiology.SequenceModel.NucleotideModels

Namespace Topologically

    ''' <summary>
    ''' 回文结构
    ''' </summary>
    ''' 
    <PackageNamespace("Palindrome.Search", Publisher:="xie.guigang@gcmodeller.org", Url:="http://gcmodeller.org")>
    Public Module Palindrome

        <ExportAPI("Palindrome.Vector")>
        Public Function PalindromeLociVector(DIR As String, Length As Integer) As Double()
            Return Density(Of PalindromeLoci)(DIR, size:=Length)
        End Function

        <ExportAPI("ImperfectPalindrome.Vector")>
        Public Function ImperfectPalindromeVector(DIR As String, length As Integer) As Double()
            Return Density(Of ImperfectPalindrome)(DIR, size:=length)
        End Function

        <ExportAPI("ImperfectPalindrome.Vector.TRIM")>
        Public Function ImperfectPalindromeVector(DIR As String, length As Integer, min As Integer, max As Integer) As Double()
            Call $"Start loading original data from {DIR}".__DEBUG_ECHO
            Dim files = (From file As String
                         In FileIO.FileSystem.GetFiles(DIR, FileIO.SearchOption.SearchTopLevelOnly, "*.csv")
                         Select file.LoadCsv(Of ImperfectPalindrome)).ToArray
            Call $"Data load done! Start to filter data...".__DEBUG_ECHO
            files = (From genome In files.AsParallel
                     Select (From site As ImperfectPalindrome
                             In genome
                             Where site.MaxMatch >= min AndAlso
                                 site.MaxMatch <= max AndAlso
                                 site.Palindrome.Count("-"c) <> site.Palindrome.Length AndAlso
                                 site.Site.Count("-"c) <> site.Site.Length
                             Select site).ToList).ToArray
            Call $"Generates density vector....".__DEBUG_ECHO
            Return Density(Of ImperfectPalindrome)(files, size:=length)
        End Function

        Public Function ToVector(Of TSite As Contig)(sites As IEnumerable(Of TSite), size As Integer) As Integer()
            Dim LQuery = (From i As Integer
                          In size.Sequence
                          Select (From site As TSite
                                  In sites
                                  Where site.MappingLocation.ContainSite(i)
                                  Select 1).FirstOrDefault).ToArray
            Call Console.Write(".")
            Return LQuery
        End Function

        Public Function Density(Of TView As Contig)(DIR As String, size As Integer) As Double()
            Dim files = (From file As String
                         In FileIO.FileSystem.GetFiles(DIR, FileIO.SearchOption.SearchTopLevelOnly, "*.csv")
                         Select file.LoadCsv(Of TView)).ToArray
            Return Density(files, size)
        End Function

        Public Function Density(Of TView As Contig)(genomes As IEnumerable(Of IEnumerable(Of TView)), size As Integer) As Double()
            Dim Vecotrs = (From genome As IEnumerable(Of TView)
                           In genomes.AsParallel
                           Select vector = ToVector(genome, size)).ToArray

            Call New String("="c, 120).__DEBUG_ECHO
            Call $"genomes={Vecotrs.Count}".__DEBUG_ECHO

            Dim p_vectors As Double() = size.ToArray(Function(index As Integer) As Double
                                                         Dim site As Integer() = Vecotrs.ToArray(Function(genome) genome(index))
                                                         Dim hashRepeats = (From g As Double In site.AsParallel Where g > 0 Select g).ToArray
                                                         Dim pHas As Double = hashRepeats.Length / site.Length
                                                         Return pHas
                                                     End Function)
            Return p_vectors
        End Function

        ''' <summary>
        ''' Have mirror repeats?
        ''' </summary>
        ''' <param name="Segment"></param>
        ''' <param name="Sequence"></param>
        ''' <returns></returns>
        ''' 
        <ExportAPI("HasMirror?")>
        Public Function HaveMirror(Segment As String, Sequence As String) As Boolean
            Dim Locations = SequenceTools.FindLocation(Sequence, Segment)
            If Locations.IsNullOrEmpty Then
                Return False
            End If

            Dim Mirror As String = New String(Segment.Reverse.ToArray)
            Dim l As Integer = Len(Segment)
            Dim Result = (From loci As Integer
                          In Locations
                          Let ml As Integer = __haveMirror(l, loci, Mirror, Sequence)
                          Where ml > -1
                          Select ml).ToArray
            Return Not Result.IsNullOrEmpty
        End Function

        Private Function __haveMirror(l As Integer, Loci As Integer, Mirror As String, Sequence As String) As Integer
            Dim mrStart As Integer = Loci + l
            Dim mMirr As String = Mid(Sequence, mrStart, l)
            If String.Equals(mMirr, Mirror) Then
                Return mrStart + l
            Else
                Return -1
            End If
        End Function

        <ExportAPI("Mirrors.Locis.Get")>
        Public Function CreateMirrors(Segment As String, Sequence As String) As PalindromeLoci()
            Dim Locations = SequenceTools.FindLocation(Sequence, Segment)
            If Locations.IsNullOrEmpty Then
                Return Nothing
            End If

            Dim Mirror As String = New String(Segment.Reverse.ToArray)
            Dim l As Integer = Len(Segment)
            Dim Result = (From loci As Integer In Locations
                          Let ml As Integer = __haveMirror(l, loci, Mirror, Sequence)
                          Where ml > -1
                          Select loci, ml).ToArray
            Return Result.ToArray(Function(site) New PalindromeLoci With {
                                      .Loci = Segment,
                                      .Start = site.loci,
                                      .PalEnd = site.ml,
                                      .Palindrome = Mirror,
                                      .MirrorSite = Mirror})
        End Function

        <ExportAPI("Palindrome.Locis.Get")>
        Public Function CreatePalindrome(Segment As String, Sequence As String) As PalindromeLoci()
            Dim Locations = SequenceTools.FindLocation(Sequence, Segment)
            If Locations.IsNullOrEmpty Then
                Return Nothing
            End If

            Dim rev As String = New String(Segment.Reverse.ToArray)
            Dim Mirror As String = NucleicAcid.Complement(rev)
            Dim l As Integer = Len(Segment)
            Dim Result = (From loci As Integer In Locations
                          Let ml As Integer = __haveMirror(l, loci, Mirror, Sequence)
                          Where ml > -1
                          Select loci, ml).ToArray
            Return Result.ToArray(Function(site) New PalindromeLoci With {
                                      .Loci = Segment,
                                      .Start = site.loci,
                                      .PalEnd = site.ml,
                                      .Palindrome = Mirror,
                                      .MirrorSite = rev})
        End Function

        <ExportAPI("Search.Mirror")>
        Public Function SearchMirror(Sequence As I_PolymerSequenceModel,
                                     Optional Min As Integer = 3,
                                     Optional Max As Integer = 20) As PalindromeLoci()
            Dim search As New Topologically.MiroorSearchs(Sequence, Min, Max)
            Call search.InvokeSearch()
            Return search.ResultSet.ToArray
        End Function

        <ExportAPI("Search.Palindrome")>
        Public Function SearchPalindrome(Sequence As I_PolymerSequenceModel,
                                     Optional Min As Integer = 3,
                                     Optional Max As Integer = 20) As PalindromeLoci()
            Dim search As New Topologically.PalindromeSearchs(Sequence, Min, Max)
            Call search.InvokeSearch()
            Return search.ResultSet.ToArray
        End Function

        <ExportAPI("Write.Csv.PalindromeLocis")>
        Public Function SaveResultSet(rs As Generic.IEnumerable(Of PalindromeSearchs), SaveTo As String) As Boolean
            Return rs.SaveTo(SaveTo)
        End Function

        ''' <summary>
        ''' Have Palindrome repeats?
        ''' </summary>
        ''' <param name="Segment"></param>
        ''' <param name="Sequence"></param>
        ''' <returns></returns>
        ''' 
        <ExportAPI("HasPalindrome?")>
        Public Function HavePalindrome(Segment As String, Sequence As String) As Boolean
            Dim Locations = SequenceTools.FindLocation(Sequence, Segment)
            If Locations.IsNullOrEmpty Then
                Return False
            End If

            Dim Mirror As String = NucleicAcid.Complement(New String(Segment.Reverse.ToArray))
            Dim l As Integer = Len(Segment)
            Dim Result = (From loci As Integer
                        In Locations
                          Let ml As Integer = __haveMirror(l, loci, Mirror, Sequence)
                          Where ml > -1
                          Select ml).ToArray
            Return Not Result.IsNullOrEmpty
        End Function
    End Module
End Namespace