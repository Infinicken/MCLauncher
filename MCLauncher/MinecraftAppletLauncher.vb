Option Strict On

Public Class MinecraftAppletLauncher
    Public Shared currentMCProcess As Process

    Public Shared Function launchMC(username As String, version As String, jarLoc As String, ParamArray arguments As String()) As Process
        Dim p As New Process
        p.StartInfo = New ProcessStartInfo("javaw", DependencyLoader.listAllDepsAsJavaCmdLine(Application.StartupPath + "\mc", Application.StartupPath + "\mc\ver\" & version & ".jar") + " " + MCLauncherHelper.formatDetailsAsJavaCmdLine(PremiumVerifier.Username, version, Application.StartupPath + "\mc", Application.StartupPath + "\mc\assets", PremiumVerifier.AccessToken, PremiumVerifier.getPlayerUUID(PremiumVerifier.Username))) With {.UseShellExecute = False, .RedirectStandardOutput = True}
        Console.WriteLine("Running Minecraft with arguments " & p.StartInfo.Arguments)
        p.Start()
        currentMCProcess = p
        Return p
    End Function
End Class
