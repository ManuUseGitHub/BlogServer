using Entities.EmmBlog.DataModelObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface ISubscribeRepository
    {
        public Subscribe GetBlogSubscription(Guid id);
        
    }
}
