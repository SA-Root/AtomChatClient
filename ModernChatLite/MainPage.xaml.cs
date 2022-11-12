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
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Dispatching;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ModernChatLite
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private void DelayedInit()
        {
            LoadingSmokeGrid.Visibility = Visibility.Visible;
            Task.Run(() =>
            {
                Thread.Sleep(500);
                bool isQueued = DispatcherQueue.TryEnqueue(
                    DispatcherQueuePriority.Normal,
                    () => InitUIFields());
            });
        }
        private void InitUIFields()
        {
            CurrentUserButton.Content = null;

            MainNavigationView.SelectedItem = MainNavigationView.MenuItems[2];
            CurrentUserNameBlock.Text = CurrentUserDatas.currentUserInfo.UserName;
            CurrentAvatar.DisplayName = CurrentUserDatas.currentUserInfo.UserName;
            AvatarPicture.DisplayName = CurrentUserDatas.currentUserInfo.UserName;
            MyNameBlock.Text = CurrentUserDatas.currentUserInfo.UserName;
            UIDBlock.Text = CurrentUserDatas.currentUserInfo.UID.ToString();
            BirthdayBlock.Text = CurrentUserDatas.currentUserInfo.BirthDate.ToString();
            WhatsUpBlock.Text = CurrentUserDatas.currentUserInfo.WhatsUp;

            LoadingSmokeGrid.Visibility = Visibility.Collapsed;
        }
        public MainPage()
        {
            this.InitializeComponent();
            //MainFrameController.mainWindow.SetTitleBar(MainNavigationView);
            MainFrameController.mainNavigationView = MainNavigationView;
            MainFrameController.chatItem = ChatItem;
            DelayedInit();
        }

        private void MainNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                contentFrame.Navigate(typeof(SettingsPage));
            }
            else
            {
                var navItem = (args.SelectedItem as NavigationViewItem).Tag as string;
                switch (navItem)
                {
                    case "Notification":
                        contentFrame.Navigate(typeof(NotificationPage));
                        break;
                    case "Contact":
                        contentFrame.Navigate(typeof(ContactPage));
                        break;
                    case "Chat":
                        contentFrame.Navigate(typeof(ChatPage));
                        break;
                    case "Group":
                        contentFrame.Navigate(typeof(GroupPage));
                        break;
                    default:
                        break;
                }
            }
        }

        private void SignoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrameController.NavigateToPage(typeof(LoginPage));
        }

        private void MyInfoButton_Click(object sender, RoutedEventArgs e)
        {
            SmokeGrid.Visibility = Visibility.Visible;
        }

        private void EditMyInfoButton_Click(object sender, RoutedEventArgs e)
        {
            MyNameBlock.Visibility = Visibility.Collapsed;
            MyNameEditBox.Text = MyNameBlock.Text;
            MyNameEditBox.Visibility = Visibility.Visible;

            BirthdayBlock.Visibility = Visibility.Collapsed;
            BirthdayPicker.Date = new DateTimeOffset(CurrentUserDatas.currentUserInfo.BirthDate.ToDateTime(TimeOnly.Parse("0:00 AM")));
            BirthdayPicker.Visibility = Visibility.Visible;

            WhatsUpBlock.Visibility = Visibility.Collapsed;
            WhatsUpEditBox.Text = WhatsUpBlock.Text;
            WhatsUpEditBox.Visibility = Visibility.Visible;

            EditMyInfoButton.Visibility = Visibility.Collapsed;
            SaveMyInfoButton.Visibility = Visibility.Visible;
        }

        private void MainNavigationView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width < 960)
            {
                MainNavigationView.PaneDisplayMode = NavigationViewPaneDisplayMode.LeftCompact;
                MainNavigationViewCustomGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                MainNavigationView.PaneDisplayMode = NavigationViewPaneDisplayMode.Left;
                MainNavigationViewCustomGrid.Visibility = Visibility.Visible;
            }
        }

        private void CloseMyInfoButton_Click(object sender, RoutedEventArgs e)
        {
            SmokeGrid.Visibility = Visibility.Collapsed;
        }

        private void SaveMyInfoButton_Click(object sender, RoutedEventArgs e)
        {
            MyNameBlock.Text = MyNameEditBox.Text;
            BirthdayBlock.Text = DateOnly.FromDateTime(BirthdayPicker.Date.DateTime).ToString();
            WhatsUpBlock.Text = WhatsUpEditBox.Text;

            MyNameEditBox.Visibility = Visibility.Collapsed;
            MyNameBlock.Visibility = Visibility.Visible;

            BirthdayPicker.Visibility = Visibility.Collapsed;
            BirthdayBlock.Visibility = Visibility.Visible;

            WhatsUpEditBox.Visibility = Visibility.Collapsed;
            WhatsUpBlock.Visibility = Visibility.Visible;

            SaveMyInfoButton.Visibility = Visibility.Collapsed;
            EditMyInfoButton.Visibility = Visibility.Visible;
        }

        private void MainNavigationView_PaneOpened(NavigationView sender, object args)
        {
            MainNavigationViewCustomGrid.Visibility = Visibility.Visible;
        }

        private void MainNavigationView_PaneClosed(NavigationView sender, object args)
        {
            MainNavigationViewCustomGrid.Visibility = Visibility.Collapsed;
        }

        private void CurrentUserButton_Click(object sender, RoutedEventArgs e)
        {
            MyInfoFlyout.ShowAt(CurrentUserButton,
                new Point(CurrentUserButton.ActualWidth / 2,
                CurrentUserButton.ActualHeight / 2));
        }
    }
}
