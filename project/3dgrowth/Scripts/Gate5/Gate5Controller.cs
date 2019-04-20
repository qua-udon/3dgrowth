using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SlimDX;
using SlimDX.Direct3D11;

namespace _3dgrowth
{
    public class Gate5Controller
    {
        private DrawCube _ground;
        private DrawCube _plane;
        private FBXRenderer _ship;
        private DrawSphere _sphere1;
        private DrawSphere _sphere2;

        public Gate5Controller(Device device, Form form)
        {
            _ground = new DrawCube(device, form);
            _ground.SetScale(20f);
            _ground.SetPosition(new Vector3(0f, -1f, 0f));
            _ground.SetRotation(new Vector3((float)(Math.PI * (float)(90f / 180f)), 0f, 0f));
            _plane = new DrawCube(device, form);
            _plane.SetPosition(new Vector3(0f, 1f, 0f));
            _sphere1 = new DrawSphere(device, form);
            _sphere1.SetPosition(new Vector3(2.5f, 2f, -0.5f));
            _sphere2 = new DrawSphere(device, form);
            _sphere2.SetPosition(new Vector3(2.75f, 4f, -2f));
            _ship = new FBXRenderer(device, form, System.AppDomain.CurrentDomain.BaseDirectory + "space_ship.fbx");
            _ship.SetPosition(new Vector3(-2f, 0f, 2f));
        }

        public void OnUpdate(Vector3 position)
        {
            _ground.SetCamera(position);
            _ground.InitializeContent();
            _ground.SetView();
            _ground.Draw();

            _plane.SetCamera(position);
            _plane.InitializeContent();
            _plane.SetView();
            _plane.Draw();

            _sphere1.SetCamera(position);
            _sphere1.InitializeContent();
            _sphere1.SetView();
            _sphere1.Draw();

            _sphere2.SetCamera(position);
            _sphere2.InitializeContent();
            _sphere2.SetView();
            _sphere2.Draw();

            _ship.SetCamera(position);
            _ship.InitializeContent();
            _ship.SetView();
            _ship.Draw();
        }

        public void Dispose()
        {
            _ground.Dispose();
            _plane.Dispose();
            _sphere1.Dispose();
            _sphere2.Dispose();
            _ship.Dispose();
        }
    }
}
