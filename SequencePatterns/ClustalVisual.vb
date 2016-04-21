Imports System.Drawing
Imports LANS.SystemsBiology.SequenceModel
Imports LANS.SystemsBiology.SequenceModel.Patterns
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Scripting.MetaData

''' <summary>
''' 多序列比对的结果可视化
''' </summary>
''' <remarks></remarks>
''' 
<[PackageNamespace]("MultipleAlignment.Visualization",
                    Category:=APICategories.UtilityTools,
                    Description:="Data visualization for the Clustal multiple alignment output fasta file.",
                    Publisher:="amethyst.asuka@gcmodeller.org",
                    Url:="http://gcmodeller.org")>
Public Module ClustalVisual

    <DataFrameColumn("MLA.Margin")> Dim Margin As Integer = 20
    ''' <summary>
    ''' 一个点默认占据10个像素
    ''' </summary>
    ''' <remarks></remarks>
    <DataFrameColumn("MLA.DotSize")> Dim DotSize As Integer = 10
    <DataFrameColumn("MLA.FontSize")> Dim FontSize As Integer = 12

    Dim ColorProfiles As Dictionary(Of String, Color)

    <ExportAPI("DotSize.Set", Info:="Setups of the dot size for the residue plot.")>
    Public Sub SetDotSize(n As Integer)
        ClustalVisual.DotSize = n
    End Sub

    Sub New()
        ClustalVisual.ColorProfiles = Polypeptides.MEGASchema.ToDictionary(Function(x) x.Key.ToString, Function(x) x.Value)

        Call ClustalVisual.ColorProfiles.Add("-", Color.FromArgb(0, 0, 0, 0))
        Call ClustalVisual.ColorProfiles.Add(".", Color.FromArgb(0, 0, 0, 0))
        Call ClustalVisual.ColorProfiles.Add("*", Color.FromArgb(0, 0, 0, 0))
        Call ClustalVisual.ColorProfiles.Add("A", Color.Green)
        Call ClustalVisual.ColorProfiles.Add("T", Color.Blue)
        Call ClustalVisual.ColorProfiles.Add("G", Color.Red)
        Call ClustalVisual.ColorProfiles.Add("C", Color.Brown)
        Call ClustalVisual.ColorProfiles.Add("U", Color.CadetBlue)
    End Sub

    <ExportAPI("Drawing.Clustal")>
    Public Function InvokeDrawing(<Parameter("aln.File",
                                             "The file path of the clustal alignment result fasta file.")>
                                  aln As String) As Image
        Dim fa As FASTA.FastaFile = FASTA.FastaFile.Read(aln)
        Return InvokeDrawing(aln:=fa)
    End Function

    ''' <summary>
    ''' 蛋白质序列和核酸序列都可以使用本函数
    ''' </summary>
    ''' <param name="aln"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    <ExportAPI("Drawing.Clustal")>
    Public Function InvokeDrawing(aln As FASTA.FastaFile) As Image
        Dim titleMaxLen = (From fa As FASTA.FastaToken
                           In aln
                           Select l = Len(fa.Title),
                               fa.Title
                           Order By l Descending).First
        Dim TitleDrawingFont As Font = New Font(FONT_FAMILY_UBUNTU, FontSize)
        Dim StringSize As Size = titleMaxLen.Title.MeasureString(TitleDrawingFont)
        Dim DotSize As Integer = ClustalVisual.DotSize

        DotSize = Math.Max(DotSize, StringSize.Height) + 5

        Dim Gr = New Size((From fa As FASTA.FastaToken
                           In aln
                           Select fa.Length).ToArray.Max * DotSize + StringSize.Width + 2 * Margin,
                          (aln.NumberOfFasta + 1) * DotSize + 2.5 * Margin).CreateGDIDevice
        Dim X As Integer = 0.5 * Margin + StringSize.Width + 10, Y As Integer = Margin

        Call Gr.ImageAddFrame(offset:=1)

        Dim DotFont As New Font(FONT_FAMILY_UBUNTU, FontSize + 1, FontStyle.Bold)
        Dim ConservedSites = (From site As SeqValue(Of SimpleSite)
                              In Patterns.Frequency(aln).Residues.SeqIterator
                              Where Not (From xs In site.obj.Alphabets Where xs.Value = 1.0R Select xs).IsNullOrEmpty
                              Select site.Pos).ToArray

        Dim idx As Integer = 0

        For Each ch As Char In aln.First.SequenceData
            If Array.IndexOf(ConservedSites, idx) = -1 Then
                X += DotSize
                idx += 1
                Continue For
            End If

            Call Gr.Gr_Device.DrawString("*", DotFont, Brushes.Black, New Point(X, Y))

            idx += 1
            X += DotSize
        Next

        X = 0.5 * Margin + StringSize.Width + 10
        Y += DotSize

        For Each fa As FASTA.FastaToken In aln
            Call Gr.Gr_Device.DrawString(fa.Title,
                                         TitleDrawingFont,
                                         Brushes.Black,
                                         New Point(Margin * 0.75, Y))

            For Each ch As Char In fa.SequenceData
                Dim s As String = ch.ToString
                Dim br As New SolidBrush(ClustalVisual.ColorProfiles(s))

                Call Gr.Gr_Device.FillRectangle(br, New Rectangle(New Point(X, Y), New Size(DotSize, DotSize)))
                Call Gr.Gr_Device.DrawString(s, DotFont, Brushes.Black, New Point(X, Y))

                X += DotSize
            Next

            X = 0.5 * Margin + StringSize.Width + 10
            Y += DotSize
        Next

        Return Gr.ImageResource
    End Function
End Module
