using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.EmmBlog.DataModelObjects
{
    public partial class Status
    {
        public enum FollowStatus
        {
            Friend = 1,
            PendingAlpha = 2,
            Following = 3,
            PendingBeta = 4,
            Blocked = 99,
        }

        public int? StatusId { get; set; }

        public string label { get; set; }
    }
}
