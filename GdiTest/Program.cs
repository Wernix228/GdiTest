using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GdiTest {
    static class Program {

        private static bool _running  = true;

        [STAThread]
        static void Main() {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var form = new Form1();
            form.FormClosed += Form_FormClosed;
            form.Show();

            Renderer renderer = new Renderer(form.Handle);
            var image = renderer.BackBuffer;

            Drawer drawer = new Drawer(image,Color32.White);

            while (_running) {
                //Process window messages
                Application.DoEvents();

                //Render image
                image.FillColor(Color32.Black);

                drawer.fillTriangle(10, 10, 30, 50, 60, 5);

                //Show image in window
                renderer.Present();
            }
        }

        private static void Form_FormClosed(object sender, FormClosedEventArgs e) {
            _running = false;
        }
    }
}
