using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3dgrowth
{
    public class D3D11Form : System.Windows.Forms.Form
    {
        private DeviceSetting _deviceSetting = new DeviceSetting();

        public D3D11Form()
        {
            _deviceSetting.InitializeDevice(this);
        }
    }
}
