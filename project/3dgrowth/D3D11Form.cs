using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3dgrowth
{
    public class D3D11Form : System.Windows.Forms.Form
    {
        private int _fps;
        private int _oldTime;
        private System.Windows.Forms.Label label1;
        private DeviceSetting _deviceSetting = new DeviceSetting();

        public D3D11Form()
        {
            InitializeComponent();
            _deviceSetting.InitializeDevice(this);
        }

        public void SetFPSView()
        {
            _fps++;
            if (Environment.TickCount >= _oldTime + 1000)
            {
                _oldTime = Environment.TickCount;
                label1.Text = _fps.ToString();
                _fps = 0;
            }
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(141, 126);
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
    }
}
