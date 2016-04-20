Imports LANS.SystemsBiology.SequenceModel
Imports LANS.SystemsBiology.SequenceModel.NucleotideModels
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.DocumentFormat.Csv.Extensions
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Microsoft.VisualBasic.Text.Similarity
Imports Microsoft.VisualBasic

Namespace Topologically.SimilarityMatches

    ''' <summary>
    ''' 模糊匹配重复的序列片段
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    <[PackageNamespace]("Seqtools.Repeats.Search", Publisher:="xie.guigang@gmail.com", Url:="http://gcmodeller.org")>
    Public Module Repeats

        ''' <summary>
        ''' 模糊匹配相似的位点在目标序列之上的位置
        ''' </summary>
        ''' <param name="Sequence"></param>
        ''' <param name="Loci"></param>
        ''' <param name="Min"></param>
        ''' <param name="Max"></param>
        ''' <param name="cutoff"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' 
        <ExportAPI("Search.Loci.Repeats")>
        Public Function MatchLociLocations(Sequence As String, Loci As String, Min As Integer, Max As Integer, Optional cutoff As Double = 0.65) As LociMatchedResult()
            Dim Chars As Char() = (From c As Char In Sequence.ToUpper.ShadowCopy(Sequence) Select c Distinct).ToArray
            Dim initSeeds = (From obj In __generateSeeds(Chars, Loci, cutoff, Min, Max)
                             Select obj
                             Group By obj.Key Into Group)
            Dim SeedsData = (From seed In initSeeds
                             Select seed.Key,
                                 seed.Group.First.Value) _
                                   .ToDictionary(Function(obj) obj.Key,
                                                 Function(obj) obj.Value)
            Dim Seeds = (From obj In SeedsData Select obj.Key).ToArray
            Dim Repeats As LociMatchedResult() = (From lociSegment As LociMatchedResult
                                                  In __matchLociLocation(Sequence, Seeds)
                                                  Let Score As Double = SeedsData(lociSegment.Matched)
                                                  Select lociSegment.InvokeSet(NameOf(lociSegment.Similarity), Score).InvokeSet(NameOf(lociSegment.Loci), Loci)).ToArray
            Return Repeats
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Sequence"></param>
        ''' <param name="seeds">为了加快计算效率，事先所生成的种子缓存</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function __matchLociLocation(Sequence As String, seeds As String()) As LociMatchedResult()
            Dim LQuery = (From s As String In seeds
                          Let Location = ShellScriptAPI.FindLocation(Sequence, s)
                          Select New LociMatchedResult With {
                              .Matched = s,
                              .Location = Location}).ToArray
            Return LQuery
        End Function

        Private Function __generateSeeds(Chars As Char(),
                                         Loci As String,
                                         Cutoff As Double,
                                         Min As Integer,
                                         Max As Integer) As KeyValuePair(Of String, Double)()
            If Min < 6 Then
                Cutoff = 0.3
            End If

            Dim Seeds As List(Of String) = SequenceTools.Topologically.InitializeSeeds(Chars, Min)
            Dim SeedsCollection = (From s As String In Seeds
                                   Let Score As Double = New StringSimilarityMatchs(s, Loci).Score
                                   Where Score >= Cutoff
                                   Select s,
                                       Score).ToList '生成初始长度的种子
            Dim TempChunk As List(Of String)
            'Seeds = (From obj In SeedsCollection Select obj.s).ToList

            For i As Integer = Min + 1 To Max   '种子延伸至长度的上限
                TempChunk = SequenceTools.Topologically.ExtendSequence(Seeds, Chars)
                Dim ChunkBuffer = (From s As String In TempChunk.AsParallel Let Score As Double = New StringSimilarityMatchs(Loci, s).Score Where Score >= Cutoff Select s, Score).ToArray
                TempChunk = (From obj In ChunkBuffer Select obj.s).ToList

                Call Console.Write(".")

                Call Seeds.AddRange(TempChunk)
                Call SeedsCollection.AddRange(ChunkBuffer)
            Next

            Call Console.WriteLine("Seeds generation thread for        {0}       job done!", Loci)
            Return (From obj In SeedsCollection.AsParallel Select New KeyValuePair(Of String, Double)(obj.s, obj.Score)).ToArray
        End Function

        ''' <summary>
        ''' 生成和<paramref name="Loci"></paramref>满足相似度匹配关系的序列的集合
        ''' </summary>
        ''' <param name="Chars"></param>
        ''' <param name="Loci"></param>
        ''' <param name="Cutoff"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function __generateSeeds(Chars As Char(), Loci As String, Cutoff As Double) As KeyValuePair(Of String, Double)()
            Dim Min As Integer = Len(Loci), Max As Integer = Len(Loci) * 2
            Return __generateSeeds(Chars, Loci, Cutoff, Min, Max)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="SequenceData"></param>
        ''' <param name="Min"></param>
        ''' <param name="Max"></param>
        ''' <param name="cutoff"></param>
        ''' <returns></returns>
        ''' <remarks>为了加快计算，首先生成种子，然后再对种子进行模糊匹配</remarks>
        ''' 
        <ExportAPI("invoke.search.similarity")>
        Public Function InvokeSearch(SequenceData As String, Min As Integer, Max As Integer, Optional cutoff As Double = 0.65) As LociMatchedResult()
            SequenceData = SequenceData.ToUpper

            Call Console.WriteLine("Start to generate seeds....")
            Dim Seeds = (From rp In SearchRepeats(New NucleotideModels.SegmentObject(SequenceData, 1), Min, Max) Select seq = rp.SequenceData Distinct).ToArray  '生成搜索所需要的种子
            Call Console.WriteLine("Generate repeats search seeds, job done! {0} repeats sequence was export for seeds!", Seeds.Count)
            Dim Chars As Char() = (From c As Char In SequenceData Select c Distinct).ToArray
            Call Console.WriteLine("Scanning the whole sequence for each repeats loci.....")
            Dim Repeats = (From Loci As String
                           In Seeds
                           Let InternalSeeds = (From obj In __generateSeeds(Chars, Loci, cutoff, Min, Max:=Len(Loci) * 1.5) Select obj Group By obj.Key Into Group).ToArray.ToDictionary(Function(obj) obj.Key, elementSelector:=Function(obj) obj.Group.First.Value)
                           Let InternalSeedsSegment As String() = (From obj In InternalSeeds Select obj.Key).ToArray
                           Select InternalSeeds, Loci, repeatsCollection = __matchLociLocation(SequenceData, InternalSeedsSegment)).ToArray '遍历种子，进行全序列扫描
            Call Console.WriteLine("{0} repeats loci!", Repeats.Count)
            Dim LQuery = (From Group In Repeats.AsParallel
                          Let data = (From Loci As LociMatchedResult
                              In Group.repeatsCollection.AsParallel
                                      Let Score As Double = Group.InternalSeeds(Loci.Matched)
                                      Select Loci.InvokeSet(NameOf(Loci.Similarity), Score).InvokeSet(NameOf(Loci.Loci), Group.Loci)).ToArray
                          Select data).ToArray.MatrixToVector
            Call Console.WriteLine("Finally generate {0} repeats loci data!", LQuery.Count)
            Return LQuery
        End Function

        <ExportAPI("invoke.search.similarity")>
        Public Function InvokeSearch(Sequence As I_PolymerSequenceModel, Min As Integer, Max As Integer, Optional cutoff As Double = 0.65) As LociMatchedResult()
            Return InvokeSearch(Sequence.SequenceData, Min, Max, cutoff)
        End Function

        <ExportAPI("write.csv.repeats_result")>
        Public Function SaveRepeatsResult(result As Generic.IEnumerable(Of LociMatchedResult), saveto As String) As Boolean
            Return result.SaveTo(saveto, False)
        End Function

        <ExportAPI("write.csv.repeats_result")>
        Public Function SaveRepeatsResult(result As Generic.IEnumerable(Of ReversedLociMatchedResult), saveto As String) As Boolean
            Return result.SaveTo(saveto, False)
        End Function

        <ExportAPI("invoke.search.similarity.reversed")>
        Public Function InvokeSearchReversed(Sequence As String, Min As Integer, Max As Integer, Optional cutoff As Double = 0.65) As ReversedLociMatchedResult()
            Dim Seeds = (From rp As RevRepeats
                         In RepeatsSearchAPI.SearchReversedRepeats(New SegmentObject(Sequence.ToUpper.ShadowCopy(Sequence), 1), Min, Max)
                         Select rp.RevSegment
                         Distinct).ToArray  '生成搜索所需要的反向重复序列的种子
            Dim Chars As Char() = (From c As Char
                                   In Sequence
                                   Select c
                                   Distinct).ToArray

            'Seeds是具有重复的反向序列
            Dim Repeats = (From Loci As String
                           In Seeds.AsParallel
                           Let InternalSeeds = (From obj In __generateSeeds(Chars, Loci, cutoff)
                                                Select obj
                                                Group By obj.Key Into Group) _
                                                      .ToDictionary(Function(obj) obj.Key,
                                                                    Function(obj) obj.Group.First.Value)
                           Let InternalSeedsSegment As String() = (From obj In InternalSeeds Select obj.Key).ToArray
                           Select InternalSeeds,
                               Loci,
                               repeatsCollection = __matchLociLocation(Sequence, InternalSeedsSegment)).ToArray '遍历种子，进行全序列扫描

            '反向重复的
            Dim LQuery = (From Group
                          In Repeats.AsParallel
                          Select (From Loci As LociMatchedResult
                                  In Group.repeatsCollection.AsParallel
                                  Let Score As Double = Group.InternalSeeds(Loci.Matched)
                                  Let LociResult = Loci.InvokeSet(NameOf(Loci.Similarity), Score).InvokeSet(NameOf(Loci.Loci), Group.Loci)
                                  Select ReversedLociMatchedResult.GenerateFromBase(LociResult))).MatrixToVector
            Return LQuery
        End Function
    End Module
End Namespace