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
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ModernChatLite
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChatPage : Page
    {
        public ChatPage()
        {
            this.InitializeComponent();
            ChatUserBriefList.ItemsSource = CurrentUserDatas.chatData;
            if (CurrentUserDatas.ChatJumpIdx >= 0)
            {
                ChatUserBriefList.SelectedIndex = CurrentUserDatas.ChatJumpIdx;
            }
        }

        private void ChatUserBriefList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var i = ChatUserBriefList.SelectedIndex;
            if (i >= 0)
            {
                ChatContentList.ItemsSource = CurrentUserDatas.chatData[i].chats;
                CurrentChatTargetAvatar.DisplayName = CurrentUserDatas.chatData[i].UserName;
                CurrentChatTargetName.Text = $"{CurrentUserDatas.chatData[i].NickName}({CurrentUserDatas.chatData[i].UserName})";
                EmptyChatTextBlock.Visibility = Visibility.Collapsed;
                ChatDetailGrid.Visibility = Visibility.Visible;
            }
            else
            {
                EmptyChatTextBlock.Visibility = Visibility.Visible;
                ChatDetailGrid.Visibility = Visibility.Collapsed;
            }
        }
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            var i = ChatUserBriefList.SelectedIndex;
            CurrentUserDatas.chatData[i].chats.Add(new ChatContent
            {
                MsgAlignment = HorizontalAlignment.Right,
                Content = MsgContentBox.Text,
                Timestamp = DateTime.Now,
                Sender = CurrentUserDatas.currentUserInfo.UserName,
                AvatarColumn = 1,
                ContentColumn = 0
            });
            MsgContentBox.Text = null;
        }

        private void SendPictureButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SendFileButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RevokeButton_Click(object sender, RoutedEventArgs e)
        {
            RevokeButton.IsEnabled = false;
            var i = ChatContentList.SelectedItem as ChatContent;
            i.isRevoked = true;
            i.Content = "[hhh]";
        }

        private void ChatContentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var i = ChatContentList.SelectedIndex;
            if (i >= 0)
            {
                var t = ChatContentList.SelectedItem as ChatContent;
                if (t.Sender == CurrentUserDatas.currentUserInfo.UserName
                        && t.isRevoked == false)
                {
                    RevokeButton.IsEnabled = true;
                }
                else
                {
                    RevokeButton.IsEnabled = false;
                }
            }
            else
            {
                RevokeButton.IsEnabled = false;
            }
        }
    }
}
