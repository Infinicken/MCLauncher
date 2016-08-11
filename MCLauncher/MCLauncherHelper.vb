Option Strict On

Public Class MCLauncherHelper
    Public Shared Function formatDetailsAsJavaCmdLine(username As String, version As String, gameDir As String, assetsDir As String, accessToken As String, uuid As String) As String
        Return "--username " & username & " --version " & version & " --gameDir " & gameDir & " --assetsDir " & assetsDir & " --assetIndex " & version & " --accessToken " & accessToken & " --uuid " & uuid & " --userType mojang --versionType release --userProperties {""twitch_access_token"":[""forthesakeofversionsbeforeonepointninecompatability""]}"
    End Function
End Class
