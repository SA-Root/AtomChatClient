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
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ModernChatLite
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
            SetAppThemeSettings();
        }
        void SetAppThemeSettings()
        {
            switch (MainFrameController.topFrame.RequestedTheme)
            {
                case ElementTheme.Light:
                    AppThemeRadioButtons.SelectedIndex = 0;
                    AppThemeText.Text = "Light";
                    break;
                case ElementTheme.Dark:
                    AppThemeRadioButtons.SelectedIndex = 1;
                    AppThemeText.Text = "Dark";
                    break;
                default:
                    AppThemeRadioButtons.SelectedIndex = 2;
                    AppThemeText.Text = "Follow System";
                    break;
            }
        }
        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    base.OnNavigatedTo(e);
        //    SetAppThemeSettings();
        //}

        private void AppThemeRadioButtons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is RadioButtons rb)
            {
                var appTheme = rb.SelectedItem as string;
                AppThemeText.Text = appTheme;
                MainFrameController.SetTheme(appTheme);
            }
        }
    }
}
