#Region "Microsoft.VisualBasic::f514e8d09d7de51163f3b222ea8e5d90, ..\LANS.SystemsBiology.AnalysisTools.ComparativeGenomics\ToolsAPI\SiteSigma.vb"

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


Imports Microsoft.VisualBasic.DocumentFormat.Csv.StorageProvider.Reflection
''' <summary>
''' 基因组两两比较所得到的位点距离数据
''' </summary>
''' <remarks></remarks>
Public Class SiteSigma
    <Column("Site")> Public Property Site As Integer
    <Column("Sigma")> Public Property Sigma As Double
    Public Property Similarity As DifferenceMeasurement.SimilarDiscriptions
End Class
