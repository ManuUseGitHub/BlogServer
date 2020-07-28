using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.EmmBlog.DataModelObjects
{
    public class ReactionCounter
    {
        public ReactionType RType { get; set; }
        public int Count { get; set; }
    }
}
