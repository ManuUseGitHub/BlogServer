using Contracts;
using Entities;
using Entities.EmmBlog.DataModelObjects;

namespace Repository.Loaders
{
    public class BlogLoader : RepoLoaderBase<Blog>, IBlogLoader
    {
        public BlogLoader(
            
            IRepoLoaderWrapper loader, 
            RepositoryContext repositoryContext

        ) : base(loader, repositoryContext) {
        }

        public void LoadBlogAccount(Blog blog)
        {
            LoadReference(b => b.Account, blog);
        }

        public void LoadBlogArticles(Blog blog)
        {
            LoadCollection(b => b.Articles, blog);
            foreach (Article article in blog.Articles)
            {
                LoadReference(a => a.Blog, article);
                LoadReference(b => b.Account, article.Blog);
            }
        }

        public void LoadBlogs(Blog blog)
        {
            LoadCollection(b => b.Articles, blog);
        }

        public void LoadBlogSubscriptions(Blog blog)
        {
            LoadCollection(b => b.Subscribe, blog);
        }

        public void LoadBlogVisibility(Blog blog)
        {
            LoadReference(b => b.Visibility, blog);
        }
    }
}
