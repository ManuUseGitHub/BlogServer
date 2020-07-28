using Entities.EmmBlog.DataModelObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IConnexionRepository
    {
        ICollection<Connexion> GetAccountConnexions(Guid? accountId);

        Connexion GetConnexion(Guid connexionId, Guid? accountId);

        Connexion CreateConnexion(Connexion connexion);

        int GetConnexionCount(Guid? accountId);

        ICollection<Connexion> DiscardConnexions(Guid? accountId);
        void InvalidateConnexion(Guid id, Guid? accountId);
    }
}
