Imports System.Text.RegularExpressions
Imports jacky8399.MCLauncher

Public Class SyntaxTree
    Public Shared Function parseTree(str As String) As ICmdValue
        If Regex.IsMatch(str.Trim(), "^#(.+)") Then
            Dim args = str.Split(" "c)
            If args.Length = 1 Then
                Return New CmdEvaluatorDirective With {.dir = str}
            Else
                Return New CmdEvaluatorDirective With {.dir = args(0), .args = (Function(a As List(Of String))
                                                                                    a.RemoveAt(0)
                                                                                    Return a
                                                                                End Function)(New List(Of String)(args))}
            End If
        ElseIf Regex.IsMatch(str.Trim(), "^(.+?)([+\-*/=])(.+)") Then
            Dim match As Match = Regex.Match(str.Trim(), "(.+?)([+\-*/=])(.+)")
            Return New CmdOpInvoke With {.operand1 = parseTree(match.Groups(1).Value), .operand2 = parseTree(match.Groups(3).Value), .opor = CmdOpInvoke.parseOp(match.Groups(2).Value)}
        ElseIf Regex.IsMatch(str.Trim(), "^(?:\{(.+)\}|scope\((.+)\))") Then
            Return New CmdScopeBlock With {.ops = parseTreeComplex(Regex.Match(str.Trim(), "(?:\{(.+)\}|scope\((.+)\))").Groups(1).Value)}
        ElseIf Regex.IsMatch(str.Trim(), "^""(.*)""$", RegexOptions.Singleline) Then
            'Return New CmdStrLiteral With {.str = Regex.Match(str.Trim(), "^""(.*)""", RegexOptions.Singleline).Groups(1).Value.Replace("\n", vbCrLf)}
            Dim toRead = Regex.Match(str.Trim(), "^""(.*)""", RegexOptions.Singleline).Groups(1).Value
            Dim escape As Boolean = False
            Dim seq As String = ""
            For Each c As Char In toRead
                If c = "\"c And Not escape Then
                    escape = True
                    Continue For
                End If
                If escape Then
                    escape = False
                    Select Case c
                        Case "n"c, "r"c
                            seq += vbCrLf
                        Case """"c
                            seq += """"c
                        Case "\"c
                            seq += "\"c
                    End Select
                    Continue For
                End If
                seq += c
            Next
            Return New CmdStrLiteral With {.str = seq}
        ElseIf CmdFuncCall.isMatch(str.Trim()) Then
            Return CmdFuncCall.match(str.Trim())
        ElseIf Regex.IsMatch(str.Trim(), "^(.+?)\[(.+)\]") Then
            Return New CmdArrayAccess With {.invoke = parseTree(Regex.Match(str.Trim(), "^(.+?)\[(.+)\]").Groups(1).Value), .index = SyntaxTree.parseTree(Regex.Match(str.Trim(), "^(.+?)\[(.+)\]").Groups(2).Value)}
        ElseIf Regex.IsMatch(str.Trim(), "^(.+)\.(.+)$") Then
            Return New CmdClassInvoke With {.invokeOn = parseTree(Regex.Match(str.Trim(), "^(.+)\.(.+)").Groups(1).Value), .invoke = parseTree(Regex.Match(str.Trim(), "^(.+)\.(.+)").Groups(2).Value)}
        ElseIf Regex.IsMatch(str.Trim(), "^(?:true|false)$", RegexOptions.IgnoreCase) Then
            Return New CmdBoolean With {.boolVal = Boolean.Parse(str.Trim())}
        ElseIf Regex.IsMatch(str.Trim(), "^\$([A-Za-z0-9_]+)") Then
            Return New CmdVarCall With {.var = Regex.Match(str.Trim(), "^\$([A-Za-z0-9_]+)").Groups(1).Value}
        ElseIf Regex.IsMatch(str.Trim(), "^(-?\d+(?:\.\d*)?)%") Then
            Return New CmdValRel With {.relVal = Double.Parse(Regex.Match(str.Trim(), "^(-?\d+(?:\.\d*)?)%").Groups(1).Value) / 100}
        ElseIf Regex.IsMatch(str.Trim(), "^(?:0x|#)([0-9AaBbCcDdEeFf]+)") Then
            Return New CmdValHex With {.value = Long.Parse(Regex.Match(str.Trim(), "^(?:0x|#)([0-9AaBbCcDdEeFf]+)").Groups(1).Value, Globalization.NumberStyles.HexNumber), .repr = str.Trim()}
        ElseIf Regex.IsMatch(str.Trim(), "^(-?\d+(?:\.\d*)?)") Then
            Return New CmdValAbs With {.value = Double.Parse(Regex.Match(str.Trim(), "^(-?\d+(?:\.\d*)?)").Groups(1).Value)}
        ElseIf Regex.IsMatch(str.Trim(), "^null$") Then
            Return CmdNull.get
        End If
        Return New CmdUnknown With {.what = str.Trim()}
    End Function

    Public Shared Function parseTreeComplex(str As String, Optional delimeter As Char = ";"c, Optional lineBreaksAsDelimeter As Boolean = False) As List(Of ICmdValue)
        If str Is Nothing Then Return New List(Of ICmdValue)
        Dim list As New List(Of ICmdValue)
        Dim escape As Boolean = False
        Dim inQuot As Boolean = False
        Dim cur As String = ""
        Dim parenStack As Integer = 0
        For Each c As Char In str
            If c = """"c AndAlso Not escape Then inQuot = Not inQuot
            If c = "("c AndAlso Not inQuot Then parenStack += 1
            If c = ")"c AndAlso Not inQuot Then parenStack -= 1
            If (c = vbCr OrElse c = vbLf OrElse c = vbCrLf) AndAlso Not inQuot AndAlso Not lineBreaksAsDelimeter Then Continue For
            If (c = delimeter OrElse (lineBreaksAsDelimeter AndAlso c = vbLf)) AndAlso Not inQuot AndAlso parenStack = 0 Then
                If cur.Trim().Length <> 0 Then
                    list.Add(parseTree(cur.Trim()))
                End If
                cur = ""
                Continue For
            End If
            If escape Then escape = False
            If c = "\"c Then escape = Not escape
            cur += c
        Next
        If cur.Trim().Length <> 0 Then list.Add(parseTree(cur))
        If parenStack <> 0 Then MsgBox("Warning: uneven parenthesis.")
        Return list
    End Function
End Class

Public Interface ICmdValue
    Function str(sb As ScriptSandBox) As String
    Function repr() As String
End Interface
Public Class CmdFuncCall
    Implements ICmdValue
    Public funcName As String
    Public args As List(Of ICmdValue)
    Friend argString As String
    Public Shared REGEX As New Regex("^([^\.\(\)]+?)\(([^)]*)\)[^\.]?$", RegexOptions.Compiled)
    Public Overrides Function ToString() As String
        Return $"CmdFuncCall({funcName},({Strings.Join(args.map(Function(a As ICmdValue) a.ToString()).array(), ",")}))"
    End Function
    Public Function str(sb As ScriptSandBox) As String Implements ICmdValue.str
        Return sb.eval(Me).str(sb)
    End Function
    Public Function repr() As String Implements ICmdValue.repr
        Return $"{funcName}({Strings.Join(args.map(Function(a As ICmdValue) a.repr()).array(), ",")})"
    End Function
    Public Shared Function isMatch(str As String) As Boolean
        Try
            Dim cmd As String = str.Trim().Substring(0, str.Trim().IndexOf("("))
            Dim between = str.Trim().Substring(str.Trim().IndexOf("("c) + 1, str.Trim().LastIndexOf(")"c) - str.Trim().IndexOf("("c) - 1)
            Dim parenStack = 0
            Dim escape = False
            Dim inQuot = False
            For Each c As Char In between.Trim()
                If c = ")"c AndAlso parenStack = 0 AndAlso Not inQuot Then
                    Return False
                End If
                If c = "("c AndAlso Not inQuot Then
                    parenStack += 1
                    Continue For
                End If
                If c = ")"c AndAlso Not inQuot Then
                    parenStack -= 1
                    Continue For
                End If
                If c = """"c AndAlso Not escape Then inQuot = Not inQuot
                If escape Then escape = False
                If c = "\"c Then escape = True
            Next
            If parenStack = 0 Then Return (Not (cmd.Contains("."))) AndAlso True Else Return False
        Catch
            Return False
        End Try
    End Function
    Public Shared Function match(str As String) As CmdFuncCall
        If Not isMatch(str) Then Throw New ArgumentException(NameOf(str))
        Dim list As New List(Of ICmdValue)
        Dim cmd As String = str.Trim().Substring(0, str.Trim().IndexOf("("))
        Dim between = str.Trim().Substring(str.Trim().IndexOf("("c) + 1, str.Trim().LastIndexOf(")"c) - str.Trim().IndexOf("("c) - 1)
        Dim escape As Boolean = False
        Dim inQuot As Boolean = False
        Dim parenStack As Integer = 0
        Dim current As String = ""
        Dim args As New List(Of String)()
        For Each c As Char In between
            If c = "("c AndAlso Not inQuot Then parenStack += 1
            If c = ")"c AndAlso Not inQuot Then parenStack -= 1
            If c = """"c AndAlso Not escape Then inQuot = Not inQuot
            If escape Then escape = False
            If c = "\"c Then escape = True
            If c = ","c AndAlso parenStack = 0 AndAlso Not inQuot Then
                args.Add(current)
                current = ""
                Continue For
            End If
            current += c
        Next
        args.Add(current)
        For Each ss As String In args
            list.Add(SyntaxTree.parseTree(ss))
        Next
        If list.Count = 1 AndAlso TypeOf list(0) Is CmdUnknown AndAlso DirectCast(list(0), CmdUnknown).what = "" Then list.Clear()
        Return New CmdFuncCall With {.funcName = cmd, .args = list, .argString = between}
    End Function
End Class
Public Class CmdStrLiteral
    Implements ICmdValue
    Public str As String
    Public Function repr() As String Implements ICmdValue.repr
        Return $"""{str}"""
    End Function
    Public Overrides Function ToString() As String
        Return $"CmdStrLiteral(""{str}"")"
    End Function
    Public Function strrepr(sb As ScriptSandBox) As String Implements ICmdValue.str
        Return str
    End Function
End Class
Public Class CmdValAbs
    Implements ICmdValue
    Public value As Double
    Public Function repr() As String Implements ICmdValue.repr
        Return value.ToString()
    End Function
    Public Function str(sb As ScriptSandBox) As String Implements ICmdValue.str
        Return value.ToString()
    End Function
    Public Overrides Function ToString() As String
        Return $"CmdValAbs({value}D)"
    End Function
End Class
Public Class CmdValHex
    Inherits CmdValAbs
    Implements ICmdValue
    Public Shadows repr As String
    Private Function ICmdValue_repr() As String Implements ICmdValue.repr
        Return repr.ToString()
    End Function
    Public Shadows Function str(sb As ScriptSandBox) As String Implements ICmdValue.str
        Return value.ToString()
    End Function
    Public Overrides Function ToString() As String
        Return $"CmdValHex(""{repr}"",{value}D)"
    End Function
End Class
Public Class CmdValRel
    Implements ICmdValue
    Public relVal As Double
    Public Function repr() As String Implements ICmdValue.repr
        Return $"{relVal}%"
    End Function
    Public Function str(sb As ScriptSandBox) As String Implements ICmdValue.str
        Return $"{relVal}%"
    End Function
    Public Overrides Function ToString() As String
        Return $"CmdValRel({relVal}%)"
    End Function
End Class
Public Class CmdVarCall
    Implements ICmdValue
    Public var As String
    Public Function repr() As String Implements ICmdValue.repr
        Return $"${var}"
    End Function
    Public Function str(sb As ScriptSandBox) As String Implements ICmdValue.str
        Return If(sb.getVariable(var)?.str(sb), "null")
    End Function
    Public Overrides Function ToString() As String
        Return $"CmdVarCall(${var})"
    End Function
End Class
Public Class CmdUnknown
    Implements ICmdValue
    Public what As String
    Public Function repr() As String Implements ICmdValue.repr
        Return what
    End Function
    Public Function str(sb As ScriptSandBox) As String Implements ICmdValue.str
        Return ""
    End Function
    Public Overrides Function ToString() As String
        Return $"CmdUnknown({what})"
    End Function
End Class
Public Class CmdOpInvoke
    Implements ICmdValue
    Public operand1 As ICmdValue
    Public Enum CmdOpOps
        UNKNOWN = -1
        UNDEF = -1
        ASSIGN = 0
        ASS = 0
        ADDITION = 1
        ADD = 1
        SUBTRACTION = 2
        [SUB] = 2
        MULTIPLICATION = 3
        MUL = 3
        DIVDISION = 4
        DIV = 4
        NEGATION = 5
        NEG = 5
        ABSOLUTION = 6
        ABS = 6
    End Enum
    Public Shared Function parseOp(str As String) As CmdOpOps
        Select Case str.Trim()
            Case "="
                Return CmdOpOps.ASS
            Case "+"
                Return CmdOpOps.ADD
            Case "-"
                Return CmdOpOps.SUB
            Case "*"
                Return CmdOpOps.MUL
            Case "/"
                Return CmdOpOps.DIV
        End Select
        Return CmdOpOps.UNDEF
    End Function
    Public Shared Function opToString(c As CmdOpOps) As String
        Select Case c
            Case CmdOpOps.ADD
                Return "+"
            Case CmdOpOps.ASS
                Return "="
            Case CmdOpOps.SUB
                Return "-"
            Case CmdOpOps.MUL
                Return "*"
            Case CmdOpOps.DIV
                Return "/"
        End Select
        Return ""
    End Function
    Public opor As CmdOpOps
    Public operand2 As ICmdValue
    Public Overrides Function ToString() As String
        Return $"CmdOpInvoke({operand1},{opor},{operand2})"
    End Function
    Public Function str(sb As ScriptSandBox) As String Implements ICmdValue.str
        Return sb.eval(Me).str(sb)
    End Function
    Public Function repr() As String Implements ICmdValue.repr
        Return $"{operand1.repr()}{opToString(opor)}{operand2.repr()}"
    End Function
End Class
Public Class CmdClassWrap
    Implements ICmdValue
    Public className As String
    Public props As New Dictionary(Of String, ICmdValue)
    Public Function repr() As String Implements ICmdValue.repr
        Return $"classInstance({className},{Strings.Join(New List(Of KeyValuePair(Of String, ICmdValue))(props).map(Function(k As KeyValuePair(Of String, ICmdValue)) $"clsProp({k.Key}{k.Value.repr()})").array(), ",")})"
    End Function
    Public Function str(sb As ScriptSandBox) As String Implements ICmdValue.str
        Return ToString()
    End Function
    Public Overrides Function ToString() As String
        Return $"CmdClassWrap${className}({Strings.Join(New List(Of KeyValuePair(Of String, ICmdValue))(props).map(Function(k As KeyValuePair(Of String, ICmdValue)) $"{k.Key} -> {k.Value.ToString()}").array(), ",")})"
    End Function
End Class
Public Class CmdBoolean
    Implements ICmdValue
    Public boolVal As Boolean
    Public Function repr() As String Implements ICmdValue.repr
        Return boolVal.ToString()
    End Function
    Public Function str(sb As ScriptSandBox) As String Implements ICmdValue.str
        Return repr()
    End Function
    Public Overrides Function ToString() As String
        Return $"CmdBoolean({boolVal})"
    End Function
End Class
Public Class CmdScopeBlock
    Implements ICmdValue
    Public ops As List(Of ICmdValue)
    Public Function repr() As String Implements ICmdValue.repr
        Return $"{{{Strings.Join(ops.map(Function(a As ICmdValue) a.ToString()).array(), ";")}}}"
    End Function
    Public Function str(sb As ScriptSandBox) As String Implements ICmdValue.str
        Return ToString()
    End Function
    Public Overrides Function ToString() As String
        Return $"CmdScopeBlock({Strings.Join(ops.map(Function(a As ICmdValue) a.ToString()).array(), ";")})"
    End Function
End Class
Public Class CmdFuncPtr
    Implements ICmdValue
    Public toFunc As String
    Public Function repr() As String Implements ICmdValue.repr
        Return $"{toFunc}"
    End Function
    Public Function str(sb As ScriptSandBox) As String Implements ICmdValue.str
        Return $"{toFunc}"
    End Function
    Public Overrides Function ToString() As String
        Return $"CmdFuncPtr({toFunc})"
    End Function
End Class
Public Class CmdNull
    Implements ICmdValue
    Private Shared instance As CmdNull
    Private Sub New()
    End Sub
    Public Shared Function [get]() As CmdNull
        If instance Is Nothing Then
            instance = New CmdNull
        End If
        Return instance
    End Function
    Public Function repr() As String Implements ICmdValue.repr
        Return "null"
    End Function
    Public Function str(sb As ScriptSandBox) As String Implements ICmdValue.str
        Return "null"
    End Function
    Public Overrides Function ToString() As String
        Return $"CmdNull()"
    End Function
End Class
Public Class CmdClassInvoke
    Implements ICmdValue
    Public invokeOn As ICmdValue
    Public invoke As ICmdValue
    Public Function repr() As String Implements ICmdValue.repr
        Return invokeOn.repr() & "." & invoke.repr()
    End Function
    Public Function str(sb As ScriptSandBox) As String Implements ICmdValue.str
        Return sb.eval(Me).str(sb)
    End Function
    Public Overrides Function ToString() As String
        Return $"CmdClassInvoke({invokeOn.ToString()},{invoke.ToString()})"
    End Function
End Class
Public Class CmdArrayAccess
    Implements ICmdValue
    Public invoke As ICmdValue
    Public index As ICmdValue
    Public Function repr() As String Implements ICmdValue.repr
        Return invoke.repr() & $"[{index}]"
    End Function
    Public Function str(sb As ScriptSandBox) As String Implements ICmdValue.str
        Return sb.eval(Me).str(sb)
    End Function
End Class
Public Class CmdArrayType
    Implements ICmdValue
    Public bounds As Integer
    Public items As New List(Of ICmdValue)
    Public Function repr() As String Implements ICmdValue.repr
        Return $"array({bounds})"
    End Function
    Public Function str(sb As ScriptSandBox) As String Implements ICmdValue.str
        Return Strings.Join(items.map(Function(c As ICmdValue) c.str(sb)).array())
    End Function
End Class
Public Class CmdEvaluatorDirective
    Implements ICmdValue
    Public dir As String
    Public args As New List(Of String)
    Public Function repr() As String Implements ICmdValue.repr
        Return $"#{dir} {Join(args.ToArray())}"
    End Function
    Public Function str(sb As ScriptSandBox) As String Implements ICmdValue.str
        Return repr()
    End Function
    Public Overrides Function ToString() As String
        Return $"CmdEvaluatorDirective({dir},{Join(args.ToArray(), ",")})"
    End Function
End Class
