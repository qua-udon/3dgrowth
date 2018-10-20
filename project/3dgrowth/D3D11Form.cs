﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3dgrowth
{
    public class D3D11Form : System.Windows.Forms.Form
    {
        private int _fps;
        private int _oldTime;
        private FPSTimer _timer;

        private DeviceSetting _deviceSetting = new DeviceSetting();
        private RenderTargetting _renderTargetting;
        private DrawTriangle _drawTriangle = new DrawTriangle();

        private System.Windows.Forms.Label label1;

        public D3D11Form()
        {
            InitializeComponent();
            _deviceSetting.InitializeDevice(this);
            _renderTargetting = new RenderTargetting(_deviceSetting.Device, _deviceSetting.SwapChain);
            _timer = new FPSTimer(this);
        }

        public void Run()
        {
            Show();
            _timer.ontickedCallbackPerFrame += () => _renderTargetting.Draw();
            _timer.ontickedCallbackPerFrame += () => _drawTriangle.Draw(_deviceSetting.Device);
            _timer.ontickedCallbackPerFrame += SetFPSView;
            _timer.StartTimer();
        }



        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _deviceSetting.Dispose();
            _renderTargetting.Dispose();
            _drawTriangle.Dispose();
            base.OnFormClosed(e);
        }

        private void SetFPSView()
        {
            _fps++;
            if (Environment.TickCount >= _oldTime + 1000)
            {
                _oldTime = Environment.TickCount;
                label1.Text = _fps.ToString();
                _fps = 0;
            }
        }

        #region GUI
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(227, 229);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // D3D11Form
            // 
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Controls.Add(this.label1);
            this.Name = "D3D11Form";
            this.Load += new System.EventHandler(this.D3D11Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void D3D11Form_Load(object sender, EventArgs e)
        {

        }
        #endregion
    }
}
