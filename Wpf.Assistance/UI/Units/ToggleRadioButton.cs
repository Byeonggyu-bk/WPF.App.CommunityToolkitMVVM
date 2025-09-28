using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Wpf.Assistance.UI.Units
{
    public class ToggleRadioButton : RadioButton
    {
        static ToggleRadioButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToggleRadioButton), new FrameworkPropertyMetadata(typeof(ToggleRadioButton)));
        }
    }
}
