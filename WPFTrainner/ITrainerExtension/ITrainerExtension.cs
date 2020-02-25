using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ITrainerExtension
{
    public interface ITrainerExtensionItem
    {
        string Text { get; }
        string ToolTip { get; }
    }
    public interface ITrainerExtensionButton : ITrainerExtensionItem
    {
        void ButtonOnClick();
    }
    public interface IITrainerExtensionCheckBox : ITrainerExtensionItem
    {
        void CheckBoxOnClick(bool ischecked);
    }

    public interface ITrainerExtensionTextBox : ITrainerExtensionItem
    {
        void FunctionOnCall(string text);
    }
    public interface ITrainerExtensionUserControl : ITrainerExtensionItem
    {
        void Layout(Window owner, Canvas canvas);
    }

}
