using Entities.EmmBlog.DataModelObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IShareRepository : IRepositoryBase<Share>
    {
        Share ShareArticle(Article article, Blog blog2);
        Share getShare(Share shared);
    }
}
