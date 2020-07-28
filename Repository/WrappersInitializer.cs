using Entities;
using Repository.Loaders;
using System;

namespace Repository
{
    internal class WrappersInitializer
    {
        private RepositoryContext _repoContext;

        public WrappersInitializer(RepositoryContext repoContext)
        {
            this._repoContext = repoContext;
        }

        public RepositoryWrapper RWrapr { private get; set; }
        public RepoLoaderWrapper RLoadr { private get; set; }

        public T initRepo<T>(T repo)
        {
            if (repo == null)
            {
                object newInstance;
                Type t = typeof(T);
                switch (t.Name)
                {
                    case "IAccountRepository": newInstance = new AccountRepository(RWrapr, _repoContext); break;
                    case "IArticleRepository": newInstance = new ArticleRepository(RWrapr, _repoContext); break;
                    case "ICredentialRepository": newInstance = new CredentialRepository(RWrapr, _repoContext); break;
                    case "IBlogRepository": newInstance = new BlogRepository(RWrapr, _repoContext); break;
                    case "ICommentRepository": newInstance = new CommentRepository(RWrapr, _repoContext); break;
                    case "IConnexionRepository": newInstance = new ConnexionRepository(RWrapr, _repoContext); break;
                    case "IReactRepository": newInstance = new ReactRepository(RWrapr, _repoContext); break;
                    case "IShareRepository": newInstance = new ShareRepository(RWrapr, _repoContext); break;
                    case "IFollowRepository": newInstance = new FollowRepository(RWrapr, _repoContext); break;
                    case "IMessageRepository": newInstance = new MessageRepository(RWrapr, _repoContext); break;
                    case "INotificationRepository": newInstance = new NotificationRepository(RWrapr, _repoContext); break;

                    default: throw new RopositoryWrapperUnlistedException(typeof(T));
                }
                return (T)newInstance;
            }

            return repo;
        }

        public T initLoader<T>(T repo)
        {
            if (repo == null)
            {
                object newInstance;
                Type t = typeof(T);
                switch (t.Name)
                {
                    case "IAccountLoader": newInstance = new AccountLoader(RLoadr, _repoContext); break;
                    case "IArticleLoader": newInstance = new ArticleLoader(RLoadr, _repoContext); break;
                    case "IBlogLoader": newInstance = new BlogLoader(RLoadr, _repoContext); break;

                    default: throw new RopoLoaderWrapperUnlistedException(typeof(T));
                }
                return (T)newInstance;
            }

            return repo;
        }

        [Serializable]
        private class RopositoryWrapperUnlistedException : InvalidOperationException
        {
            public RopositoryWrapperUnlistedException(Type t) : base($"The class {t.Name} is not listed as a repository")
            { }
        }

        [Serializable]
        private class RopoLoaderWrapperUnlistedException : InvalidOperationException
        {
            public RopoLoaderWrapperUnlistedException(Type t) : base($"The class {t.Name} is not listed as a repository loader")
            { }
        }
    }
}