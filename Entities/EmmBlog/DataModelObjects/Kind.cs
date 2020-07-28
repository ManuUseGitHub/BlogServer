using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.EmmBlog.DataModelObjects
{
    public class Kind
    {
        public enum NotificationKind
        {
            System = 1,
            BLog = 2,
            Social = 3,
            Messenger = 4
        }

        public int? KindId { get; set; }

        public string label { get; set; }
    }
}
