using System;
using System.Collections.Generic;
using System.Text;
using Entities.EmmBlog.DataTransferObjects.Out.Reduced;

namespace Entities.EmmBlog.DataTransferObjects
{
    public class BlogWithSubscription
    {
        public Guid Id { get; set; }
        public string BlogName { get; set; }

        public ICollection<SubscriptionOfBlogDTO> Subscribe { get; set; }
    }
}
