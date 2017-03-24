using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hitokoto
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {



        public const uint SWP_NOSIZE = 0x0001;
        public const uint SWP_NOMOVE = 0x0002;
        public const uint SWP_NOACTIVATE = 0x0010;
        public static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

        public MainWindow()
        {
            InitializeComponent();
            ShowDesktop.AddHook(this);

            Task TaskReflushHitokoto = Task.Factory.StartNew(() =>
            {
                ReflushHitokoto();
            });
            //label1.Content = "test";
        }

        private void ReflushHitokoto()
        {
            while (true)
            {
                string hitokoto = HitokotoMethods.Get();
                try
                {
                    var jser = new JavaScriptSerializer();
                    var json = jser.Deserialize<HitokotoEntity.Sentence>(hitokoto);
                    Dispatcher.BeginInvoke(new Action(delegate
                    {
                        textBlockSentence.Text = json.hitokoto;
                        labelFrom.Content = "—— " + json.from;
                    }));
                    
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                    Dispatcher.BeginInvoke(new Action(delegate
                    {
                        textBlockSentence.Text = "加载失败,请重试~";
                        labelFrom.Content = "加载失败";
                    }));

                }
                Thread.Sleep(15000);
            }

        }

        private void MainForm_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void MainForm_StateChanged(object sender, EventArgs e)
        {
            //SetBottom(this);
        }

        private void SetBottom(Window window)
        {
            IntPtr hWnd = new WindowInteropHelper(window).Handle;
            NativeMethods.SetWindowPos(hWnd, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            SetBottom(this);
        }

        private void MainForm_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            string hitokoto = HitokotoMethods.Get();
            try
            {
                var jser = new JavaScriptSerializer();
                var json = jser.Deserialize<HitokotoEntity.Sentence>(hitokoto);
                textBlockSentence.Text = json.hitokoto;
                labelFrom.Content = "—— " + json.from;
            }
            catch
            {
                textBlockSentence.Text = "加载失败,请重试~";
                labelFrom.Content = "加载失败";
            }
        }
    }
}
