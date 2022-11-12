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

        private void LoginButtonX_Click(object sender, RoutedEventArgs e)
        {
            MainFrameController.NavigateToPage(typeof(MainPage));
        }

        private async void WebClientButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Switch to Web Client?";
            dialog.PrimaryButtonText = "Now";
            dialog.CloseButtonText = "Later";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.RequestedTheme = MainFrameController.topFrame.RequestedTheme;

            var result = await dialog.ShowAsync();
            if (result is ContentDialogResult.Primary)
            {
                MainFrameController.NavigateToWebPage("https://microsoft.github.io/microsoft-ui-xaml/");
            }
        }
    }
}
