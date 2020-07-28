using Entities.EmmBlog.DataModelObjects;

namespace Contracts
{
    public interface IArticleLoader
    {
        void LoadBlogOfArticle(Article article);
        void LoadAccount(Article article);
    }
}