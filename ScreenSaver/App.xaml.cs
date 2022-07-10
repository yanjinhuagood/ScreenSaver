using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using Application = System.Windows.Application;

namespace ScreenSaver
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private HwndSource winWPFContent;
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length == 0 || e.Args[0].ToLower().StartsWith("/s"))
            {
                foreach (Screen s in Screen.AllScreens)
                {
                    var window = new MainWindow();
                    window.Left = s.WorkingArea.Left;
                    window.Top = s.WorkingArea.Top;
                    window.Width = s.WorkingArea.Width;
                    window.Height = s.WorkingArea.Height;
                    window.Show();
                }
            }
            else if (e.Args[0].ToLower().StartsWith("/p"))
            {
                var window = new MainWindow();
                var previewHandle = Convert.ToInt32(e.Args[1]);
                var pPreviewHnd = new IntPtr(previewHandle);
                var lpRect = new RECT();
                var bGetRect = Win32API.GetClientRect(pPreviewHnd, ref lpRect);
                var sourceParams = new HwndSourceParameters("sourceParams");
                sourceParams.PositionX = 0;
                sourceParams.PositionY = 0;
                sourceParams.Height = lpRect.Bottom - lpRect.Top;
                sourceParams.Width = lpRect.Right - lpRect.Left;
                sourceParams.ParentWindow = pPreviewHnd;
                sourceParams.WindowStyle = (int)(WindowStyles.WS_VISIBLE | WindowStyles.WS_CHILD | WindowStyles.WS_CLIPCHILDREN);
                winWPFContent = new HwndSource(sourceParams);
                winWPFContent.Disposed += (o, args) => window.Close();
                winWPFContent.RootVisual = window.MainGrid;
            }

        }
    }
}
