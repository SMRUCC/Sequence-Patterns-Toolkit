Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Linq.Extensions
Imports System.Drawing
Imports LANS.SystemsBiology.AnalysisTools.NBCR.Extensions.MEME_Suite.DocumentFormat.MEME.LDM
Imports LANS.SystemsBiology.AnalysisTools.NBCR.Extensions.MEME_Suite.DocumentFormat
Imports LANS.SystemsBiology.SequenceModel.FASTA
Imports System.Runtime.CompilerServices

Namespace SequenceLogo

    ''' <summary>
    ''' In bioinformatics, a sequence logo is a graphical representation of the sequence conservation 
    ''' of nucleotides (in a strand Of DNA/RNA) Or amino acids (In protein sequences).
    ''' A sequence logo Is created from a collection of aligned sequences And depicts the consensus 
    ''' sequence And diversity of the sequences. Sequence logos are frequently used to depict sequence 
    ''' characteristics such as protein-binding sites in DNA Or functional units in proteins.
    ''' </summary>
    <[PackageNamespace]("SequenceLogo",
                        Description:="In bioinformatics, a sequence logo is a graphical representation of the sequence conservation " &
                 "of nucleotides (in a strand Of DNA/RNA) Or amino acids (In protein sequences). " &
                 "A sequence logo Is created from a collection of aligned sequences And depicts the consensus " &
                 "sequence And diversity of the sequences. Sequence logos are frequently used to depict sequence " &
                 "characteristics such as protein-binding sites in DNA Or functional units in proteins.",
                        Cites:="<li>(1990). ""Sequence logos: a New way to display consensus sequences.""</li>///
<li>Crooks;, G. E., et al. (2004). ""WebLogo: A Sequence Logo Generator."" Genome Res 12(1): 47-56.
<p>A systematic computational analysis of protein sequences containing known nuclear domains led to the identification of 28 novel domain families. 
                        This represents a 26% increase in the starting set of 107 known nuclear domain families used for the analysis. Most of the novel domains are present in all major eukaryotic lineages, but 3 are species specific. For about 500 of the 1200 proteins that contain these new domains, nuclear localization could be inferred, and for 700, additional features could be predicted. 
                        For example, we identified a new domain, likely to have a role downstream of the unfolded protein response; a nematode-specific signalling domain; and a widespread domain, likely to be a noncatalytic homolog of ubiquitin-conjugating enzymes.</li>")>
    <Cite(Title:="Sequence logos: a New way to display consensus sequences.",
          Abstract:="A graphical method is presented for displaying the patterns in a set of aligned sequences. 
The characters representing the sequence are stacked on top of each other for each position in the aligned sequences. 
The height of each letter is made proportional to Its frequency, and the letters are sorted so the most common one is on top. 
The height of the entire stack is then adjusted to signify the information content of the sequences at that position. 
From these 'sequence logos', one can determine not only the consensus sequence but also the relative frequency of bases and the information content (measured In bits) at every position in a site or sequence. 
The logo displays both significant residues and subtle sequence patterns.",
          AuthorAddress:="National Cancer Institute, Frederick Cancer Research and Development Center, Laboratory of Mathematical Biology, PO Box B, Frederick, MD 21701, USA",
          Authors:="Thomas D.Schneider* and R.Michael Stephens",
          DOI:="",
          ISSN:="",
          Issue:="20",
          Journal:="Nucleic Acids Research",
          Keywords:="",
          Pages:="6097-6100",
          Year:=1990,
          Volume:=18)>
    <Cite(Title:="WebLogo: A Sequence Logo Generator.", Volume:=12, Issue:="1", Year:=2004, Pages:="47-56", Authors:="Crooks;, G. E., et al.",
          Abstract:="A systematic computational analysis of protein sequences containing known nuclear domains led to the identification of 28 novel domain families. 
This represents a 26% increase in the starting set of 107 known nuclear domain families used for the analysis. Most of the novel domains are present in all major eukaryotic lineages, but 3 are species specific. 
For about 500 of the 1200 proteins that contain these new domains, nuclear localization could be inferred, and for 700, additional features could be predicted. 
For example, we identified a new domain, likely to have a role downstream of the unfolded protein response; a nematode-specific signalling domain; and a widespread domain, likely to be a noncatalytic homolog of ubiquitin-conjugating enzymes.",
          Journal:="Genome Res")>
    Public Module DrawingDevice

        ''' <summary>
        ''' 字符的宽度
        ''' </summary>
        Public Property WordSize As Integer = 80
        Dim Height As Integer = 75

        ''' <summary>
        ''' 绘制各个残基的出现频率
        ''' </summary>
        ''' <param name="Fasta"></param>
        ''' <returns></returns>
        <ExportAPI("Drawing.Frequency")>
        Public Function DrawFrequency(Fasta As FastaFile) As Image
            Dim Bits = If(Fasta.First.IsProtSource, Math.Log(20, 2), 2)
            Dim Frequency = LANS.SystemsBiology.BioAssemblyExtensions.Frequency(Fasta)
            Dim Model As SequenceLogo.DrawingModel = New DrawingModel
            Model.ModelsId = $"{NameOf(DrawFrequency)} for {Fasta.NumberOfFasta} sequence."
            Model.Residues = Frequency.ToArray(Function(rsd) New Residue With {
                                                   .Bits = Bits,
                                                   .Alphabets = rsd.Value.ToArray(Function(oa) New Alphabet With {
                                                        .Alphabet = oa.Key,
                                                        .RelativeFrequency = oa.Value}),
                                                        .AddrHwnd = rsd.Key})
            Return InvokeDrawing(Model, False)
        End Function

        ''' <summary>
        ''' The approximation for the small-sample correction, en, Is given by
        '''     en = 1/ln2 x (s-1)/2n
        ''' 
        ''' </summary>
        ''' <param name="s">s Is 4 For nucleotides, 20 For amino acids</param>
        ''' <param name="n">n Is the number Of sequences In the alignment</param>
        ''' <returns></returns>
        Public Function E(s As Integer, n As Integer) As Double
            Dim result As Double = 1 / Math.Log(2)
            result *= (s - 1) / 2 * n
            Return result
        End Function

        ''' <summary>
        ''' 绘制SequenceLogo图
        ''' </summary>
        ''' <param name="Model"></param>
        ''' <returns></returns>
        <ExportAPI("Invoke.Drawing", Info:="Drawing a sequence logo from a generated sequence motif model.")>
        Public Function InvokeDrawing(Model As SequenceLogo.DrawingModel,
                                      <Parameter("Order.Frequency", "Does the alphabets in a residue position will be ordered its drawing order based on their relative frequency in the residue site?")>
                                      Optional FrequencyOrder As Boolean = True,
                                      Optional Margin As Integer = 200,
                                      Optional reverse As Boolean = False) As Image

            Dim n As Integer = Model.Residues(Scan0).Alphabets.Length
            Dim ColorSchema As Dictionary(Of Char, Image) =
                If(n = 4, SequenceLogo.ColorSchema.NucleotideSchema, SequenceLogo.ColorSchema.ProteinSchema)

            Dim Gr As GDIPlusDeviceHandle =
                New Size(Model.Residues.Length * DrawingDevice.WordSize + 2 * Margin, 2 * Margin + n * Height).CreateGDIDevice
            Dim X, Y As Integer
            Dim DrawingFont As Font = New Font(FONT_FAMILY_MICROSOFT_YAHEI, CInt(WordSize * 0.6), FontStyle.Bold)
            Dim sz As SizeF

            sz = Gr.Gr_Device.MeasureString(Model.ModelsId, DrawingFont)
            Call Gr.Gr_Device.DrawString(Model.ModelsId, DrawingFont, Brushes.Black, New Point((Gr.Width - sz.Width) / 2, y:=Margin / 2.5))

            DrawingFont = New Font(FONT_FAMILY_MICROSOFT_YAHEI, CInt(WordSize * 0.4), FontStyle.Bold)

#Region "画坐标轴"

            X = Margin
            Y = Gr.Height - Margin  '坐标轴原点

            Dim MaxBits As Double = Math.Log(n, newBase:=2)
            Dim YHeight As Integer = n * DrawingDevice.Height

            Call Gr.Gr_Device.DrawLine(Pens.Black, New Point(X, Y - YHeight), New Point(X, Y))
            Call Gr.Gr_Device.DrawLine(Pens.Black, New Point(X, Y), New Point(X + Model.Residues.Length * DrawingDevice.WordSize, y:=Y))

            Dim ddddd As Integer = If(MaxBits = 2, 2, 5)
            Dim d As Double = MaxBits / ddddd
            YHeight = d / MaxBits * (DrawingDevice.Height * n) '步进

            Dim YBits As Double = 0

            For j As Integer = 0 To ddddd
                sz = Gr.Gr_Device.MeasureString(YBits, font:=DrawingFont)

                Dim y1 = Y - sz.Height / 2

                Gr.Gr_Device.DrawString(YBits, DrawingFont, Brushes.Black, New Point(x:=X - sz.Width, y:=y1))

                y1 = Y '- sz.Height / 8
                Gr.Gr_Device.DrawLine(Pens.Black, New Point(x:=X, y:=y1), New Point(x:=X + 10, y:=y1))

                YBits += d
                Y -= YHeight
            Next

            Dim source As Generic.IEnumerable(Of Residue) = If(reverse, Model.Residues.Reverse, Model.Residues)

            For Each residue As Residue In source
                Dim order As Alphabet() = If(FrequencyOrder, (From rsd As Alphabet
                                                              In residue.Alphabets
                                                              Select rsd
                                                              Order By rsd.RelativeFrequency Ascending).ToArray, residue.Alphabets)
                Y = Gr.Height - Margin
                YHeight = (n * DrawingDevice.Height) * (If(residue.Bits > MaxBits, MaxBits, residue.Bits) / MaxBits)

                Dim idx As String = CStr(residue.AddrHwnd)
                sz = Gr.Gr_Device.MeasureString(idx, DrawingFont)
                Call Gr.Gr_Device.DrawString(idx, DrawingFont, Brushes.Black, New Point(x:=X + sz.Width / If(Math.Abs(residue.AddrHwnd) < 10, 2, 5), y:=Y))

                For Each Alphabet As Alphabet In order
                    Dim H As Single = Alphabet.RelativeFrequency * YHeight

                    Y -= H
                    Gr.Gr_Device.DrawImage(ColorSchema(Alphabet.Alphabet), CSng(X), CSng(Y), CSng(DrawingDevice.WordSize), H)
                Next

                X += DrawingDevice.WordSize
            Next

            '绘制bits字符串
            DrawingFont = New Font(DrawingFont.Name, DrawingFont.Size / 2)
            sz = Gr.Gr_Device.MeasureString("Bits", DrawingFont)
            Call Gr.Gr_Device.RotateTransform(-90)
            Call Gr.Gr_Device.DrawString("Bits", DrawingFont, Brushes.Black, New Point((Height - sz.Width) / 2, Margin / 3))

#End Region
            Return Gr.ImageResource
        End Function
    End Module
End Namespace