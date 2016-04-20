Imports Microsoft.VisualBasic.CommandLine.Reflection

''' <summary>
''' Clustal Omega - 1.2.0 (AndreaGiacomo)
'''
''' If you Like Clustal - Omega please cite:
''' Sievers F, Wilm A, Dineen D, Gibson TJ, Karplus K, Li W, Lopez R, McWilliam H, Remmert M, S├╢ding J, Thompson JD, Higgins DG.
''' Fast, scalable generation of high-quality protein multiple sequence alignments using Clustal Omega.
''' Mol Syst Biol. 2011 Oct 11;7:539. doi: 10.1038/msb.2011.75. PMID: 21988835.
''' If you don't like Clustal-Omega, please let us know why (and cite us anyway).
'''
''' Check http :   //www.clustal.org for more information And updates.
'''
''' Usage: clustalo [-hv] [-i {&lt;file>,-}] [--hmm-In=&lt;file>]... [--dealign] [--profile1=&lt;file>] [--profile2=&lt;file>] 
''' [--Is-profile] [-t {Protein, RNA, DNA}] [--infmt={a2m=fa[sta],clu[stal],msf,phy[lip],selex,st[ockholm],vie[nna]}] 
''' [--distmat-In=&lt;file>] [--distmat-out=&lt;file>] [--guidetree-In=&lt;file>] [--guidetree-out=&lt;file>] [--full] [--full-iter] 
''' [--cluster-size=&lt;n>] [--clustering-out=&lt;file>] [--use-kimura] [--percent-id] [-o {file,-}] 
''' [--outfmt={a2m=fa[sta],clu[stal],msf,phy[lip],selex,st[ockholm],vie[nna]}] [--residuenumber] [--wrap=&lt;n>] 
''' [--output-order={input-order,tree-order}] [--iterations=&lt;n>] [--max-guidetree-iterations=&lt;n>] [--max-hmm-iterations=&lt;n>] 
''' [--maxnumseq=&lt;n>] [--maxseqlen=&lt;l>] [--auto] [--threads=&lt;n>] [-l &lt;file>] [--version] [--Long-version] [--force] 
''' [--MAC-RAM=&lt;n>]
'''
''' A typical invocation would be: clustalo -i my-in-seqs.fa -o my-out-seqs.fa -v
''' </summary>
Public Class CLIBuilder

    ' See below For a list Of all options.

#Region "Sequence Input"
    ''' <summary>
    ''' -i, --in, --infile={&lt;file>,-} 
    ''' Multiple sequence input file (- for stdin)
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--infile", [Optional].Types.String)> Public Property InFile As String

    ''' <summary>
    ''' --hmm-in=&lt;file>           
    ''' HMM input files
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--hmm-in", [Optional].Types.String)> Public Property HMMIn As String

    ''' <summary>
    ''' --dealign                 
    ''' Dealign input sequences
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--dealign", [Optional].Types.Boolean)> Public Property Dealign As Boolean

    ''' <summary>
    ''' --profile1, --p1=&lt;file>   
    ''' Pre-aligned multiple sequence file (aligned columns will be kept fix)
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--profile1", [Optional].Types.String)> Public Property p1 As String

    ''' <summary>
    ''' --profile2, --p2=&lt;file>   
    ''' Pre-aligned multiple sequence file (aligned columns will be kept fix)
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--profile2", [Optional].Types.String)> Public Property p2 As String

    ''' <summary>
    ''' --Is-profile              
    ''' disable check if profile, force profile (default no)
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--is-profile", [Optional].Types.Boolean)> Public Property IsProfile As Boolean

    ''' <summary>
    ''' -t, --seqtype={Protein, RNA, DNA} 
    ''' Force a sequence type (default: auto)
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--seqtype", [Optional].Types.String)> Public Property SeqType As String

    ''' <summary>
    ''' --infmt={a2m=fa[sta],clu[stal],msf,phy[lip],selex,st[ockholm],vie[nna]} 
    ''' Forced sequence input file format (default: auto)
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--infmt", [Optional].Types.String)> Public Property InFmt As OutFmts = OutFmts.auto
#End Region


#Region "Clustering"

    ''' <summary>
    ''' --distmat-in=&lt;file>       
    ''' Pairwise distance matrix input file (skips distance computation)
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--distmat-in", [Optional].Types.String)> Public Property DistmatIn As String
    '  
    ''' <summary>
    ''' --distmat-out=&lt;file>      
    ''' Pairwise distance matrix output file
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--distmat-out", [Optional].Types.String)> Public Property DistmatOut As String
    '  
    ''' <summary>
    ''' --guidetree-in=&lt;file>     
    ''' Guide tree input file (skips distance computation And guide-tree clustering step)
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--guidetree-in", [Optional].Types.String)> Public Property GuidetreeIn As String
    '  
    ''' <summary>
    ''' --guidetree-out=&lt;file>    
    ''' Guide tree output file
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--guidetree-out", [Optional].Types.String)> Public Property GuidetreeOut As String
    '  
    ''' <summary>
    ''' --full                    
    ''' Use full distance matrix for guide-tree calculation (might be slow; mBed Is default)
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--full", [Optional].Types.String)> Public Property Full As String
    '  
    ''' <summary>
    ''' --full-iter               
    ''' Use full distance matrix for guide-tree calculation during iteration (might be slowish; mBed Is default)
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--full-iter", [Optional].Types.String)> Public Property FullIter As String
    '  
    ''' <summary>
    ''' --cluster-size=&lt;n>        
    ''' soft maximum of sequences in sub-clusters
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--cluster-size", [Optional].Types.Integer)> Public Property ClusterSize As Integer
    '  
    ''' <summary>
    ''' --clustering-out=&lt;file>   
    ''' Clustering output file
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--clustering-out", [Optional].Types.String)> Public Property ClusteringOut As String
    '  
    ''' <summary>
    ''' --use-kimura              
    ''' use Kimura distance correction for aligned sequences (default no)
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--use-kimura", [Optional].Types.Boolean)> Public Property UseKimura As Boolean
    '  
    ''' <summary>
    ''' --percent-id              
    ''' convert distances into percent identities (default no)
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--percent-id", [Optional].Types.Boolean)> Public Property PercentId As Boolean
#End Region


#Region "Alignment Output"

    ''' <summary>
    ''' -o, --out, --outfile={file,-} 
    ''' Multiple sequence alignment output file (default stdout)
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--outfile", [Optional].Types.String)> Public Property Out As String

    ''' <summary>
    ''' --outfmt={a2m=fa[sta],clu[stal],msf,phy[lip],selex,st[ockholm],vie[nna]} 
    ''' MSA output file format (default: fasta)
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--outfmt", [Optional].Types.String)> Public Property OutFmt As OutFmts = OutFmts.auto

    ''' <summary>
    ''' --residuenumber, --resno  
    ''' in Clustal format print residue numbers (default no)
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--residuenumber", [Optional].Types.Boolean)> Public Property ResidueNumber As Boolean

    ''' <summary>
    ''' --wrap=&lt;n>                
    ''' number of residues before line-wrap in output
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--wrap", [Optional].Types.Integer)> Public Property Wrap As Integer

    ''' <summary>
    ''' --output-order={input-order,tree-order} 
    ''' MSA output orderlike in input/guide-tree
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--output-order", [Optional].Types.String)> Public Property OutputOrder As String
#End Region

#Region "Iteration"

    ''' <summary>
    ''' --iterations, --iter=&lt;n>  
    ''' Number of (combined guide-tree/HMM) iterations
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--iterations", [Optional].Types.Integer)> Public Property Iterations As Integer

    ''' <summary>
    ''' --max-guidetree-iterations=&lt;n> 
    ''' Maximum number guidetree iterations
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--max-guidetree-iterations", [Optional].Types.Integer)> Public Property MaxGuidetreeIterations As Integer

    ''' <summary>
    ''' --max-hmm-iterations=&lt;n>  
    ''' Maximum number of HMM iterations
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--max-hmm-iterations", [Optional].Types.Integer)> Public Property MaxHMMIterations As Integer
#End Region

#Region "Limits (will exit early, if exceeded)"

    ''' <summary>
    ''' --maxnumseq=&lt;n>           
    ''' Maximum allowed number of sequences
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--maxnumseq", [Optional].Types.Integer)> Public Property MaxNumSeq As Integer

    ''' <summary>
    ''' --maxseqlen=&lt;l>           
    ''' Maximum allowed sequence length
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--maxseqlen", [Optional].Types.Integer)> Public Property MaxSeqLen As Integer
#End Region

#Region "Miscellaneous"

    ''' <summary>
    ''' --auto                    
    ''' Set options automatically (might overwrite some of your options)
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--auto", [Optional].Types.Boolean)> Public Property Auto As Boolean
    ''' <summary>
    ''' --threads=&lt;n>             
    ''' Number of processors to use
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--threads", [Optional].Types.Integer)> Public Property Threads As Integer
    ''' <summary>
    ''' -l, --log=&lt;file>          
    ''' Log all non-essential output to this file
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--log", [Optional].Types.String)> Public Property Log As String
    ''' <summary>
    ''' -h, --help                
    ''' Print this help And exit
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--help", [Optional].Types.String)> Public Property Help As String
    ''' <summary>
    ''' -v, --verbose             
    ''' Verbose output (increases if given multiple times)
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--v", [Optional].Types.String)> Public Property Verbose As String
    ''' <summary>
    ''' --version                 
    ''' Print version information And exit
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--version", [Optional].Types.String)> Public Property Version As String
    ''' <summary>
    ''' --long-version            
    ''' Print long version information And exit
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--long-version", [Optional].Types.String)> Public Property LongVersion As String
    ''' <summary>
    ''' --force                   
    ''' Force file overwriting
    ''' </summary>
    ''' <returns></returns>
    <[Optional]("--force", [Optional].Types.Boolean)> Public Property Force As Boolean
#End Region

End Class
