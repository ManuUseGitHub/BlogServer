using Contracts;
using Entities.EmmBlog.DataModelObjects;
using Utilities;
using System;
using static Utilities.Reflector;

namespace Repository
{
    public partial class ArticleRepository : EmmBlogRepositoryBase<Article>, IArticleRepository
    {
        public Article CreateArticle(Article article)
        {
            SuggestSlug(article);

            Reflector.Merge(article, new Article()
            {
                VisibilityId = PUBLIC,
                WriteDate = DateTime.Now,
                BlogId = article.Blog.Id
            }, MergeOptions.KEEP_TARGET);

            if (article.VDepth == null)
            {
                article.VDepth = 0;
            }

            ThrowOnExisting(article);

            /* detaching dependecies because otherwise, EF will try to figure
             * out what entity matches to the copy... but copy should concidered
             * not existing yet*/
            article.Shares = null;
            article.Reactions = null;

            Create(article);
            TrySave();

            Wrapper.Notificaton.CreateArticle(GetArticle(article));

            return GetArticle(article);
        }
    }
}