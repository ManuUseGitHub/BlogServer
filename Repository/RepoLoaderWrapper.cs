using Contracts;
using Entities;
using System;

namespace Repository
{
    public class RepoLoaderWrapper : IRepoLoaderWrapper
    {
        private IArticleLoader _article;

        public IArticleLoader Article
        { get { return (_article = Initializer.initLoader(_article)); } }

        private IBlogLoader _blog;
        public IBlogLoader Blog
        { get { return (_blog = Initializer.initLoader(_blog)); } }

        private IAccountLoader _account;
        public IAccountLoader Account 
        { get { return (_account = Initializer.initLoader(_account)); } }

        private ISocialLoader _social;
        public ISocialLoader Social
        { get { return (_social = Initializer.initLoader(_social)); } }

        private WrappersInitializer Initializer { get; set; }

        public RepoLoaderWrapper(RepositoryContext repositoryContext)
        {
            Initializer = new WrappersInitializer(repositoryContext);
            Initializer.RLoadr = this;
        }
    }
}
