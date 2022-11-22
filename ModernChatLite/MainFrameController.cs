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
using Windows.Storage;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using System.Text.Json;

namespace ModernChatLite
{
    public class LoginRequest
    {
        [JsonPropertyName("username")]
        public string UserName { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [JsonPropertyName("loginFree")]
        public bool IsLoginFree { get; set; }
    }
    public class LoginResponse
    {
        [JsonPropertyName("userId")]
        public string UID { get; set; }
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
    internal class NetworkService
    {
        public SocketIO WSClient { get; set; }
        public HttpClient HClient { get; set; }
        private string WSServer { get; set; }
        private string HttpServer { get; set; }
        public NetworkService(string ws, string http)
        {
            WSServer = ws;
            HttpServer = http;
            InitHClient();
            InitWSClient();
        }
        public void InitHClient()
        {
            HClient = new HttpClient();
            HClient.BaseAddress = new Uri(HttpServer);
        }
        public void InitWSClient()
        {
            WSClient = new SocketIO(WSServer);
            WSClient.OnConnected += async (sender, e) =>
            {
                Debug.Print("[INFO]WSConnected");
            };
            WSClient.SetCallbacks();
            WSClient.ConnectAsync();
        }
        public async Task<LoginResponse> SendLoginRequestAsync(LoginRequest loginRequest)
        {
            var resp = await HClient.PostAsJsonAsync(new Uri(""), loginRequest);
            if (resp.IsSuccessStatusCode)
            {
                return await resp.Content.ReadFromJsonAsync<LoginResponse>();
            }
            else
            {
                Debug.Print("[ERROR]Login response error");
                return null;
            }
        }
    }
    static class SICExtension
    {
        public static void SetCallbacks(this SocketIO wsc)
        {
            wsc.On("event", response =>
            {
                var txt = response.GetValue<string>();

            });
        }
    }
    internal static class MainFrameController
    {
        public static Frame topFrame = null;
        public static Frame contentFrame = null;
        public static NavigationViewItem chatItem = null;
        public static NavigationView mainNavigationView = null;
        public static Window mainWindow = null;
        public static NetworkService NService = null;
        private static readonly string WSServer = "http://127.0.0.1:11000/";
        private static readonly string HttpServer = "http://localhost:9000/atomchat/";
        static MainFrameController()
        {
            NService = new NetworkService(WSServer, HttpServer);
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
