using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.EmmBlog.DataModelObjects
{
    public partial class State
    {
        public enum MessageState
        {
            Sending = 1,
            Sent = 2,
            Unsent = 3,
            Removed = 4
        }

        public int? StateId { get; set; }

        public string label { get; set; }
    }
}
