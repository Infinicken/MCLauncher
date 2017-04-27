Imports System.IO
Imports Newtonsoft.Json

Public Class AssetsManager
    Public Shared Sub downloadForVersion(ver As String, dir As String)
        If Not Directory.Exists(Path.GetFullPath(dir) + "\assets\indexes") Then
            Directory.CreateDirectory(Path.GetFullPath(dir) + "\assets\indexes")
        End If
        If Not Directory.Exists(Path.GetFullPath(dir) + "\assets\objects") Then
            Directory.CreateDirectory(Path.GetFullPath(dir) + "\assets\objects")
        End If
        Dim a As AssetsJson = getAssetJsonForVersion(dir, ver)
        MsgBox(a.objects.Count)
        For Each item In a.objects
            If Not Directory.Exists(Path.GetFullPath(dir) & "\assets\objects\" & item.Value.hash.Substring(0, 2)) Then
                Directory.CreateDirectory(Path.GetFullPath(dir) & "\assets\objects\" & item.Value.hash.Substring(0, 2))
            End If
            BatchFileDownload.addDownload(item.Value.hash, "http://resources.download.minecraft.net/" & item.Value.hash.Substring(0, 2) & "/" & item.Value.hash, Path.GetFullPath(dir) & "\assets\objects\" & item.Value.hash.Substring(0, 2) & "\" & item.Value.hash)
        Next
        BatchDisplay.Show()
        MsgBox(I18n.translate("info.assets.finish", ver))
    End Sub

    Public Shared Sub downloadForgeFromProvider(ver As String, dir As String)
        If MsgBox(I18n.translate("warn.fromProvider"), CType(vbYesNo + vbCritical, Global.Microsoft.VisualBasic.MsgBoxStyle), I18n.translate("warn.dlForge")) = vbNo Then
            Return
        End If
    End Sub

    Public Shared Function getAssetIndexForVersion(dir As String, ver As String) As String
        Return DependencyLoader.getVersionJSONForVersion(dir, ver).assets
    End Function

    Public Shared Function getAssetJsonForVersion(dir As String, ver As String) As AssetsJson
        Dim toFetch As String = DependencyLoader.getVersionJSONForVersion(dir, ver).assets
        If Not File.Exists(Path.GetFullPath(dir) + "\assets\indexes\" & toFetch & ".json") Then
            Dim dl As New FileDownload
            dl.StartDownloadingAwait("https://s3.amazonaws.com/Minecraft.Download/indexes/" & toFetch & ".json", Path.GetFullPath(dir) + "\assets\indexes\" & toFetch & ".json")
        End If
        Dim json As String = (New StreamReader(Path.GetFullPath(dir) & "\assets\indexes\" & toFetch & ".json")).ReadToEnd()
        Return JsonConvert.DeserializeObject(Of AssetsJson)(json)
    End Function
End Class

Public Class AssetsJson
    Public Class Asset
        Public hash As String
        Public size As Integer
    End Class
    Public objects As Dictionary(Of String, Asset)
End Class
