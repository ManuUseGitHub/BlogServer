using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.EmmBlog.DataModelObjects
{
    public class Importance
    {
        public enum FollowImportance
        {
            Default = 1,
            First = 2,
            Hidden = 3
        }

        public int? ImportanceId { get; set; }

        public string label { get; set; }
    }
}
