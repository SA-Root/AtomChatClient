using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Dispatching;
using System.Threading;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ModernChatLite
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContactPage : Page
    {
        List<ContactInfo> searchCollection;
        public ContactPage()
        {
            this.InitializeComponent();
            CurrentUserDatas.ChatJumpIdx = -1;
            ContactBriefList.ItemsSource = CurrentUserDatas.contactInfos;
            FillSearchCollection();
        }

        void FillSearchCollection()
        {
            searchCollection = new List<ContactInfo>
            {
                new ContactInfo
                {
                    UserName = "Aello1",
                    UID = 1344250
                },
                new ContactInfo
                {
                    UserName = "Bello2",
                    UID = 1344251
                },
                new ContactInfo
                {
                    UserName = "Cello3",
                    UID = 1344252
                },
                new ContactInfo
                {
                    UserName = "134425",
                    UID = 13425
                }
            };
        }

        private void ContactBriefList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var i = ContactBriefList.SelectedIndex;
            if (i >= 0)
            {
                EmptyContactTextBlock.Visibility = Visibility.Collapsed;
                ContactDetailGrid.Visibility = Visibility.Visible;
                SetCurrentContactInfo(CurrentUserDatas.contactInfos[i]);
            }
            else
            {
                EmptyContactTextBlock.Visibility = Visibility.Visible;
                ContactDetailGrid.Visibility = Visibility.Collapsed;
            }
        }
        private void SetCurrentContactInfo(ContactInfo ci)
        {
            AvatarPicture.DisplayName = ci.UserName;
            NickNameBlock.Text = ci.NickName;
            UserNameBlock.Text = ci.UserName;
            UIDBlock.Text = ci.UID.ToString();
            BirthdayBlock.Text = ci.BirthDate.ToString();
            WhatsUpBlock.Text = ci.WhatsUp;
        }

        private void AddContactButton_Click(object sender, RoutedEventArgs e)
        {
            AddContactSmokeGrid.Visibility = Visibility.Visible;
        }

        private void CloseAddContactButton_Click(object sender, RoutedEventArgs e)
        {
            AddContactSmokeGrid.Visibility = Visibility.Collapsed;
            SearchResultListView.ItemsSource = null;
            QueryStringBox.Text = null;
            NoUserFoundInfoBar.IsOpen = false;
        }

        private void SearchNewContactButton_Click(object sender, RoutedEventArgs e)
        {
            var query = QueryStringBox.Text;
            var pResult = uint.TryParse(query, out var UID);
            if (pResult == false)
            {
                UID = 0;
            }

            IEnumerable<ContactInfo> results =
                from user in searchCollection
                where user.UserName.Contains(query) || user.UID == UID
                select user;

            if (results.Count() == 0)
            {
                NoUserFoundInfoBar.IsOpen = true;
            }
            else
            {
                SearchResultListView.ItemsSource = new ObservableCollection<ContactInfo>(results);
            }
        }
        private async void SendRequestButton_Click(object sender, RoutedEventArgs e)
        {
            var Tag = (sender as Button).Tag as string;

            ContentDialog dialog = new()
            {
                XamlRoot = this.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = $"Request sent to UID: {Tag}",
                PrimaryButtonText = "OK",
                DefaultButton = ContentDialogButton.Primary,
                RequestedTheme = MainFrameController.topFrame.RequestedTheme
            };
            await dialog.ShowAsync();
        }

        private void QueryStringBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            NoUserFoundInfoBar.IsOpen = false;
            if (QueryStringBox.Text is null || QueryStringBox.Text == "")
            {
                SearchNewContactButton.IsEnabled = false;
                SearchResultListView.ItemsSource = null;
            }
            else
            {
                SearchNewContactButton.IsEnabled = true;
            }
        }

        private void StartChatButton_Click(object sender, RoutedEventArgs e)
        {
            var uid = UIDBlock.Text;
            var idx = -1;
            for (var i = 0; i < CurrentUserDatas.chatData.Count; i++)
            {
                if (CurrentUserDatas.chatData[i].UID.ToString() == uid)
                {
                    idx = i;
                    break;
                }
            }
            if (idx >= 0)
            {
                CurrentUserDatas.ChatJumpIdx = idx;
                MainFrameController.mainNavigationView.SelectedItem = MainFrameController.chatItem;
            }
            else
            {
                CurrentUserDatas.chatData.Add(new ChatWithUser
                {
                    UserName = UserNameBlock.Text,
                    NickName = NickNameBlock.Text,
                    LastMsgTime = "",
                    UID = uint.Parse(uid),
                    chats = new ObservableCollection<ChatContent>()
                });
                CurrentUserDatas.ChatJumpIdx = CurrentUserDatas.chatData.Count - 1;
                MainFrameController.mainNavigationView.SelectedItem = MainFrameController.chatItem;
            }
        }

        private async void DeleteContactButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new()
            {
                XamlRoot = this.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = $"Delete friend {UserNameBlock.Text}(UID: {UIDBlock.Text})?",
                PrimaryButtonText = "Yes",
                CloseButtonText = "No",
                DefaultButton = ContentDialogButton.Close,
                RequestedTheme = MainFrameController.topFrame.RequestedTheme
            };
            var res = await dialog.ShowAsync();

            if (res == ContentDialogResult.Primary)
            {
                var tUID = UIDBlock.Text;
                var delTask = Task.Run(() =>
                {
                    var idx = -1;
                    for (var i = 0; i < CurrentUserDatas.contactInfos.Count; i++)
                    {
                        if (CurrentUserDatas.contactInfos[i].UID.ToString() == tUID)
                        {
                            idx = i;
                            break;
                        }
                    }
                    DispatcherQueue.TryEnqueue(
                        DispatcherQueuePriority.Normal,
                        () => CurrentUserDatas.contactInfos.RemoveAt(idx));

                    var idx2 = -1;
                    for (var i = 0; i < CurrentUserDatas.chatData.Count; i++)
                    {
                        if (CurrentUserDatas.chatData[i].UID.ToString() == tUID)
                        {
                            idx2 = i;
                            break;
                        }
                    }
                    if (idx2 >= 0)
                    {
                        DispatcherQueue.TryEnqueue(
                            DispatcherQueuePriority.Normal,
                            () => CurrentUserDatas.chatData.RemoveAt(idx2));
                    }
                    Thread.Sleep(500);
                });

                ContactBriefList.SelectedItem = null;
                DeletingSmokeGrid.Visibility = Visibility.Visible;
                await delTask;
                DeletingSmokeGrid.Visibility = Visibility.Collapsed;
            }
        }
    }
}
