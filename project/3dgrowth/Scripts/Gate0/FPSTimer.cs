using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3dgrowth
{
    /// <summary>
    /// ゲート0:FPS計測
    /// </summary>
    public class FPSTimer
    {
        private const double WAIT_TIME = 1000.0f / 60.0f;

        public Action ontickedCallbackPerFrame;

        private readonly D3D11Form _form;

        public FPSTimer(D3D11Form form)
        {
            _form = form;
        }

        public void StartTimer()
        {
            var targetTime = (double)Environment.TickCount;
            targetTime += WAIT_TIME;
            while (_form.Created)
            {
                if ((double)Environment.TickCount >= targetTime)
                {
                    //メインの処理
                    ontickedCallbackPerFrame?.Invoke();

                    targetTime += WAIT_TIME;
                }
                System.Threading.Thread.Sleep(1);
                Application.DoEvents();
            }
        }
    }
}
