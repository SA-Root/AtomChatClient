using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ModernChatLite
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        uint InputBoxWidth = 280;
        uint ButtonWidth = 160;

        public LoginPage()
        {
            this.InitializeComponent();
            //MainFrameController.SetTitleBar(AppTitleBar);
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrameController.NavigateToPage(typeof(RegisterPage));
        }

        private async void LoginButtonX_Click(object sender, RoutedEventArgs e)
        {
            var res = await MainFrameController.NService.SendLoginRequestAsync(new LoginRequest
            {
                UserName = UserNameBox.Text,
                Password = PwdBox.Password,
                IsLoginFree = false
            });
            if (res != null)
            {
                CurrentUserDatas.currentUserInfo.UID = uint.Parse(res.UID);
                CurrentUserDatas.Token= res.Token;
                MainFrameController.NavigateToPage(typeof(MainPage));
            }
            else
            {
                UserNameBox.Text = null;
                PwdBox.Password = null;
                ContentDialog dialog = new()
                {
                    // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
                    XamlRoot = this.XamlRoot,
                    Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                    Title = "Login Failed",
                    PrimaryButtonText = "OK",
                    DefaultButton = ContentDialogButton.Primary,
                    RequestedTheme = MainFrameController.topFrame.RequestedTheme
                };

                await dialog.ShowAsync();
            }
        }

        private async void WebClientButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new()
            {
                // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
                XamlRoot = this.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = "Switch to Web Client?",
                PrimaryButtonText = "Now",
                CloseButtonText = "Later",
                DefaultButton = ContentDialogButton.Primary,
                RequestedTheme = MainFrameController.topFrame.RequestedTheme
            };

            var result = await dialog.ShowAsync();
            if (result is ContentDialogResult.Primary)
            {
                MainFrameController.NavigateToWebPage("https://microsoft.github.io/microsoft-ui-xaml/");
            }
        }
    }
}
