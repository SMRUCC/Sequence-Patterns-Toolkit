#Region "Microsoft.VisualBasic::1d08b911e48cde9fda8682c57ff3170f, ..\SequencePatterns\Topologically\Palindrome\ImperfectPalindrome.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2016 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.

#End Region

Imports LANS.SystemsBiology.ComponentModel.Loci
Imports LANS.SystemsBiology.ComponentModel.Loci.Abstract
Imports LANS.SystemsBiology.SequenceModel
Imports Microsoft.VisualBasic.Scripting.MetaData

Namespace Topologically

    Public Class ImperfectPalindrome : Inherits NucleotideModels.Contig
        Implements ILoci

        ''' <summary>
        ''' 种子生成的序列
        ''' </summary>
        ''' <returns></returns>
        Public Property Site As String
        ''' <summary>
        ''' 种子序列在基因组上面的位置
        ''' </summary>
        ''' <returns></returns>
        Public Property Left As Integer Implements ILoci.Left
        ''' <summary>
        ''' 回文片段的位点位置
        ''' </summary>
        ''' <returns></returns>
        Public Property Paloci As Integer
        ''' <summary>
        ''' 回文片段的序列
        ''' </summary>
        ''' <returns></returns>
        Public Property Palindrome As String
        ''' <summary>
        ''' 片段相似度距离大小
        ''' </summary>
        ''' <returns></returns>
        Public Property Distance As Double
        ''' <summary>
        ''' 相似度高低
        ''' </summary>
        ''' <returns></returns>
        Public Property Score As Double
        ''' <summary>
        ''' 匹配的碱基
        ''' </summary>
        ''' <returns></returns>
        Public Property Matches As String
        ''' <summary>
        ''' 演化的路径
        ''' </summary>
        ''' <returns></returns>
        Public Property Evolr As String
        Public Property MaxMatch As Integer

        Public Overrides Function ToString() As String
            Return $"{Site} <==> {Palindrome}, {Matches}"
        End Function

        Protected Overrides Function __getMappingLoci() As NucleotideLocation
            Return New NucleotideLocation(Left, Paloci)
        End Function
    End Class
End Namespace
