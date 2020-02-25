Imports PVZClass

Public Class OperationWindow
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        Application.ChangeLanguage(Content)
    End Sub

    Public scale As Integer = 100

    Private Sub Window_PreviewMouseWheel(sender As Object, e As MouseWheelEventArgs)
        If Keyboard.Modifiers = ModifierKeys.Control Then
            If e.Delta > 0 Then
                scale += 5
            Else
                scale -= 5
            End If
            scale = Math.Max(10, scale)
            scale = Math.Min(300, scale)
            Dim con As UIElement = Content
            con.RenderTransform = New ScaleTransform(scale / 100, scale / 100)
            Height = 470 * scale / 100
            Width = 400 * scale / 100
        End If
    End Sub

    Private Sub Window_MouseDown(sender As Object, e As MouseButtonEventArgs)
        Try
            DragMove()
        Catch ex As InvalidOperationException
        End Try
    End Sub

    Private Sub Window_Closed(sender As Object, e As EventArgs)
        If Not IsNothing(Application.Current.MainWindow) Then
            CType(Application.Current.MainWindow, MainWindow).BtnOperate.IsEnabled = True
        End If
    End Sub

    Private Sub BtnResumeLawnmover_Click(sender As Object, e As RoutedEventArgs)
        PVZ.ResumeLawnmover()
    End Sub

    Private Sub BtnStartLawnmover_Click(sender As Object, e As RoutedEventArgs)
        PVZ.StartLawnmover()
    End Sub

    Private Sub BtnCreatePlant_Click(sender As Object, e As RoutedEventArgs)
        Dim plant As PVZ.Plant = PVZ.CreatePlant(CBPlantTypes.SelectedIndex, NudRow.Value - 1, NudColumn.Value - 1, CBPlantIsImitate.IsChecked.Value)
        Dim hp As Integer = Convert.ToDouble(TBPlantHp.Text)
        If hp > 0 Then
            plant.MaxHp = hp
            plant.Hp = hp
        End If
        If CBPlantIsFixed.IsChecked = True Then
            plant.Fix()
        End If
    End Sub

    Private Sub BtnCreateZombie_Click(sender As Object, e As RoutedEventArgs)
        Dim zombie As PVZ.Zombie = PVZ.CreateZombie(CBZombieTypes.SelectedIndex, NudRow.Value - 1, NudColumn.Value - 1)
        Dim hp As Integer = Convert.ToDouble(TBZombieBodyHp.Text)
        If hp > 0 Then
            zombie.MaxBodyHP = hp
            zombie.BodyHP = hp
        End If
        hp = Convert.ToDouble(TBZombieHatHp.Text)
        If hp > 0 Then
            zombie.MaxAccessoriesType1HP = hp
            zombie.AccessoriesType1HP = hp
        End If
        hp = Convert.ToDouble(TBZombieShieldHp.Text)
        If hp > 0 Then
            zombie.MaxAccessoriesType2HP = hp
            zombie.AccessoriesType2HP = hp
        End If
        If CBZombieIsHypnotized.IsChecked = True Then
            zombie.Hypnotized = True
        End If
    End Sub

    Private Sub BtnCreateLadder_Click(sender As Object, e As RoutedEventArgs)
        PVZ.CreateLadder(NudRow.Value - 1, NudColumn.Value - 1)
    End Sub

    Private Sub BtnCreateGrave_Click(sender As Object, e As RoutedEventArgs)
        PVZ.CreateGrave(NudRow.Value - 1, NudColumn.Value - 1)
    End Sub

    Private Sub BtnAutoLadder_Click(sender As Object, e As RoutedEventArgs)
        For Each griditem In PVZ.AllGriditems
            If griditem.Type = PVZ.GriditemType.Ladder Then
                griditem.Exist = False
            End If
        Next
        For Each plant In PVZ.AllPlants
            If plant.Type = PVZ.PlantType.Pumpkin Then
                PVZ.CreateLadder(plant.Row, plant.Column)
            End If
        Next
    End Sub

    Private Sub BtnCreatRake_Click(sender As Object, e As RoutedEventArgs)
        PVZ.CreateRake(NudRow.Value - 1, NudColumn.Value - 1)
    End Sub
End Class
