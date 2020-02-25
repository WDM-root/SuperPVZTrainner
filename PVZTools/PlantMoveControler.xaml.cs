using PVZClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PVZTools
{
    /// <summary>
    /// PlantMoveControler.xaml 的交互逻辑
    /// </summary>
    public partial class PlantMoveControler : System.Windows.Controls.UserControl
    {
        public PlantMoveControler()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            RegisterHotKey();
        }

        private void UnRegisterHotKey()
        {
            IntPtr handle = new WindowInteropHelper(Window.GetWindow(this)).Handle;
            HotKey.UnregisterHotKey(handle, 100);
            HotKey.UnregisterHotKey(handle, 101);
            HotKey.UnregisterHotKey(handle, 102);
            HotKey.UnregisterHotKey(handle, 103);
            HotKey.UnregisterHotKey(handle, 104);
            HotKey.UnregisterHotKey(handle, 105);
            HotKey.UnregisterHotKey(handle, 106);
            HotKey.UnregisterHotKey(handle, 107);
        }

        private void RegisterHotKey()
        {
            IntPtr handle = new WindowInteropHelper(Window.GetWindow(this)).Handle;
            HotKey.RegisterHotKey(handle, 100, HotKey.KeyModifiers.None, (int)Keys.Up);
            HotKey.RegisterHotKey(handle, 101, HotKey.KeyModifiers.None, (int)Keys.Down);
            HotKey.RegisterHotKey(handle, 102, HotKey.KeyModifiers.None, (int)Keys.Left);
            HotKey.RegisterHotKey(handle, 103, HotKey.KeyModifiers.None, (int)Keys.Right);
            HotKey.RegisterHotKey(handle, 104, HotKey.KeyModifiers.None, (int)Keys.W);
            HotKey.RegisterHotKey(handle, 105, HotKey.KeyModifiers.None, (int)Keys.S);
            HotKey.RegisterHotKey(handle, 106, HotKey.KeyModifiers.None, (int)Keys.A);
            HotKey.RegisterHotKey(handle, 107, HotKey.KeyModifiers.None, (int)Keys.D);

            HwndSource source = HwndSource.FromHwnd(handle);
            source.AddHook(HotKeyHook);
        }


        private IntPtr HotKeyHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            if (msg == WM_HOTKEY)
            {
                var plant1 = new PVZ.Plant((int)NudPlantIndex1.Value);
                var plant2 = new PVZ.Plant((int)NudPlantIndex2.Value);
                switch (wParam.ToInt32())
                {
                    case 100:
                        plant1.Row--;
                        break;
                    case 101:
                        plant1.Row++;
                        break;
                    case 102:
                        plant1.Column--;
                        break;
                    case 103:
                        plant1.Column++;
                        break;
                    case 104:
                        plant2.Row--;
                        break;
                    case 105:
                        plant2.Row++;
                        break;
                    case 106:
                        plant2.Column--;
                        break;
                    case 107:
                        plant2.Column++;
                        break;
                }
                var pos = new System.Drawing.Point(plant1.Row, plant1.Column);
                PVZ.RCToXY(ref pos);
                plant1.X = pos.X;
                plant1.Y = pos.Y;
                pos = new System.Drawing.Point(plant2.Row, plant2.Column);
                PVZ.RCToXY(ref pos);
                plant2.X = pos.X;
                plant2.Y = pos.Y;
                Refresh();
            }
            return IntPtr.Zero;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            UnRegisterHotKey();
        }

        private void NudPlantIndex1_ValueChanged(object sender, EventArgs e)
        {
            if (IsLoaded) Refresh();
        }

        private void Refresh()
        {
            var plant = new PVZ.Plant((int)NudPlantIndex1.Value);
            TBSelPlant1.Text = plant.Type.GetDescription();
            TBSelPlantRow1.Text = $"{plant.Row}行";
            TBSelPlantColumn1.Text = $"{plant.Column}列";
            plant = new PVZ.Plant((int)NudPlantIndex2.Value);
            TBSelPlant2.Text = plant.Type.GetDescription();
            TBSelPlantRow2.Text = $"{plant.Row}行";
            TBSelPlantColumn2.Text = $"{plant.Column}列";
        }

        private void NudPlantIndex2_ValueChanged(object sender, EventArgs e)
        {
            if (IsLoaded) Refresh();
        }
    }
}
