using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3dgrowth
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var form = new D3D11Form();
            form.Show();

            var timer = new FPSTimer(form);
            timer.StartTimer();
        }
    }
}
