using System;
using System.Collections.Generic;
using System.Text;

namespace Ara.Domain.Common
{
    public class Message
    {
        public string Text { get; set; }
        public MessageType Type { get; set; }

        public enum MessageType
        {
            Note = 1,
            Notice,
            Info,
            Warning,
        }
    }


}
