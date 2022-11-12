using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ModernChatLite
{
    internal class ChatContent : INotifyPropertyChanged
    {
        public DateTime Timestamp;
        public string Sender;
        private string _content;
        public string Content
        {
            get
            {
                if (isRevoked) return "[Message revoked]";
                else return _content ?? "[Empty message]";
            }
            set
            {
                if (!isRevoked) _content = value;
                OnPropertyChanged();
            }
        }
        public HorizontalAlignment MsgAlignment = HorizontalAlignment.Left;
        public int AvatarColumn = 0;
        public int ContentColumn = 1;
        public bool isRevoked = false;
        public bool isImage = false;
        public bool isFile = false;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
