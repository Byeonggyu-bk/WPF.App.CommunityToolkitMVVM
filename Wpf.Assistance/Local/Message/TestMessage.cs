using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Wpf.Assistance.Local.Message
{

    public class ColorMessage
    {
        public Brush Color { get; set; }

        public ColorMessage(Brush color) { Color = color; }
    }
}
