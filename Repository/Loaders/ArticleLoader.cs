using Contracts;
using Entities;
using Entities.EmmBlog.DataModelObjects;
using Entities.EmmBlog.DataModelObjects.Utilities;
using System.Linq;

namespace Repository.Loaders
{
    public class ArticleLoader : RepoLoaderBase<Article>, IArticleLoader
    {   
        public ArticleLoader(

            IRepoLoaderWrapper loader,
            RepositoryContext repositoryContext

        ) : base(loader, repositoryContext)
        {
        }

        public void AssignAllAnswers(Comment comment)
        {
            LoadCollection((c => c.Answers), comment);

            foreach (var answer in comment.Answers)
            {
                LoadReference(a => a.Account, answer);
            }

            comment.Answers = DataPresenter
                .GetSortedByWriteDate(comment.Answers)
                .ToList();

            LoadCollection(c => c.Reactions, comment);
        }

        public void LoadAccount(Article article)
        {
            LoadReference(b => b.Account, article.Blog);
        }

        public void LoadBlogOfArticle(Article article)
        {
            LoadReference(a => a.Blog, article);
        }
    }
}
