using Contracts;
using Entities;
using Entities.EmmBlog.DataModelObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class ConnexionRepository : RepositoryBase<Connexion>, IConnexionRepository
    {
        public ConnexionRepository(
            RepositoryWrapper repositoryWrapper,
            RepositoryContext repositoryContext)
            : base(repositoryWrapper, repositoryContext)
        { }

        public Connexion CreateConnexion(Connexion connexion)
        {
            connexion.Id = Guid.NewGuid();
            connexion.Valid = true;

            Create(connexion);
            TrySave();

            return GetConnexion(connexion.Id, connexion.AccountId);
        }

        public ICollection<Connexion> DiscardConnexions(Guid? accountId)
        {
            ICollection<Connexion> discarded = FindByCondition(c => !c.Valid).ToList();

            foreach (Connexion disc in discarded)
            {
                Delete(disc);
            }

            TrySave();

            return discarded;
        }

        public ICollection<Connexion> GetAccountConnexions(Guid? accountId)
        {
            return RepositoryContext.Connexions
                .Where(c => c.AccountId.Equals(accountId))
                .ToList();
        }

        public Connexion GetConnexion(Guid connexionId, Guid? accountId)
        {
            return FindByCondition(c =>
                c.Id.Equals(connexionId) &&
                c.AccountId.Equals(accountId)
            ).FirstOrDefault();
        }

        public int GetConnexionCount(Guid? accountId)
        {
            return GetAccountConnexions(accountId).Count();
        }

        public void InvalidateConnexion(Guid id, Guid? accountId)
        {
            GetConnexion(id, accountId).Valid = false;

            TrySave();
        }
    }
}