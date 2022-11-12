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
    public sealed partial class GroupPage : Page
    {
        List<GroupInfo> searchCollection;
        GroupInfo currentGroup;
        public GroupPage()
        {
            this.InitializeComponent();
            GroupBriefList.ItemsSource = CurrentUserDatas.groupInfos;
            FillSearchCollection();
        }

        void FillSearchCollection()
        {
            searchCollection = new List<GroupInfo>
            {
                new GroupInfo
                {
                    GID=12334,
                    GroupName="Gello1"
                },
                new GroupInfo
                {
                    GID=12335,
                    GroupName="Gello2"
                },
                new GroupInfo
                {
                    GID=12336,
                    GroupName="Gello3"
                },
                new GroupInfo
                {
                    GID=12467,
                    GroupName="Gello4"
                }
            };
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SendFileButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SendPictureButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GroupBriefList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var i = GroupBriefList.SelectedIndex;
            if (i >= 0)
            {
                currentGroup = CurrentUserDatas.groupInfos[i];
                GroupChatContentList.ItemsSource = currentGroup.chats;
                CurrentGroupAvatar.DisplayName = currentGroup.GroupName;
                CurrentGroupName.Text = $"{currentGroup.GroupName}({currentGroup.GID})";
                EmptyChatTextBlock.Visibility = Visibility.Collapsed;
                GroupDetailGrid.Visibility = Visibility.Visible;
            }
            else
            {
                EmptyChatTextBlock.Visibility = Visibility.Visible;
                GroupDetailGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void CloseJoinGroupButton_Click(object sender, RoutedEventArgs e)
        {
            JoinGroupSmokeGrid.Visibility = Visibility.Collapsed;
            SearchResultListView.ItemsSource = null;
            QueryStringBox.Text = null;
            NoGroupFoundInfoBar.IsOpen = false;
        }

        private void QueryStringBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            NoGroupFoundInfoBar.IsOpen = false;
            if (QueryStringBox.Text is null || QueryStringBox.Text == "")
            {
                SearchGroupButton.IsEnabled = false;
                SearchResultListView.ItemsSource = null;
            }
            else
            {
                SearchGroupButton.IsEnabled = true;
            }
        }

        private void SearchGroupButton_Click(object sender, RoutedEventArgs e)
        {
            var query = QueryStringBox.Text;
            var pResult = uint.TryParse(query, out var GID);
            if (pResult == false)
            {
                GID = 0;
            }

            IEnumerable<GroupInfo> results =
                from _group in searchCollection
                where _group.GroupName.Contains(query) || _group.GID == GID
                select _group;

            if (results.Count() == 0)
            {
                NoGroupFoundInfoBar.IsOpen = true;
            }
            else
            {
                SearchResultListView.ItemsSource = new ObservableCollection<GroupInfo>(results);
            }
        }

        private async void SendRequestButton_Click(object sender, RoutedEventArgs e)
        {
            var Tag = (sender as Button).Tag as string;

            ContentDialog dialog = new()
            {
                XamlRoot = this.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = $"Request sent to GID: {Tag}",
                PrimaryButtonText = "OK",
                DefaultButton = ContentDialogButton.Primary,
                RequestedTheme = MainFrameController.topFrame.RequestedTheme
            };
            await dialog.ShowAsync();
        }

        private void JoinGroupButton_Click(object sender, RoutedEventArgs e)
        {
            JoinGroupSmokeGrid.Visibility = Visibility.Visible;
        }

        private void CreateGroupButton_Click(object sender, RoutedEventArgs e)
        {
            InviteFriendsListView.ItemsSource = CurrentUserDatas.contactInfos;
            CreateGroupSmokeGrid.Visibility = Visibility.Visible;
        }

        private void ManageGroupButton_Click(object sender, RoutedEventArgs e)
        {
            GroupMembersCVS.Source = new ObservableCollection<GroupInfoDict>
            {
                new GroupInfoDict("Owner",
                    new List<ContactInfo>
                    {
                        currentGroup.Owner
                    }),
                new GroupInfoDict("Admin",currentGroup.Admins),
                new GroupInfoDict("Member",currentGroup.AllMembers)
            };
            MgmtCurrentGroupAvatar.DisplayName = currentGroup.GroupName;
            MgmtCurrentGroupGIDBlock.Text = currentGroup.GID.ToString();
            MgmtCurrentGroupNameBlock.Text = currentGroup.GroupName;

            ManageGroupSmokeGrid.Visibility = Visibility.Visible;
        }

        private void RevokeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CloseCreateGroupButton_Click(object sender, RoutedEventArgs e)
        {
            NewGroupNameBox.Text = "";
            InviteFriendsListView.SelectedItems.Clear();
            CreateGroupSmokeGrid.Visibility = Visibility.Collapsed;
        }

        private async void ConfirmCreateGroupButton_Click(object sender, RoutedEventArgs e)
        {
            ConfirmCreateGroupButton.IsEnabled = false;
            CreateGroupProgressRing.Visibility = Visibility.Visible;

            var tmp = new GroupInfo
            {
                GroupName = NewGroupNameBox.Text,
                GID = 12777,
                Owner = CurrentUserDatas.currentUserInfo,
                Admins = new List<ContactInfo>(),
                AllMembers = new List<ContactInfo>(),
                chats = new ObservableCollection<ChatContent>()
            };
            foreach (ContactInfo i in InviteFriendsListView.SelectedItems)
            {
                tmp.AllMembers.Add(i);
            }
            CurrentUserDatas.groupInfos.Add(tmp);
            await Task.Delay(500);

            ContentDialog dialog = new()
            {
                XamlRoot = this.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = $"Your new group has created.",
                PrimaryButtonText = "OK",
                DefaultButton = ContentDialogButton.Primary,
                RequestedTheme = MainFrameController.topFrame.RequestedTheme
            };
            var res = dialog.ShowAsync();

            CreateGroupProgressRing.Visibility = Visibility.Collapsed;
            ConfirmCreateGroupButton.IsEnabled = true;
            CloseCreateGroupButton_Click(null, null);

            await res;
        }

        private void ClearInvitationButton_Click(object sender, RoutedEventArgs e)
        {
            InviteFriendsListView.SelectedItems.Clear();
        }

        private void ChatWithGroupMemberButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CloseManageGroupButton_Click(object sender, RoutedEventArgs e)
        {
            ManageGroupSmokeGrid.Visibility = Visibility.Collapsed;
        }
    }
}
