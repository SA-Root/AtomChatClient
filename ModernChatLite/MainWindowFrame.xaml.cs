using Microsoft.UI;
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
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ModernChatLite
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindowFrame : Page
    {
        public MainWindowFrame(Window mainWindow)
        {
            this.InitializeComponent();

            MainFrameController.contentFrame = contentFrame;
            //MainFrameController.NavigateToPage(typeof(LoginPage));
            MainFrameController.NavigateToWebPage("http://10.0.0.55");

            mainWindow.ExtendsContentIntoTitleBar = true;
            mainWindow.SetTitleBar(AppTitleBar);
            MainFrameController.mainWindow = mainWindow;
        }
    }
}
