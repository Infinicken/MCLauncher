Public Class ScriptSandBox
    ''' <summary>
    ''' Return nothing for not handling
    ''' </summary>
    ''' <param name="functionName"></param>
    ''' <param name="arguments"></param>
    ''' <returns></returns>
    Public Delegate Function FunctionHandler(functionName As String, arguments As List(Of ICmdValue), func As CmdFuncCall) As ICmdValue
    Public Class FuncInfo
        Public Sub New(inst As List(Of ICmdValue), Optional args As List(Of String) = Nothing)
            instructions = inst
            arguments = args
        End Sub
        Public instructions As List(Of ICmdValue)
        Public arguments As List(Of String)
    End Class
    Public Class ClassInfo
        Public staticFunc As List(Of CmdFuncPtr)
        Public memberFunc As List(Of CmdFuncPtr)
        Public props As Dictionary(Of String, ICmdValue)
    End Class
    Public Class ClassHandler
        Public staticFunc As Dictionary(Of String, Func(Of List(Of ICmdValue), ICmdValue))
        Public memberFunc As Dictionary(Of String, Func(Of List(Of ICmdValue), ICmdValue))
        Public props As Dictionary(Of String, Func(Of ICmdValue))
    End Class
    Private definedFunc As New Dictionary(Of String, List(Of ICmdValue))
    Private definedClass As New Dictionary(Of String, ClassInfo)
    Private funcHnd As New List(Of FunctionHandler)
    Private classHnd As New Dictionary(Of String, ClassHandler)
    Private varlist As New Dictionary(Of String, ICmdValue)
    Private swapvar As Dictionary(Of String, ICmdValue)
    Private tempFuncRet As ICmdValue
    Private doInlineBoxing As Boolean = True
    Public Sub New()
        funcHnd.Add(AddressOf defaultFunctions)
        For Each item As KeyValuePair(Of String, ClassHandler) In genClass()
            classHnd.Add(item.Key, item.Value)
        Next
    End Sub
    Private Function genClass() As Dictionary(Of String, ClassHandler)
        Dim d As New Dictionary(Of String, ClassHandler)
        d.Add("String", New ClassHandler() With {.memberFunc = (Function()
                                                                    Dim ddd As New Dictionary(Of String, Func(Of List(Of ICmdValue), ICmdValue))
                                                                    ddd.Add("length", Function(args As List(Of ICmdValue))
                                                                                          Return numCtor(DirectCast(DirectCast(args(0), CmdClassWrap).props("__primitive"), CmdStrLiteral).str.Length)
                                                                                      End Function)
                                                                    ddd.Add("substring", Function(args As List(Of ICmdValue))
                                                                                             If args.Count = 3 Then
                                                                                                 Return strCtor(DirectCast(DirectCast(args(0), CmdClassWrap).props("__primitive"), CmdStrLiteral).str.Substring(intCast(asNum(args(1))), intCast(asNum(args(2)))))
                                                                                             End If
                                                                                             Return args(0)
                                                                                         End Function)
                                                                    Return ddd
                                                                End Function)()})
        d.Add("Array", New ClassHandler() With {.memberFunc = (Function()
                                                                   Dim ddd As New Dictionary(Of String, Func(Of List(Of ICmdValue), ICmdValue))
                                                                   ddd.Add("length", Function(args As List(Of ICmdValue))
                                                                                         Return numCtor(DirectCast(DirectCast(args(0), CmdClassWrap).props("__primitive"), CmdArrayType).bounds)
                                                                                     End Function)
                                                                   ddd.Add("get", Function(args As List(Of ICmdValue))
                                                                                      If args.Count = 2 Then
                                                                                          Return DirectCast(DirectCast(args(0), CmdClassWrap).props("__primitive"), CmdArrayType).items(CInt(asNum(args(1)).value))
                                                                                      Else
                                                                                          Return CmdNull.get
                                                                                      End If
                                                                                  End Function)
                                                                   ddd.Add("push", Function(args As List(Of ICmdValue))
                                                                                       If args.Count = 2 Then
                                                                                           DirectCast(DirectCast(args(0), CmdClassWrap).props("__primitive"), CmdArrayType).items.Add(args(1))
                                                                                       End If
                                                                                       Return CmdNull.get
                                                                                   End Function)
                                                                   ddd.Add("pop", Function(args As List(Of ICmdValue))
                                                                                      Dim l = DirectCast(DirectCast(args(0), CmdClassWrap).props("__primitive"), CmdArrayType).items
                                                                                      Dim r = l(l.Count - 1)
                                                                                      l.RemoveAt(l.Count - 1)
                                                                                      Return r
                                                                                  End Function)
                                                                   Return ddd
                                                               End Function)()})
        d.Add("UUID", New ClassHandler() With {.staticFunc = (Function()
                                                                  Dim sfunc As New Dictionary(Of String, Func(Of List(Of ICmdValue), ICmdValue))
                                                                  sfunc.Add("create", Function(args As List(Of ICmdValue))
                                                                                          Return (Function(wrap As CmdClassWrap)
                                                                                                      wrap.props("__uuid") = strCtor(Guid.NewGuid().ToString())
                                                                                                      wrap.className = "UUID"
                                                                                                      Return wrap
                                                                                                  End Function)(New CmdClassWrap)
                                                                                      End Function)
                                                                  Return sfunc
                                                              End Function)(),
                                               .memberFunc = (Function()
                                                                  Dim mfunc As New Dictionary(Of String, Func(Of List(Of ICmdValue), ICmdValue))
                                                                  mfunc.Add("toString", Function(args As List(Of ICmdValue))
                                                                                            Return DirectCast(args(0), CmdClassWrap).props("__uuid")
                                                                                        End Function)
                                                                  Return mfunc
                                                              End Function)()})
        Return d
    End Function
    Private Function defaultFunctions(functionName As String, arguments As List(Of ICmdValue), func As CmdFuncCall) As ICmdValue
        Select Case functionName
            Case "alert"
                Dim str As New Text.StringBuilder()
                For Each item As ICmdValue In arguments
                    str.Append(item.str(Me))
                Next
                MsgBox(str.ToString())
                Return CmdNull.get
            Case "prompt"
                Dim str As New Text.StringBuilder()
                For Each item As ICmdValue In arguments
                    str.Append(item.str(Me))
                Next
                Return New CmdStrLiteral With {.str = InputBox(str.ToString())}
            Case "repr"
                Return New CmdStrLiteral With {.str = arguments(0).repr()}
            Case "repreval", "evalrepr", "repr_eval", "eval_repr"
                Return New CmdStrLiteral With {.str = eval(arguments(0)).repr()}
            Case "str"
                Return New CmdStrLiteral With {.str = arguments(0).str(Me)}
            Case "eval"
                Return eval(SyntaxTree.parseTree(TryCast(arguments(0), CmdStrLiteral)?.str))
            Case "parse"
                For Each c As ICmdValue In SyntaxTree.parseTreeComplex(TryCast(arguments(0), CmdStrLiteral)?.str)
                    MsgBox(c.ToString())
                Next
                Return CmdNull.get
            Case "scope"
                Return New CmdScopeBlock With {.ops = SyntaxTree.parseTreeComplex(func.argString)}
            Case "func"
                If arguments.Count = 1 Then
                    If TypeOf arguments(0) Is CmdUnknown Then
                        Return New CmdFuncPtr With {.toFunc = DirectCast(arguments(0), CmdUnknown).what}
                    ElseIf isScope(arguments(0)) Then
                        Dim lambdaName As String = "lambda" & Guid.NewGuid().ToString.Replace("-", "")
                        definedFunc.Add(lambdaName, DirectCast(arguments(0), CmdScopeBlock).ops)
                        Return New CmdFuncPtr With {.toFunc = lambdaName}
                    End If
                ElseIf arguments.Count = 2 Then
                    If TypeOf arguments(0) Is CmdStrLiteral AndAlso isScope(arguments(1)) Then
                        Dim lambdaName As String = DirectCast(arguments(0), CmdStrLiteral).str
                        Dim scope As CmdScopeBlock = If(TypeOf arguments(1) Is CmdScopeBlock, DirectCast(arguments(1), CmdScopeBlock), TryCast(eval(arguments(1)), CmdScopeBlock))
                        If Not definedFunc.ContainsKey(lambdaName) Then definedFunc.Add(lambdaName, scope.ops) Else definedFunc(lambdaName) = scope.ops
                        Return New CmdFuncPtr With {.toFunc = lambdaName}
                    End If
                End If
                Return CmdNull.get
            Case "return"
                If arguments.Count >= 1 Then tempFuncRet = arguments(0)
                Return CmdNull.get
            Case "del"
                For Each i As ICmdValue In arguments
                    If TypeOf i Is CmdUnknown Then
                        Dim cmd As CmdUnknown = DirectCast(i, CmdUnknown)
                        If cmd.what = "*" Then
                            definedFunc.Clear()
                        ElseIf cmd.what = "**" Then
                            definedFunc.Clear()
                            definedClass.Clear()
                            varlist.Clear()
                        End If
                        If definedFunc.ContainsKey(cmd.what) Then
                            definedFunc.Remove(cmd.what)
                        End If
                    ElseIf TypeOf i Is CmdVarCall Then
                        Dim cmd As CmdVarCall = DirectCast(i, CmdVarCall)
                        If varlist.ContainsKey(cmd.var) Then
                            Dim a As ICmdValue = Nothing
                            If arguments.Count = 1 Then a = varlist(cmd.var)
                            If TypeOf a Is CmdFuncPtr AndAlso DirectCast(a, CmdFuncPtr).toFunc.StartsWith("lambda") Then
                                If definedFunc.ContainsKey(DirectCast(a, CmdFuncPtr).toFunc) Then
                                    definedFunc.Remove(DirectCast(a, CmdFuncPtr).toFunc)
                                End If
                            End If
                            varlist.Remove(cmd.var)
                            Return a
                        End If
                    End If
                Next
                Return CmdNull.get
            Case "if"
                If arguments.Count = 2 Then
                    Dim bool As Boolean = asBool(arguments(0)).boolVal
                    If bool Then
                        If isScope(arguments(1)) Then
                            run(eval(arguments(1)))
                        Else
                            Return eval(arguments(1))
                        End If
                    Else
                        Return eval(arguments(0))
                    End If
                ElseIf arguments.Count = 3 Then
                    Dim bool As Boolean = asBool(arguments(0)).boolVal
                    If bool Then
                        If isScope(arguments(1)) Then
                            run(eval(arguments(1)))
                        Else
                            Return eval(arguments(1))
                        End If
                    Else
                        If isScope(arguments(2)) Then
                            run(eval(arguments(2)))
                        Else
                            Return eval(arguments(2))
                        End If
                    End If
                End If
                Return CmdNull.get
            Case "int", "integer", "floor"
                If arguments.Count = 1 Then
                    Return New CmdValAbs With {.value = Math.Floor(asNum(arguments(0)).value)}
                End If
                Return New CmdValAbs With {.value = 0}
            Case "num", "number"
                If arguments.Count = 1 Then
                    Return asNum(arguments(0))
                End If
                Return New CmdValAbs With {.value = 0}
            Case "bool", "boolean"
                If arguments.Count = 1 Then
                    Return asBool(arguments(0))
                End If
                Return New CmdBoolean With {.boolVal = False}
            Case "len"
                If arguments.Count = 1 Then
                    If TypeOf arguments(0) Is CmdStrLiteral Then
                        Return New CmdValAbs With {.value = DirectCast(arguments(0), CmdStrLiteral).str.Length}
                    ElseIf TypeOf arguments(0) Is CmdArrayAccess AndAlso TypeOf eval(DirectCast(arguments(0), CmdArrayAccess).invoke) Is CmdArrayType Then
                        Return New CmdValAbs With {.value = DirectCast(eval(arguments(0)), CmdArrayType).bounds}
                    End If
                End If
            Case "debugbreak", "debug_break"
                If True Then
                End If
                Return CmdNull.get
            Case "box"
                If arguments.Count = 1 Then
                    Dim evalRes As ICmdValue = eval(arguments(0))
                    If TypeOf evalRes Is CmdStrLiteral Then
                        Return createInstanceOfClass("String", New KeyValuePair(Of String, ICmdValue)("__primitive", evalRes))
                    ElseIf TypeOf evalRes Is CmdArrayType Then
                        Return createInstanceOfClass("Array", New KeyValuePair(Of String, ICmdValue)("__primitive", evalRes))
                    End If
                End If
                Return CmdNull.get
            Case "array"
                If arguments.Count = 1 AndAlso CInt(asNum(arguments(0)).value) > 0 Then
                    Return New CmdArrayType With {.bounds = CInt(asNum(arguments(0)).value), .items = (Function(a As List(Of ICmdValue))
                                                                                                           For i As Integer = 0 To .bounds - 1
                                                                                                               a.Add(CmdNull.get)
                                                                                                           Next
                                                                                                           Return a
                                                                                                       End Function)(New List(Of ICmdValue)())}
                Else
                    Return CmdNull.get
                End If
        End Select
        Return Nothing
    End Function
    Public Shared Function boolCtor(val As Boolean) As CmdBoolean
        Return New CmdBoolean With {.boolVal = val}
    End Function
    Public Shared Function boolCast(val As CmdBoolean) As Boolean
        Return val.boolVal
    End Function
    Public Shared Function numCtor(val As Double) As CmdValAbs
        Return New CmdValAbs With {.value = val}
    End Function
    Public Shared Function intCast(val As CmdValAbs) As Integer
        Return CInt(val.value)
    End Function
    Public Shared Function dblCast(val As CmdValAbs) As Double
        Return val.value
    End Function
    Public Shared Function strCtor(val As String) As CmdStrLiteral
        Return New CmdStrLiteral With {.str = val}
    End Function
    Public Shared Function strCast(val As CmdStrLiteral) As String
        Return val.str
    End Function
    Public Function getVariable(str As String) As ICmdValue
        Return If(varlist.ContainsKey(str), varlist(str), Nothing)
    End Function
    Public Sub setVariable(str As String, rslt As ICmdValue)
        If varlist.ContainsKey(str) Then varlist(str) = rslt Else varlist.Add(str, rslt)
    End Sub
    Public Sub delVariable(str As String)
        If varlist.ContainsKey(str) Then varlist.Remove(str)
    End Sub
    Public Function asNum(c As ICmdValue) As CmdValAbs
        If TypeOf c Is CmdValAbs Then Return CType(c, CmdValAbs)
        If TypeOf c Is CmdStrLiteral Then
            Dim r As Double
            Double.TryParse(CType(c, CmdStrLiteral).str, r)
            Return New CmdValAbs With {.value = r}
        End If
        If TypeOf c Is CmdValRel Then Return New CmdValAbs With {.value = CType(c, CmdValRel).relVal}
        If TypeOf c Is CmdFuncCall OrElse TypeOf c Is CmdVarCall OrElse TypeOf c Is CmdOpInvoke Then Return asNum(eval(c))
        If TypeOf c Is CmdBoolean Then Return New CmdValAbs With {.value = If(CType(c, CmdBoolean).boolVal, 1, 0)}
        Return New CmdValAbs With {.value = 0}
    End Function
    Public Function asBool(c As ICmdValue) As CmdBoolean
        If TypeOf c Is CmdValAbs Then Return New CmdBoolean With {.boolVal = CType(c, CmdValAbs).value <> 0}
        If TypeOf c Is CmdValRel Then Return New CmdBoolean With {.boolVal = CType(c, CmdValRel).relVal <> 0}
        If TypeOf c Is CmdBoolean Then Return CType(c, CmdBoolean)
        If TypeOf c Is CmdStrLiteral Then Return New CmdBoolean With {.boolVal = Not String.IsNullOrWhiteSpace(CType(c, CmdStrLiteral).str)}
        If TypeOf c Is CmdFuncCall OrElse TypeOf c Is CmdVarCall OrElse TypeOf c Is CmdOpInvoke Then Return asBool(eval(c))
        Return New CmdBoolean With {.boolVal = False}
    End Function
    Public Function isScope(c As ICmdValue) As Boolean
        Return TypeOf c Is CmdScopeBlock OrElse (TypeOf c Is CmdVarCall AndAlso isScope(getVariable(DirectCast(c, CmdVarCall).var))) OrElse (TypeOf c Is CmdFuncCall AndAlso (DirectCast(c, CmdFuncCall).funcName = "scope" OrElse
            DirectCast(c, CmdFuncCall).funcName = "!scope" OrElse (TypeOf SyntaxTree.parseTree(DirectCast(c, CmdFuncCall).funcName) Is CmdVarCall AndAlso TypeOf getVariable(DirectCast(SyntaxTree.parseTree(DirectCast(c, CmdFuncCall).funcName), CmdVarCall).var) Is CmdFuncPtr AndAlso
            (DirectCast(getVariable(DirectCast(SyntaxTree.parseTree(DirectCast(c, CmdFuncCall).funcName), CmdVarCall).var), CmdFuncPtr).toFunc = "scope" OrElse DirectCast(getVariable(DirectCast(SyntaxTree.parseTree(DirectCast(c, CmdFuncCall).funcName), CmdVarCall).var), CmdFuncPtr).toFunc = "!scope"))))
    End Function
    Private Function isClassNameTaken(str As String) As Boolean
        For Each item As KeyValuePair(Of String, ClassHandler) In classHnd
            If item.Key = str Then Return True
        Next
        For Each item As KeyValuePair(Of String, ClassInfo) In definedClass
            If item.Key = str Then Return True
        Next
        Return False
    End Function
    Public Function createInstanceOfClass(className As String, ParamArray props As KeyValuePair(Of String, ICmdValue)()) As CmdClassWrap
        Return New CmdClassWrap With {.className = className, .props = (Function()
                                                                            Dim d As New Dictionary(Of String, ICmdValue)()
                                                                            For Each item As KeyValuePair(Of String, ICmdValue) In props
                                                                                d.Add(item.Key, item.Value)
                                                                            Next
                                                                            Return d
                                                                        End Function)()}
    End Function
    Public Function eval(c As ICmdValue) As ICmdValue
        If c Is Nothing Then Return Nothing
        If TypeOf c Is CmdEvaluatorDirective Then
            Dim direc As CmdEvaluatorDirective = DirectCast(c, CmdEvaluatorDirective)
            Select Case direc.dir
                Case "#Disable", "#Enable"
                    Dim value = direc.dir = "#Enable"
                    Select Case direc.args(0)
                        Case "InlineBoxing"
                            doInlineBoxing = value
                        Case Else
                            MsgBox("Invalid #[Dis/En]able option. Acceptable options are:
InlineBoxing.")
                    End Select
                Case Else
                    MsgBox("Invalid directive " & direc.dir & ". Acceptable directives are:
#Disable, #Enable.")
            End Select
        ElseIf TypeOf c Is CmdFuncCall Then
            Dim cmd As CmdFuncCall = CType(c, CmdFuncCall)
            If TypeOf SyntaxTree.parseTree(cmd.funcName) Is CmdVarCall Then
                If TypeOf getVariable(DirectCast(SyntaxTree.parseTree(cmd.funcName), CmdVarCall).var) Is CmdFuncPtr Then 'AndAlso definedFunc.ContainsKey(DirectCast(getVariable(DirectCast(SyntaxTree.parseTree(cmd.funcName), CmdVarCall).var), CmdFuncPtr).toFunc) Then
                    Return eval(New CmdFuncCall With {.funcName = DirectCast(getVariable(DirectCast(SyntaxTree.parseTree(cmd.funcName), CmdVarCall).var), CmdFuncPtr).toFunc, .args = cmd.args, .argString = cmd.argString})
                ElseIf TypeOf getVariable(DirectCast(SyntaxTree.parseTree(cmd.funcName), CmdVarCall).var) Is CmdStrLiteral Then
                    Return eval(New CmdFuncCall With {.funcName = DirectCast(getVariable(DirectCast(SyntaxTree.parseTree(cmd.funcName), CmdVarCall).var), CmdStrLiteral).str, .args = cmd.args, .argString = cmd.argString})
                End If
            ElseIf cmd.funcName Like "!*" Then
                'Negative lookup
                cmd.funcName = cmd.funcName.Substring(1, cmd.funcName.Length - 1)
                For Each item As FunctionHandler In funcHnd
                    Dim ret As ICmdValue = item(cmd.funcName, cmd.args, cmd)
                    If ret IsNot Nothing Then
                        Return ret
                    End If
                Next
                If definedFunc.ContainsKey(cmd.funcName) Then
                    'Shadow all var
                    swapvar = New Dictionary(Of String, ICmdValue)(varlist)
                    varlist.Clear()
                    For i As Integer = 0 To cmd.args.Count - 1
                        setVariable(i.ToString(), cmd.args(i))
                    Next
                    run(definedFunc(cmd.funcName))
                    'Un-shadow all var
                    varlist = New Dictionary(Of String, ICmdValue)(swapvar)
                    swapvar = Nothing
                    Return If(tempFuncRet, CmdNull.get)
                End If
                MsgBox($"Negative function lookup for {cmd.funcName} failed.")
                Return CmdNull.get
            End If
            If definedFunc.ContainsKey(cmd.funcName) Then
                'Shadow all var
                swapvar = New Dictionary(Of String, ICmdValue)(varlist)
                varlist.Clear()
                For i As Integer = 0 To cmd.args.Count - 1
                    setVariable(i.ToString(), cmd.args(i))
                Next
                run(definedFunc(cmd.funcName))
                'Un-shadow all var
                varlist = New Dictionary(Of String, ICmdValue)(swapvar)
                swapvar = Nothing
                Return If(tempFuncRet, CmdNull.get)
            End If
            For Each item As FunctionHandler In CollectionHelper.reverse(funcHnd)
                Dim ret As ICmdValue = item(cmd.funcName, cmd.args, cmd)
                If ret IsNot Nothing Then
                    Return ret
                End If
            Next
            MsgBox($"Function lookup for {cmd.funcName} failed.")
            Return CmdNull.get
        ElseIf TypeOf c Is CmdOpInvoke Then
            Dim cmd As CmdOpInvoke = CType(c, CmdOpInvoke)
            Select Case cmd.opor
                Case CmdOpInvoke.CmdOpOps.ASS
                    If TypeOf cmd.operand1 Is CmdVarCall Then
                        setVariable(CType(cmd.operand1, CmdVarCall).var, eval(cmd.operand2))
                        Return cmd.operand2
                    ElseIf TypeOf cmd.operand1 Is CmdClassInvoke Then
                        Dim inv As CmdClassInvoke = DirectCast(cmd.operand1, CmdClassInvoke)
                        Dim evalRes As ICmdValue = eval(inv.invokeOn)
                        If TypeOf evalRes Is CmdClassWrap AndAlso TypeOf inv.invoke Is CmdUnknown Then
                            If DirectCast(inv.invoke, CmdUnknown).what.StartsWith("__") Then
                                MsgBox("Cannot modify attributes starting with __.")
                                Return CmdNull.get
                            End If
                            If DirectCast(evalRes, CmdClassWrap).props.ContainsKey(DirectCast(inv.invoke, CmdUnknown).what) Then
                                DirectCast(evalRes, CmdClassWrap).props(DirectCast(inv.invoke, CmdUnknown).what) = eval(cmd.operand2)
                            Else
                                DirectCast(evalRes, CmdClassWrap).props.Add(DirectCast(inv.invoke, CmdUnknown).what, eval(cmd.operand2))
                            End If
                        End If
                    ElseIf TypeOf cmd.operand1 Is CmdArrayAccess Then
                        Dim acc As CmdArrayAccess = DirectCast(cmd.operand1, CmdArrayAccess)
                        Dim invocationRes As ICmdValue = eval(acc.invoke)
                        If TypeOf invocationRes Is CmdArrayType Then
                            If DirectCast(invocationRes, CmdArrayType).items.Count < DirectCast(invocationRes, CmdArrayType).bounds AndAlso DirectCast(invocationRes, CmdArrayType).items.Count < CInt(asNum(acc.index).value) Then
                                DirectCast(invocationRes, CmdArrayType).items.Add(eval(cmd.operand2))
                            Else
                                DirectCast(invocationRes, CmdArrayType).items(CInt(asNum(acc.index).value)) = eval(cmd.operand2)
                            End If
                        End If
                    End If
                Case CmdOpInvoke.CmdOpOps.ADD
                    If TypeOf cmd.operand1 Is CmdStrLiteral Then
                        Return New CmdStrLiteral With {.str = cmd.operand1.str(Me) & cmd.operand2.str(Me)}
                    End If
                    Return New CmdValAbs With {.value = asNum(cmd.operand1).value + asNum(cmd.operand2).value}
                Case CmdOpInvoke.CmdOpOps.SUB
                    Return New CmdValAbs With {.value = asNum(cmd.operand1).value - asNum(cmd.operand2).value}
                Case CmdOpInvoke.CmdOpOps.MUL
                    Return New CmdValAbs With {.value = asNum(cmd.operand1).value * asNum(cmd.operand2).value}
                Case CmdOpInvoke.CmdOpOps.DIV
                    Return New CmdValAbs With {.value = asNum(cmd.operand1).value / asNum(cmd.operand2).value}
            End Select
            Return CmdNull.get
        ElseIf TypeOf c Is CmdVarCall Then
            Return getVariable(CType(c, CmdVarCall).var)
        ElseIf TypeOf c Is CmdClassInvoke Then
            Dim clsInv As CmdClassInvoke = DirectCast(c, CmdClassInvoke)
            Dim toinvoke As ICmdValue = eval(clsInv.invokeOn)
            If TypeOf toinvoke Is CmdClassWrap Then
                Dim wrap As CmdClassWrap = DirectCast(toinvoke, CmdClassWrap)
                Dim found As ClassHandler = If(classHnd.ContainsKey(wrap.className), classHnd(wrap.className), Nothing)
                If found Is Nothing Then
                    Throw New NotImplementedException
                Else
                    If TypeOf clsInv.invoke Is CmdFuncCall Then
                        Dim func As CmdFuncCall = DirectCast(clsInv.invoke, CmdFuncCall)
                        Dim invocation As Func(Of List(Of ICmdValue), ICmdValue) = If(found?.memberFunc?.ContainsKey(func.funcName), found?.memberFunc?(func.funcName), If(found?.staticFunc?.ContainsKey(func.funcName), found?.staticFunc?(func.funcName), Nothing))
                        Return If(invocation?((Function(a As List(Of ICmdValue))
                                                   a.Insert(0, wrap)
                                                   Return a
                                               End Function)(func.args)), CmdNull.get)
                    ElseIf TypeOf clsInv.invoke Is CmdUnknown Then
                        Return If(wrap.props.ContainsKey(DirectCast(clsInv.invoke, CmdUnknown).what), wrap.props(DirectCast(clsInv.invoke, CmdUnknown).what), CmdNull.get)
                    End If
                End If
            ElseIf TypeOf toinvoke Is CmdUnknown Then
                Dim name As CmdUnknown = DirectCast(toinvoke, CmdUnknown)
                Dim found As ClassHandler = If(classHnd.ContainsKey(name.what), classHnd(name.what), Nothing)
                If found Is Nothing Then
                    Throw New NotImplementedException
                Else
                    If TypeOf clsInv.invoke Is CmdFuncCall Then
                        Dim func As CmdFuncCall = DirectCast(clsInv.invoke, CmdFuncCall)
                        Dim invocation As Func(Of List(Of ICmdValue), ICmdValue) = If(found.staticFunc.ContainsKey(func.funcName), found.staticFunc(func.funcName), Nothing)
                        Return invocation?(func.args)
                    End If
                End If
            ElseIf doInlineBoxing Then
                Return eval(New CmdClassInvoke With {.invokeOn = New CmdFuncCall With {.funcName = "box", .args = New List(Of ICmdValue) From {clsInv.invokeOn}, .argString = ""}, .invoke = clsInv.invoke})
            Else
                Return CmdNull.get
            End If
        ElseIf TypeOf c Is CmdArrayAccess Then
            Dim access As CmdArrayAccess = DirectCast(c, CmdArrayAccess)
            Dim result As ICmdValue = eval(access.invoke)
            If TypeOf result Is CmdArrayType Then
                Dim index As Integer = CInt(asNum(access.index).value)
                Dim rslt As CmdArrayType = DirectCast(result, CmdArrayType)
                Return If(index < rslt.bounds AndAlso index >= 0 AndAlso index < rslt.items.Count, rslt.items(index), boolCtor(False))
            End If
        End If
        Return c
    End Function
    Public Sub run(cmd As ICmdValue)
        If TypeOf cmd Is CmdScopeBlock Then
            run(DirectCast(cmd, CmdScopeBlock).ops)
            Return
        End If
        setVariable("it", eval(cmd))
    End Sub
    Public Sub run(cmd As List(Of ICmdValue))
        For Each item As ICmdValue In cmd
            run(item)
        Next
    End Sub
End Class
