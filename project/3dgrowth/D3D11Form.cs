﻿using System;
using System.Windows.Forms;
using SlimDX.Direct3D11;

namespace _3dgrowth
{
    public class D3D11Form : System.Windows.Forms.Form
    {
        private int _fps;
        private int _oldTime;
        private bool _isWire;
        private FPSTimer _timer;

        private DeviceSetting _deviceSetting = new DeviceSetting();
        private RenderTargetting _renderTargetting;
        private HitSphere _base;
        private RayCast _objectController;

        private System.Windows.Forms.Label label1;

        public D3D11Form()
        {
            InitializeComponent();
            _timer = new FPSTimer(this);
        }

        public void Run()
        {
            Show();
            _deviceSetting.InitializeDevice(this);
            _renderTargetting = new RenderTargetting(_deviceSetting.Device, _deviceSetting.SwapChain, Width, Height);
            InitializeViewport();
            _base = new HitSphere(_deviceSetting.Device, this);
            _objectController = new RayCast();
            _objectController.SetObject(_base);
            _timer.ontickedCallbackPerFrame += MainLoop;
            _timer.StartTimer();
        }

        private void MainLoop()
        {
            _renderTargetting.Clear();
            _objectController.OnUpdate();
            _renderTargetting.PresentView();
            SetFPSView();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _deviceSetting.Dispose();
            _renderTargetting.Dispose();
            _base.Dispose();
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

        private void InitializeViewport()
        {
            _deviceSetting.Device.ImmediateContext.Rasterizer.SetViewports(
                new Viewport
                {
                    Width = ClientSize.Width,
                    Height = ClientSize.Height,
                    MaxZ = 1
                }
                );
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
