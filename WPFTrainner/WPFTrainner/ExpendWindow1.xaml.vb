Public Class ExpendWindow
    Private Sub Window_MouseMove(sender As Object, e As MouseEventArgs)
        If e.GetPosition(Me).Y < 50 Then
            Try
                DragMove()
            Catch ex As InvalidOperationException
            End Try
        End If
    End Sub
    Private Sub BtnExit_Click(sender As Object, e As RoutedEventArgs)
        Owner.ShowInTaskbar = True
        Owner.WindowState = WindowState.Normal
        Close()
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        BtnExit.Style = Owner.Resources("ExitButtonStyle")
    End Sub
End Class
