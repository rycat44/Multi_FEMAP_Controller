Public Class Form1

    Dim App As femap.model
    Dim appObj As femap.model 'The high-level application object we use to get other FEMAP instances
    Dim rc As femap.zReturnCode
    Dim vPID, vActWind, vVersion As Object

    Private Sub RectangleShape1_Click(sender As Object, e As EventArgs) Handles RectangleShape1.Click
        '***********************************************************************
        '  Your code here running with the desired FEMAP instance
        '***********************************************************************
        Dim itemIndex As Long
        Dim pid As Long
        Dim nthing As Object

        itemIndex = ListView1.SelectedIndices(0)

        pid = vPID(itemIndex)

        appObj = Nothing
        rc = App.feAppGetRunningApplication("Femap", pid, nthing, appObj)

        rc = appObj.feAppMessage(femap.zMessageColor.FCM_NORMAL, "Attached to PID: " + pid.ToString)

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DoRefresh()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        DoRefresh()
    End Sub

    Public Sub DoRefresh()
        Try
            App = GetObject(, "femap.model")

            rc = App.feAppRunningApplicationInfo("Femap", vPID, vActWind, vVersion)

            Dim n As Long
            Dim v As Object
            Dim sName As String = ""

            Dim pIDset As femap.Set
            pIDset = App.feSet

            For i = 0 To UBound(vPID)

                Dim nthing As Object

                rc = App.feAppGetRunningApplication("Femap", vPID(i), nthing, appObj)

                rc = appObj.feAppMessage(0, "Hello, FEMAP")

                If pIDset.IsAdded(vPID(i)) Then
                    GoTo doNext
                Else
                    pIDset.Add(vPID(i))
                End If


                Try
                    v = Nothing
                    rc = appObj.feAppGetAllModels(n, v)
                    For j = 0 To UBound(v)
                        rc = appObj.feAppGetModelName(v(j), sName)

                        Dim item As New ListViewItem(sName)
                        item.SubItems.Add(vPID(i).ToString)
                        ListView1.Items.Add(item)
                    Next

                Catch ex As Exception
                    rc = 1
                End Try
doNext:
                appObj = Nothing

            Next


        Catch ex As Exception
            Dim itemEx As New ListViewItem("Exception:")
            itemEx.SubItems.Add(ex.ToString)
            ListView1.Items.Add(itemEx)

        End Try
    End Sub
End Class
