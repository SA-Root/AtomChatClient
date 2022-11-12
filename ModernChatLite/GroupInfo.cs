using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernChatLite
{
    internal class GroupInfo
    {
        public uint GID;
        public string GroupName;
        public string GroupNickName;
        public ContactInfo Owner;
        public List<ContactInfo> Admins;
        public List<ContactInfo> AllMembers;
        public ObservableCollection<ChatContent> chats;
        public string CombinedName
        {
            get
            {
                if (GroupNickName is not null) return $"{GroupNickName}({GroupName})";
                else return GroupName;
            }
        }
    }
    internal class GroupInfoDict : List<object>
    {
        public GroupInfoDict(string key, IEnumerable<object> items) : base(items)
        {
            Key = key;
        }
        public object Key { get; set; }
    }
}
