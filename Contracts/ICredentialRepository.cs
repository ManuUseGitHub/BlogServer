using Entities.EmmBlog.DataModelObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface ICredentialRepository : IRepositoryBase<Credential>
    {

        void CreateAccount(Credential credential);

        void DeleteCredentialAndAccount(Credential credential);

        Account GetAccountWithCredential(Guid? accountId);
        Account GetAccountWithCredential(Credential found);
        Connexion ConnectUser(string mailAddress, string passworldSHA512);
        Account GetAccountByMailAddress(string mailAddress);
    }
}
