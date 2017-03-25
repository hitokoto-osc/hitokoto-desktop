using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Forms;
using System.Drawing;

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
        [DllImport("user32.dll")]
        internal static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpWindowClass, string lpWindowName);
        public MainWindow()
        {
            InitializeComponent();
            ShowDesktop.AddHook(this);

            //nIcon.ShowBalloonTip(5000, "测试", "手动更换了一条一言", ToolTipIcon.Info);

            Task TaskReflushHitokoto = Task.Factory.StartNew(() =>
            {
                ReflushHitokoto();
            });
            //label1.Content = "test";
        }

        private void CreateNotifyIcon()
        {
            //NotifyIcon nIcon = new NotifyIcon();
            //nIcon.Icon = new Icon(@"hitokoto.ico");
            //nIcon.Visible = true;
            ////设置菜单项  
            //MenuItem setting1 = new MenuItem("setting1");
            //MenuItem setting2 = new MenuItem("setting2");
            //MenuItem setting = new MenuItem("setting", new MenuItem[] { setting1, setting2 });

            ////帮助选项  
            //MenuItem help = new MenuItem("help");

            ////关于选项  
            //MenuItem about = new MenuItem("about");

            ////退出菜单项  
            //MenuItem exit = new MenuItem("exit");
            //exit.Click += new EventHandler(exit_Click);

            ////关联托盘控件  
            //MenuItem[] childen = new MenuItem[] { setting, help, about, exit };
            //notifyIcon.ContextMenu = new ContextMenu(childen);

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
            e.Handled = true;
            this.DragMove();
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

        private void MainForm_Loaded(object sender, RoutedEventArgs e)
        {
            /*设置窗口为ToolWindow 用于隐藏ALT+TAB内显示*/
            WindowInteropHelper wndHelper = new WindowInteropHelper(this);
            int exStyle = (int)SetWindowStyle.GetWindowLong(wndHelper.Handle, (int)SetWindowStyle.GetWindowLongFields.GWL_EXSTYLE);
            exStyle |= (int)SetWindowStyle.ExtendedWindowStyles.WS_EX_TOOLWINDOW;
            SetWindowStyle.SetWindowLong(wndHelper.Handle, (int)SetWindowStyle.GetWindowLongFields.GWL_EXSTYLE, (IntPtr)exStyle);


        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItemSetting_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
