using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ModernChatLite
{
    internal static class CurrentUserDatas
    {
        public static ObservableCollection<ChatWithUser> chatData;
        public static ObservableCollection<ContactInfo> contactInfos;
        public static ObservableCollection<GroupInfo> groupInfos;
        public static ObservableCollection<NotificationContent> Notifications;
        public static ContactInfo currentUserInfo;
        public static string Token;
        public static int ChatJumpIdx = -1;
        static CurrentUserDatas()
        {
            Reset();
        }
        static string LastUpdateTimeHelper(DateTime dt)
        {
            var cul = CultureInfo.CurrentCulture;
            var now = DateTime.Now;
            if (dt.Year == now.Year)
            {
                if (dt.Month == now.Month && dt.Day == now.Day)
                {
                    return dt.ToString("t", cul);
                }
                else
                {
                    return dt.ToString("M", cul);
                }
            }
            else
            {
                return dt.ToString("g", cul);
            }
        }
        static void ResetUserBriefsAndChatContents()
        {
            var cul = CultureInfo.CreateSpecificCulture("en-US");

            var tmpList = new List<ChatWithUser>();
            tmpList.Add(new ChatWithUser
            {
                UserName = "Aello1",
                NickName = "Nick1",
                UID = 1344250,
                chats = new ObservableCollection<ChatContent>()
            });
            tmpList.Add(new ChatWithUser
            {
                UserName = "Bello2",
                NickName = "Nick2",
                UID = 1344251,
                chats = new ObservableCollection<ChatContent>()
            });
            tmpList.Add(new ChatWithUser
            {
                UserName = "Cello3",
                NickName = "Nick3",
                UID = 1344252,
                chats = new ObservableCollection<ChatContent>()
            });

            //Aello1
            tmpList[0].chats.Add(new ChatContent
            {
                MsgAlignment = HorizontalAlignment.Left,
                Content = "hhhhhhhh hhhhhhhhhh hhhhhhhhhh \nhhhhhh hhhhhhhh",
                Timestamp = DateTime.Parse("01/10/2009 7:34 PM", cul),
                Sender = tmpList[0].UserName
            });
            tmpList[0].chats.Add(new ChatContent
            {
                MsgAlignment = HorizontalAlignment.Right,
                Content = "ahhhh 0",
                Timestamp = DateTime.Parse("9/1/2022 7:34 AM", cul),
                Sender = currentUserInfo.UserName,
                AvatarColumn = 1,
                ContentColumn = 0
            });
            tmpList[0].LastMsgTime = LastUpdateTimeHelper(tmpList[0].chats.Last().Timestamp);

            //Bello2
            tmpList[1].chats.Add(new ChatContent
            {
                MsgAlignment = HorizontalAlignment.Left,
                Content = "hhhhhhhh hhhhhhhhhh hhhhhhhhhh \nhhhhhh hhhhhhhh",
                Timestamp = DateTime.Parse("01/10/2009 7:34 PM", cul),
                Sender = tmpList[1].UserName
            });
            tmpList[1].chats.Add(new ChatContent
            {
                MsgAlignment = HorizontalAlignment.Right,
                Content = "ahhhh 0",
                Timestamp = DateTime.Now,
                Sender = currentUserInfo.UserName,
                AvatarColumn = 1,
                ContentColumn = 0
            });
            tmpList[1].LastMsgTime = LastUpdateTimeHelper(tmpList[1].chats.Last().Timestamp);

            //Cello3
            tmpList[2].chats.Add(new ChatContent
            {
                MsgAlignment = HorizontalAlignment.Left,
                Content = "hhhhhhhh hhhhhhhhhh hhhhhhhhhh \nhhhhhh hhhhhhhh",
                Timestamp = DateTime.Parse("01/10/2009 3:34:02 PM", cul),
                Sender = tmpList[2].UserName
            });
            tmpList[2].chats.Add(new ChatContent
            {
                MsgAlignment = HorizontalAlignment.Right,
                Content = "ahhhh 0",
                Timestamp = DateTime.Parse("01/9/2018 5:34:07 PM", cul),
                Sender = currentUserInfo.UserName,
                AvatarColumn = 1,
                ContentColumn = 0
            });
            tmpList[2].LastMsgTime = LastUpdateTimeHelper(tmpList[2].chats.Last().Timestamp);

            chatData = new ObservableCollection<ChatWithUser>(tmpList.OrderByDescending(msg => msg.chats.Last().Timestamp));
        }
        static void ResetContacts()
        {
            contactInfos = new ObservableCollection<ContactInfo>();
            contactInfos.Add(new ContactInfo
            {
                UserName = "Aello1",
                NickName = "nick1",
                BirthDate = new DateOnly(2000, 10, 1),
                UID = 1344250,
                WhatsUp = "Hello there!1"
            });
            contactInfos.Add(new ContactInfo
            {
                UserName = "Bello2",
                NickName = "nick2",
                BirthDate = new DateOnly(2001, 10, 1),
                UID = 1344251,
                WhatsUp = "Hello there!2"
            });
            contactInfos.Add(new ContactInfo
            {
                UserName = "Cello3",
                NickName = "nick3",
                BirthDate = new DateOnly(2002, 10, 1),
                UID = 1344252,
                WhatsUp = "Hello there!3"
            });
            contactInfos.Add(new ContactInfo
            {
                UserName = "Dello4",
                NickName = "nick4",
                BirthDate = new DateOnly(2003, 10, 1),
                UID = 1344253,
                WhatsUp = "Hello there!4"
            });
        }
        static void ResetCurrentUserInfo()
        {
            currentUserInfo = new ContactInfo
            {
                UserName = "Axolottie",
                NickName = "Axo",
                BirthDate = new DateOnly(1999, 10, 1),
                UID = 1277650,
                WhatsUp = "Hello there!0"
            };
        }
        static void ResetGroupInfo()
        {
            groupInfos = new ObservableCollection<GroupInfo>();
            groupInfos.Add(new GroupInfo
            {
                GID = 14577,
                GroupName = "mYgROUP",
                GroupNickName = "mickName",
                Owner = currentUserInfo,
                Admins = new List<ContactInfo>
                {
                    contactInfos[0],contactInfos[1]
                },
                AllMembers = new List<ContactInfo>
                {
                    contactInfos[2],contactInfos[3]
                },
                chats = chatData[1].chats
            });
        }
        static void ResetNotifications()
        {
            Notifications = new ObservableCollection<NotificationContent>
            {
                new NotificationContent
                {
                    SenderName="Aello",
                    UID=123455,
                    NotificationType=0,
                    NID=120
                },
                new NotificationContent
                {
                    SenderName="Bello",
                    UID=1234665,
                    NotificationType=1,
                    GID=198074461,
                    GroupName="HelloThere!",
                    NID=123
                }
            };
        }
        public static void Reset()
        {
            ResetCurrentUserInfo();
            ResetContacts();
            ResetUserBriefsAndChatContents();
            ResetGroupInfo();
            ResetNotifications();
        }
    }
}
