using Contracts;
using Entities.EmmBlog.DataModelObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Repository
{
    public interface IBlogRepository : IRepositoryBase<Blog>
    {
        public Blog GetBlogById(Guid? id);

        public Blog GetBlogArticles(Guid? id);
        Blog CreateBlog(Blog blog);
        Subscribe CreateSubscription(Guid? accountId, Guid? blogId);
        Subscribe CreateSubscription(string mailAddress, Guid? blogId);
        Subscribe CreateSubscription(Account account, Guid? blogId);
        Blog GetBlogOfAccount(Guid? id);

        Blog GetEndBlogOfAccount(Guid? id, Guid? idViewer);
        Collection<Account> GetAllowedAccounts(Blog blog);
        Collection<Account> GetAllowedAccounts(Account account);
        void ChangeBlogInfos(Blog modifications);
        ICollection<Blog> GetBlogs();
    }
}
