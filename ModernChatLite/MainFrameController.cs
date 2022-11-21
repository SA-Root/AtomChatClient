using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Diagnostics;
using SocketIOClient;
using SocketIOClient.Transport;

namespace ModernChatLite
{
    internal static class MainFrameController
    {
        public static Frame topFrame = null;
        public static Frame contentFrame = null;
        public static NavigationViewItem chatItem = null;
        public static NavigationView mainNavigationView = null;
        public static Window mainWindow = null;
        public static SocketIO WSClient = null;
        private static readonly string WSServer = "http://127.0.0.1:11000/";
        public static void InitWSClient()
        {
            if (WSClient == null)
            {
                WSClient = new SocketIO(WSServer);
                WSClient.OnConnected += async (sender, e) =>
                {
                    Debug.Print("[INFO]WSConnected");
                };
            }
            else if (!WSClient.Connected)
            {
                WSClient.ConnectAsync();
            }
        }
        public static void SetTitleBar(UIElement uie)
        {
            mainWindow?.SetTitleBar(uie);
        }
        public static void SetTheme(string theme)
        {
            switch (theme)
            {
                case "Light":
                    topFrame.RequestedTheme = ElementTheme.Light;
                    break;
                case "Dark":
                    topFrame.RequestedTheme = ElementTheme.Dark;
                    break;
                case "Follow System":
                    topFrame.RequestedTheme = ElementTheme.Default;
                    break;
            }
        }
        public static void NavigateToPage(Type pageType)
        {
            contentFrame?.Navigate(pageType);
        }
        public static void NavigateToWebPage(string uri)
        {
            contentFrame.Content = new WebView2Page(uri);
        }
    }
}
