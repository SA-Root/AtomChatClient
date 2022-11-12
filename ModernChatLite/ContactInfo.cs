using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernChatLite
{
    internal class ContactInfo
    {
        public string UserName;
        public string NickName;
        public string NickName_UserName
        {
            get => NickName + "(" + UserName + ")";
        }
        public uint UID;
        public DateOnly BirthDate;
        public string WhatsUp;
    }
}
