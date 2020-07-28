using Contracts;
using Entities;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        #region repository list

        private IShareRepository _share;

        public IShareRepository Share
        { get { return (_share = Initializer.initRepo(_share)); } }

        private IAccountRepository _account;

        public IAccountRepository Account
        { get { return (_account = Initializer.initRepo(_account)); } }

        private IArticleRepository _article;

        public IArticleRepository Article
        { get { return (_article = Initializer.initRepo(_article)); } }

        private ICredentialRepository _credential;

        public ICredentialRepository Credential
        { get { return (_credential = Initializer.initRepo(_credential)); } }

        private IBlogRepository _blog;

        public IBlogRepository Blog
        { get { return (_blog = Initializer.initRepo(_blog)); } }

        private ICommentRepository _comment;

        public ICommentRepository Comment
        { get { return (_comment = Initializer.initRepo(_comment)); } }

        private IConnexionRepository _connexion;

        public IConnexionRepository Connexion
        { get { return (_connexion = Initializer.initRepo(_connexion)); } }

        private IReactRepository _react;

        public IReactRepository React
        { get { return (_react = Initializer.initRepo(_react)); } }

        private IFollowRepository _follow;

        public IFollowRepository Follow
        { get { return (_follow = Initializer.initRepo(_follow)); } }

        private IMessageRepository _message;

        public IMessageRepository Message
        { get { return (_message = Initializer.initRepo(_message)); } }

        private INotificationRepository _notification;

        public INotificationRepository Notificaton
        { get { return (_notification = Initializer.initRepo(_notification)); } }

        #endregion repository list

        private RepositoryContext _repoContext;
        private WrappersInitializer Initializer { get; set; }

        public RepositoryWrapper(RepositoryContext repositoryContext)
        {
            _repoContext = repositoryContext;
            Initializer = new WrappersInitializer(_repoContext);
            Initializer.RWrapr = this;
        }

        public void Save()
        {
            _repoContext.SaveChanges();
        }
    }
}