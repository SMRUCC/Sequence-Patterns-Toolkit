﻿#Region "Microsoft.VisualBasic::d9abe85a4e609902ed7122757f5627a3, analysis\SequenceToolkit\NeedlemanWunsch\NeedlemanWunsch.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
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



    ' /********************************************************************************/

    ' Summaries:

    ' Class NeedlemanWunsch
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Function: defaultScoreMatrix
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.DataMining.DynamicProgramming.NeedlemanWunsch

Public Class NeedlemanWunsch : Inherits NeedlemanWunsch(Of Char)

    Sub New(query As String, subject As String)
        Call MyBase.New(defaultScoreMatrix, "-"c, Function(x) x)

        Me.Sequence1 = query.ToCharArray
        Me.Sequence2 = subject.ToCharArray
    End Sub

    Private Shared Function defaultScoreMatrix() As ScoreMatrix(Of Char)
        Return New ScoreMatrix(Of Char)(Function(a, b) a = b)
    End Function
End Class
