using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Hitokoto.Helpers;
using Hitokoto.Conroller;

namespace Hitokoto
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Timer RefreshTimer = null;
        private int RefreshTime = 5000;
        public MainWindow()
        {
            InitializeComponent();
            ShowDesktop.AddHook(this);
            RefreshTimer = new Timer((o) =>
            {
                ReflushHitokoto();
            }, null, 0, RefreshTime);
        }

        private static void CreateNotifyIcon()
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
            string hitokoto = HitokotoController.GetOneSentence();
            try
            {
                var json = JsonSerializer.Deserialize<HitokotoEntity.Sentence>(hitokoto);
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
        }

        private void MainForm_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            this.DragMove();
        }

        private static void SetBottom(Window window)
        {
            IntPtr hWnd = new WindowInteropHelper(window).Handle;
            NativeMethods.SetWindowPos(hWnd, NativeMethods.HWND_BOTTOM, 0, 0, 0, 0, NativeMethods.SWP_NOSIZE | NativeMethods.SWP_NOMOVE | NativeMethods.SWP_NOACTIVATE);
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            SetBottom(this);
        }

        private void MainForm_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            string hitokoto = HitokotoController.GetOneSentence();
            try
            {
                var json = JsonSerializer.Deserialize<HitokotoEntity.Sentence>(hitokoto);
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
            WindowInteropHelper wndHelper = new(this);
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
