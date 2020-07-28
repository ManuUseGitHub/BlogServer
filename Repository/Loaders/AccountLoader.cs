using Contracts;
using Entities;
using Entities.EmmBlog.DataModelObjects;
using Entities.EmmBlog.DataModelObjects.dependentInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Loaders
{
    class AccountLoader: RepoLoaderBase<Account>, IAccountLoader
    {   
        public AccountLoader(

            IRepoLoaderWrapper loader,
            RepositoryContext repositoryContext

        ) : base(loader, repositoryContext)
    {
    }

        public void LoadAccount(IHaveAccount havingAccount)
        {
            LoadReference(h => h.Account, havingAccount);
        }

        public void LoadCredentials(Account account)
        {
            LoadReference(a => a.Credential, account);
        }

    }
}
