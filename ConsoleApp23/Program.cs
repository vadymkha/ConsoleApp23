using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
namespace ConsoleApp23
{
    class Program
    {
        static void Main(string[] args)
        {
            
            SnapShot sh = new SnapShot();
            sh.ev += OnClick;
            Console.ReadKey();
        }
       public static void OnClick(object sender, EventArgs e)
        { }
    }
    public class SnapShot
    {
        UdpClient client;
        IPEndPoint endp;
        public event EventHandler ev;
        public SnapShot()
        {
            TimerCallback tm = new TimerCallback(timerOn);
            System.Threading.Timer timer = new System.Threading.Timer(tm, this, 0, 5000);
        }
        public void timerOn(object obj)
        {
            client = new UdpClient();
            endp = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1024);
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                }
                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, ImageFormat.Gif);
                byte[] bytes = ms.ToArray();
                client.Send(bytes,  bytes.Length, endp);
                bitmap.Save("test" + DateTime.Now.Second.ToString() + ".jpg", ImageFormat.Jpeg);
            }
            ev(this, new EventArgs());
            client.Close();
        }
    }
}
