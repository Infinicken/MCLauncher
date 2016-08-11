Namespace My
    ' The following events are available for MyApplication:
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication
        Protected Sub onclose() Handles Me.Shutdown
            Logger.log("Application is shutting down!", Logger.LogLevel.INFO)
            ConfigManager.writeToConfig()
            Logger.stop()
        End Sub
        Protected Sub boot() Handles Me.Startup
            Logger.log("Application is booting up!", Logger.LogLevel.INFO)
            I18n.loadTranslationsFromFileIntoMappings()
            ConfigManager.readFromConfig()
        End Sub
        Protected Sub onErr(sender As Object, e As ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
            Dim errFile As New Text.StringBuilder
            errFile.AppendLine("==== CRASH REPORT ====")
            errFile.AppendLine(String.Format("{0} :{1}" & vbCrLf & "{2}", e.Exception.GetType.ToString, e.Exception.Message, e.Exception.StackTrace))
            errFile.AppendLine("==== END OF STACKTRACE ====")
            errFile.AppendLine(String.Format("Command Line: {0}", Environment.CommandLine))
            errFile.AppendLine(String.Format("OS Version: {0}", Environment.OSVersion.VersionString))
            errFile.AppendLine(String.Format("CPU Processors: {0}", Environment.ProcessorCount))
            errFile.AppendLine(String.Format("System Architecture: {0}", If(Not Environment.Is64BitOperatingSystem, "x86", "x64")))
            Dim premIsVerify As Boolean = PremiumVerifier.getAccessTokenValid(PremiumVerifier.AccessToken, PremiumVerifier.ClientToken, False)
            errFile.AppendLine(String.Format("Is Premium: {0}, {1}", PremiumVerifier.VerifyType.ToString, premIsVerify.ToString))
            If premIsVerify Then errFile.AppendLine(String.Format("Premium Verification Info: {0}", BasicEncryption.func_46293525_1_(PremiumVerifier.getUserInfo())))
            errFile.Append("==== END OF CRASH REPORT ====")
            Dim crashFilePath As String = "crash_" & String.Format("{0}_{1}_{2}_{3}_{4}_{5}", Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute, Now.Second) & ".txt"
            Dim crashFile As New IO.StreamWriter(crashFilePath, False, Text.Encoding.UTF8)
            crashFile.Write(errFile.ToString)
            crashFile.Close()
            crashFile.Dispose()
            MsgBox("Application crashed! See " & crashFilePath & " for crash report.")
            Logger.log("Application crashed!", Logger.LogLevel.FATAL)
            Logger.log(errFile.ToString, Logger.LogLevel.FATAL)
            Logger.stop()
        End Sub
    End Class
End Namespace
