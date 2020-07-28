using Entities.EmmBlog.DataModelObjects;
using Repository;
using System;

namespace Contracts
{
    public interface IRepositoryWrapper
    {
        ICredentialRepository Credential { get; }
        ICommentRepository Comment { get; }
        IArticleRepository Article { get; }
        IBlogRepository Blog { get; }
        IAccountRepository Account { get;}
        IReactRepository React { get; }
        IConnexionRepository Connexion { get; }
        IShareRepository Share { get; }
        IFollowRepository Follow { get; }
        INotificationRepository Notificaton { get; }

        void Save();
    }
}
