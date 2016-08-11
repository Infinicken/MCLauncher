Imports Newtonsoft.Json
Imports System.IO

Public Class DependencyLoader
    Public Shared Function listAllDepsAsJavaCmdLine(Optional path As String = "", Optional mcJar As String = "") As String
        Dim toLoadLib As String = path + "\lib"
        Dim toLoadNaive As String = path + "\native"
        Dim doMCJar As Boolean = Not mcJar = ""
        If path = "" Then toLoadLib = Application.StartupPath + "\lib" : toLoadNaive = Application.StartupPath + "\native"
        If Not IO.Directory.Exists(toLoadLib) OrElse Not IO.Directory.Exists(toLoadNaive) Then Return ""
        Dim sb As New Text.StringBuilder
        sb.Append("-Djava.library.path=" + toLoadNaive + " -cp ")
        For Each file As String In IO.Directory.GetFiles(toLoadLib, "*.jar", IO.SearchOption.AllDirectories)
            sb.Append(file + ";")
        Next
        If doMCJar Then
            sb.Append(IO.Path.GetFullPath(mcJar) + " net.minecraft.client.main.Main")
        End If
        Return sb.ToString
    End Function

    Public Shared Sub downloadForVersion(ver As String, loc As String)
        If Not Directory.Exists(Path.GetFullPath(loc) & "\lib") Then
            Directory.CreateDirectory(Path.GetFullPath(loc) & "\lib")
        Else
            For Each filepath In Directory.GetFiles(Path.GetFullPath(loc) & "\lib")
                File.Delete(filepath)
            Next
        End If
        If Not Directory.Exists(Path.GetFullPath(loc) & "\native") Then
            Directory.CreateDirectory(Path.GetFullPath(loc) & "\native")
        Else
            For Each filepath In Directory.GetFiles(Path.GetFullPath(loc) & "\native")
                File.Delete(filepath)
            Next
        End If
        Dim dl As New FileDownload
        dl.StartDownloadingAwait("https://s3.amazonaws.com/Minecraft.Download/versions/" & ver & "/" & ver & ".json", Path.GetFullPath(loc) + "\ver\" & ver & ".json")
        Dim json As VersionJSON = JsonConvert.DeserializeObject(Of VersionJSON)((New StreamReader(Path.GetFullPath(loc) + "\ver\" & ver & ".json").ReadToEnd))
        Dim osver As String = If(My.Computer.Info.OSFullName.Contains("Windows"), "windows", "osx")
        For Each dep As VersionJSON.Dependency In json.libraries
            Console.WriteLine("Downloading dependency " & dep.name)
            dl = New FileDownload
            If dep.natives Is Nothing Then
                If dep.rules IsNot Nothing Then
                    Dim ok As Boolean = True
                    For Each rule As VersionJSON.Dependency.Rule In dep.rules
                        If rule.action = "disallow" AndAlso rule.os.name = osver Then
                            ok = False
                        End If
                    Next
                    If Not ok Then Continue For
                End If
                dl.StartDownloadingAwait(dep.downloads.artifact.url, Path.GetFullPath(loc) & "\lib\" & dep.downloads.artifact.path.Split(CType("/", Char()))(dep.downloads.artifact.path.Split(CType("/", Char())).Length - 1))
            Else
                If dep.rules IsNot Nothing Then
                    Dim ok As Boolean = True
                    For Each rule As VersionJSON.Dependency.Rule In dep.rules
                        If rule.action = "disallow" AndAlso rule.os.name = osver Then
                            ok = False
                        End If
                    Next
                    If Not ok Then Continue For
                End If
                If osver = "windows" Then
                    Dim artifact As VersionJSON.Dependency.DependencyDownloadInfo.DependencyArtifact = If(dep.downloads.classifiers.windows, If(IntPtr.Size = 8, dep.downloads.classifiers.windows64, dep.downloads.classifiers.windows32))
                    If dep.extract Is Nothing Then
                        dl.StartDownloadingAwait(artifact.url, Path.GetFullPath(loc) & "\lib\" & artifact.path.Split(CType("/", Char()))(artifact.path.Split(CType("/", Char())).Length - 1))
                    Else
                        dl.StartDownloadingAwait(artifact.url, Path.GetFullPath(loc) & "\native\" & artifact.path.Split(CType("/", Char()))(artifact.path.Split(CType("/", Char())).Length - 1))
                        Dim fromJar As String = (Path.GetFullPath(loc) & "\native\" & artifact.path.Split(CType("/", Char()))(artifact.path.Split(CType("/", Char())).Length - 1))
                        Dim toZip As String = (Path.GetFullPath(loc) & "\native\" & Path.GetFileNameWithoutExtension(artifact.path.Split(CType("/", Char()))(artifact.path.Split(CType("/", Char())).Length - 1)) & ".zip")
                        Dim extDir As String = Path.GetFullPath(loc) & "\native"
                        File.Copy(fromJar, toZip)
                        'Dim unzipper As Object = Activator.CreateInstance(Type.GetTypeFromProgID("Shell.Application"))
                        'Dim output As Object = unzipper.NameSpace((Path.GetFullPath(loc) & "\native"))
                        'Dim input As Object = unzipper.NameSpace((toZip))
                        'output.CopyHere((input.Items), 4)
                        Using zip As Ionic.Zip.ZipFile = Ionic.Zip.ZipFile.Read(toZip)
                            For Each e As Ionic.Zip.ZipEntry In zip
                                e.Extract(extDir, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently)
                            Next
                        End Using
                        For Each exc As String In dep.extract.exclude
                            If Directory.Exists(Path.GetFullPath(loc) & "\native\" & exc) Then
                                Directory.Delete(Path.GetFullPath(loc) & "\native\" & exc, True)
                            End If
                        Next
                        File.Delete(fromJar)
                        File.Delete(toZip)
                    End If
                Else
                    If dep.extract Is Nothing Then
                        dl.StartDownloadingAwait(dep.downloads.classifiers.osx.url, Path.GetFullPath(loc) & "\lib\" & dep.downloads.classifiers.osx.path.Split(CType("/", Char()))(dep.downloads.classifiers.osx.path.Split(CType("/", Char())).Length - 1))
                    Else
                        dl.StartDownloadingAwait(dep.downloads.classifiers.osx.url, Path.GetFullPath(loc) & "\native\" & dep.downloads.classifiers.osx.path.Split(CType("/", Char()))(dep.downloads.classifiers.osx.path.Split(CType("/", Char())).Length - 1))
                        Dim fromJar As String = (Path.GetFullPath(loc) & "\native\" & dep.downloads.classifiers.osx.path.Split(CType("/", Char()))(dep.downloads.classifiers.osx.path.Split(CType("/", Char())).Length - 1))
                        Dim toZip As String = (Path.GetFullPath(loc) & "\native\" & Path.GetFileNameWithoutExtension(dep.downloads.classifiers.osx.path.Split(CType("/", Char()))(dep.downloads.classifiers.osx.path.Split(CType("/", Char())).Length - 1)) & ".zip")
                        Dim extDir As String = Path.GetFullPath(loc) & "\native"
                        File.Copy(fromJar, toZip)
                        'Dim unzipper As Object = Activator.CreateInstance(Type.GetTypeFromProgID("Shell.Application"))
                        'Dim output As Object = unzipper.NameSpace((Path.GetFullPath(loc) & "\native"))
                        'Dim input As Object = unzipper.NameSpace((toZip))
                        'output.CopyHere((input.Items), 4)
                        Using zip As Ionic.Zip.ZipFile = Ionic.Zip.ZipFile.Read(toZip)
                            For Each e As Ionic.Zip.ZipEntry In zip
                                e.Extract(extDir, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently)
                            Next
                        End Using
                        For Each exc As String In dep.extract.exclude
                            If Directory.Exists(Path.GetFullPath(loc) & "\native\" & exc) Then
                                Directory.Delete(Path.GetFullPath(loc) & "\native\" & exc, True)
                            End If
                        Next
                        File.Delete(fromJar)
                        File.Delete(toZip)
                    End If
                End If
            End If
        Next
    End Sub
End Class

Public Class VersionJSON
    Public Class AssetInfo
        Public id As String
        Public sha1 As String
        Public size As Integer
        Public url As String
        Public totalSize As Long
    End Class
    Public assetIndex As AssetInfo
    Public assets As String
    Public Class SidedDownloads
        Public Class SidedDownload
            Public sha1 As String
            Public size As Long
            Public url As String
        End Class
        Public client As SidedDownload
        Public server As SidedDownload
    End Class
    Public id As String
    Public Class Dependency
        Public name As String
        Public Class DependencyDownloadInfo
            Public Class DependencyArtifact
                Public size As Long
                Public sha1 As String
                Public path As String
                Public url As String
            End Class
            Public artifact As DependencyArtifact
            Public Class NativeClassifier
                <JsonProperty("natives-linux")> Public linux As DependencyArtifact
                <JsonProperty("natives-osx")> Public osx As DependencyArtifact
                <JsonProperty("natives-windows")> Public windows As DependencyArtifact
                <JsonProperty("natives-windows-32")> Public windows32 As DependencyArtifact
                <JsonProperty("natives-windows-64")> Public windows64 As DependencyArtifact
            End Class
            Public classifiers As NativeClassifier
        End Class
        Public downloads As DependencyDownloadInfo
        Public Class Rule
            Public action As String
            Public Class OSInfo
                Public name As String
            End Class
            Public os As OSInfo
        End Class
        Public rules As List(Of Rule)
        Public Class ExtractRule
            Public exclude As List(Of String)
        End Class
        Public extract As ExtractRule
        Public Class NativeInfo
            Public linux As String
            Public osx As String
            Public windows As String
        End Class
        Public natives As NativeInfo
    End Class
    Public libraries As List(Of Dependency)
    Public Class LoggingInfo
        Public Class LoggingClientInfo
            Public file As Dependency.DependencyDownloadInfo.DependencyArtifact
            Public argument As String
            Public type As String
        End Class
    End Class
    Public logging As LoggingInfo
    Public mainClass As String
    Public minecraftArguments As String
    Public minimumLauncherVersion As Short
    Public releaseTime As DateTime
    Public time As DateTime
    Public type As String
End Class
