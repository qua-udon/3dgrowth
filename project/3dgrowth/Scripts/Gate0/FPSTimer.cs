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
        public double GlobalTime;

        public Action onStart;
        public Action ontickedCallbackPerFrame;

        private readonly D3D11Form _form;

        public FPSTimer(D3D11Form form)
        {
            _form = form;
        }

        public void StartTimer()
        {
            onStart?.Invoke();
            GlobalTime = (double)Environment.TickCount;
            GlobalTime += WAIT_TIME;
            while (_form.Created)
            {
                if ((double)Environment.TickCount >= GlobalTime)
                {
                    //メインの処理
                    ontickedCallbackPerFrame?.Invoke();

                    GlobalTime += WAIT_TIME;
                }
                System.Threading.Thread.Sleep(1);
                Application.DoEvents();
            }
        }
    }
}
