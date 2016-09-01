Imports System.Collections.Concurrent
Imports System.Threading
''' <summary>
''' A wrapper class for managing <see cref="System.Threading.Thread"/>
''' </summary>
Public NotInheritable Class Threads
    Private Sub New()
    End Sub
    Private Shared threadPool As New ConcurrentDictionary(Of String, Thread)
    Public Shared Mutex As New Mutex
    Protected Shared taskPool As New ConcurrentDictionary(Of String, List(Of Runnable))
    Public Delegate Sub RunnableSub()
    Public Class Runnable
        Public runnable As RunnableSub
        Public name As String
        Public Sub New(name As String, runnable As RunnableSub)
            Me.name = name
            Me.runnable = runnable
        End Sub
    End Class
    ''' <summary>
    ''' Create a thread using an unique <paramref name="identifier"/>.
    ''' The thread will fail to create if the name is not unique.
    ''' </summary>
    ''' <param name="identifier">The identifier to reference the thread in the future.</param>
    Public Shared Sub createThread(identifier As String)
        Mutex.WaitOne()
        Try
            If threadPool.TryAdd(identifier, New Thread(Sub()
                                                            While True
                                                                If taskPool.ContainsKey(identifier) AndAlso taskPool(identifier).Count > 0 Then
                                                                    taskPool(identifier)(0).runnable.Invoke
                                                                    taskPool(identifier).RemoveAt(0)
                                                                Else
                                                                    Thread.Sleep(500)
                                                                End If
                                                            End While
                                                        End Sub) With {.IsBackground = True, .Name = $"THREAD-{identifier}", .Priority = ThreadPriority.BelowNormal}) = False Then Throw New Exception()
            threadPool(identifier).Start()
            'Console.WriteLine($"Created thread {identifier}!")
        Catch ex As Exception
            Console.WriteLine($"Failed to create thread {identifier}!")
        Finally
            Mutex.ReleaseMutex()
        End Try
    End Sub

    ''' <summary>
    ''' Add a scheduled task to the thread of <paramref name="identifier"/>.
    ''' </summary>
    ''' <param name="identifier">The identifier of the thread.</param>
    ''' <param name="name">The name of the task.</param>
    ''' <param name="[delegate]">The <seealso cref="Runnable"/> to add to the thread.</param>
    Public Shared Sub addScheduledTask(identifier As String, name As String, [delegate] As RunnableSub)
        Mutex.WaitOne()
        SyncLock threadPool
            SyncLock taskPool
                Try
                    If threadPool.ContainsKey(identifier) Then
                        If taskPool.ContainsKey(identifier) Then
                            taskPool(identifier).Add(New Runnable(name, [delegate]))
                        Else
                            taskPool.TryAdd(identifier, New List(Of Runnable))
                            taskPool(identifier).Add(New Runnable(name, [delegate]))
                        End If
                        'Console.WriteLine($"Scheduled task for {identifier}!")
                    End If
                Catch ex As ArgumentException
                    Console.WriteLine($"Failed to schedule task for {identifier}!")
                Finally
                    Mutex.ReleaseMutex()
                End Try
            End SyncLock
        End SyncLock
    End Sub

    ''' <summary>
    ''' Kills all threads in the pool. You monster.
    ''' Also clears all scheduled task assigned to them.
    ''' </summary>
    Public Shared Sub killAllThread()
        For Each t As KeyValuePair(Of String, Thread) In threadPool
            t.Value.Abort()
            'Console.WriteLine($"Killed thread {t.Value} in pool!")
        Next
        threadPool.Clear()
        taskPool.Clear()
    End Sub

    ''' <summary>
    ''' Kills the thread specified by the <paramref name="identifier"/>
    ''' </summary>
    ''' <param name="identifier">The identifier of the thread to kill.</param>
    Public Shared Sub killThread(identifier As String)
        If threadPool.ContainsKey(identifier) Then
            threadPool(identifier).Abort()
            threadPool.TryRemove(identifier, Nothing)
            'Console.WriteLine($"The thread {identifier} is now terminated!")
        End If
    End Sub

    ''' <summary>
    ''' Checks if the thread specified by the <paramref name="identifier"/> is in the thread pool.
    ''' </summary>
    ''' <param name="identifier">The identifier of the thread to check.</param>
    ''' <returns>Returns true if the thread is in the thread pool, false otherwise.</returns>
    Public Shared Function hasThread(identifier As String) As Boolean
        Mutex.WaitOne()
        Dim retBl As Boolean = threadPool.ContainsKey(identifier)
        Mutex.ReleaseMutex()
        Return retBl
    End Function
End Class
