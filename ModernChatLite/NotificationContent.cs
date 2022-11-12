using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernChatLite
{
    internal class NotificationContent
    {
        public uint NID;
        public uint UID;
        public string SenderName;
        public int NotificationType;
        public uint GID;
        public string GroupName;
        public string Message
        {
            get
            {
                return NotificationType switch
                {
                    //Friend request
                    0 => $"'{SenderName}({UID})' wants to be your friend",
                    //Group invitation
                    1 => $"'{SenderName}({UID})' invites you to join '{GroupName}({GID})'",
                    _ => "",
                };
            }
        }
    }
}
