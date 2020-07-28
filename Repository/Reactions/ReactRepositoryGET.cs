using Contracts;
using Entities;
using Entities.EmmBlog.DataModelObjects;
using Entities.EmmBlog.DataModelObjects.dependentInterfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public partial class ReactRepository : EmmBlogRepositoryBase<React>, IReactRepository
    {
        public React GetReact<T>(T entity, Account account)
        {
            if (entity is Article)
            {
                return GetReactOnArticle(entity as Article, account);
            }
            else if (entity is Comment)
            {
                return GetReactOnComment(entity as Comment, account);
            }
            else return null;
        }

        private React GetReactOnArticle(Article entity, Account account)
        {
            return FindByCondition(r =>
                    r.Slug.Equals(entity.Slug) &&
                    r.BlogId.Equals(entity.BlogId) &&
                    r.AccountId.Equals(account.Id))
                .OrderByDescending(r => r.VDepth)
                .FirstOrDefault();
        }

        private React GetReactOnComment(Comment entity, Account account)
        {
            return FindByCondition(r =>
                    r.CommentId.Equals(entity.Id) &&
                    r.AccountId.Equals(account.Id))
                .OrderByDescending(r => r.RDepth)
                .FirstOrDefault();
        }
    }
}