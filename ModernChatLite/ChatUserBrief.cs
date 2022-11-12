using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernChatLite
{
    internal class ChatWithUser
    {
        public string UserName;
        public string NickName;
        public uint UID;
        public string LastMsgTime;
        public ObservableCollection<ChatContent> chats;
    }
}
