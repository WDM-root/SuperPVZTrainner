using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using ITrainerExtension;
using Microsoft.Win32;

namespace PVZTools
{
    public class CancelFullScreen : ITrainerExtensionButton
    {
        public string Text => "取消全屏模式";

        public string ToolTip => "可用于修复win10系统因不支持全屏而报错的问题";

        public void ButtonOnClick()
        {
            RegistryKey software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey popCap = software.OpenSubKey("PopCap");
            if (popCap != null)
            {
                RegistryKey pvz = popCap.OpenSubKey("PlantsVsZombies", true);
                pvz?.SetValue("ScreenMode", 0, RegistryValueKind.DWord);
                System.Windows.Forms.MessageBox.Show("修改成功","针对普通版本", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            RegistryKey steamPopCap = software.OpenSubKey("SteamPopCap");
            if (steamPopCap != null)
            {
                RegistryKey pvz = steamPopCap.OpenSubKey("PlantsVsZombies", true);
                pvz?.SetValue("ScreenMode", 0, RegistryValueKind.DWord);
                System.Windows.Forms.MessageBox.Show("修改成功", "针对Steam版本", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }

    public class OpenUserdata : ITrainerExtensionButton
    {
        public string Text => "打开存档目录";

        public string ToolTip => "对于xp系统,存档和游戏在同一级目录中";

        public void ButtonOnClick()
        {
            string path = @"C:\ProgramData\PopCap Games\PlantsVsZombies\userdata";
            if (Directory.Exists(path))
                Process.Start(path);
        }
    }

    public partial class LawnStringsManager : ITrainerExtensionUserControl
    {

        public string Text => "LawnStrings管理器";

        string ITrainerExtensionItem.ToolTip => "可用于动态修改LawnStrings的内容";

        public void Layout(Window owner, Canvas canvas)
        {
            //设置控件位置
            var usercon = new LawnStringsManager();
            Canvas.SetLeft(usercon, 20);
            Canvas.SetTop(usercon, 60);
            //修改窗口大小
            owner.Width = 540;
            owner.Height = 695;
            canvas.Children.Add(usercon);
        }
    }

    public class OpenTexturedFontEditor : ITrainerExtensionButton
    {
        public string Text => "纹理化字体生成工具";

        public string ToolTip => "程序来源未知(TexturedFontEditor)，配合字体转换器使用";

        TexturedFontEditor.FontEditor Instance;

        public void ButtonOnClick()
        {
            if (Instance == null)
                Instance = new TexturedFontEditor.FontEditor();
            if(Instance.IsDisposed)
                Instance = new TexturedFontEditor.FontEditor();
            Instance.Show();
        }
    }

    public class Openpvz_cnv2_font_convarter : ITrainerExtensionButton
    {
        static Openpvz_cnv2_font_convarter()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            new pvz_cnv2_font_convarter.Form1().Dispose();
        }
        public string Text => "汉化2版字体转换器";

        public string ToolTip => "作者冥谷川恋(pvz_cnv2_font_convarter)，配合纹理化字体生成工具使用";

        pvz_cnv2_font_convarter.Form1 Instance;

        public void ButtonOnClick()
        {
            if (Instance == null)
                Instance = new pvz_cnv2_font_convarter.Form1();
            if (Instance.IsDisposed)
                Instance = new pvz_cnv2_font_convarter.Form1();
            Instance.Show();
        }
    }

    public partial class PlantMoveControler : ITrainerExtensionUserControl
    {
        public string Text => "键盘控制植物";

        string ITrainerExtensionItem.ToolTip => "允许通过按键来控制场上的2个植物";

        public void Layout(Window owner, Canvas canvas)
        {
            //设置控件位置
            var usercon = new PlantMoveControler();
            Canvas.SetLeft(usercon, 20);
            Canvas.SetTop(usercon, 60);
            //修改窗口大小
            owner.Width = 340;
            owner.Height = 275;
            canvas.Children.Add(usercon);
        }
    }

    public class GameDownloadPage : ITrainerExtensionButton
    {
        public string Text => "游戏资源下载";

        public string ToolTip => "提供植物大战僵尸游戏的下载地址";

        public void ButtonOnClick()
        {
            Process.Start("www.lonelystar.org/download.htm");
        }
    }

}
