using Contracts;
using Entities;
using Entities.EmmBlog.DataModelObjects;
using Repository.Exceptions;
using System;
using System.Linq;
using static Repository.Exceptions.AccountsExceptions;

namespace Repository
{
    public class CredentialRepository : RepositoryBase<Credential>, ICredentialRepository
    {
        public CredentialRepository(
            RepositoryWrapper repositoryWrapper,
            RepositoryContext repositoryContext)
            : base(repositoryWrapper, repositoryContext)
        { }

        public Connexion ConnectUser(string mailAddress, string passworldSHA512)
        {
            var found = RepositoryContext.Credentials
                .Where(cred => cred.MailAddress.Equals(mailAddress))
                .Where(cred => cred.PassWord.Equals(passworldSHA512))
                .FirstOrDefault();

            Account acc = GetAccountWithCredential(found);

            Connexion con = Wrapper.Connexion.CreateConnexion(new Connexion(){
                AccountId = acc.Id,
                WriteDate = DateTime.Now
            });

            return con;
        }

        public void CreateAccount(Credential credential)
        {
            Create(credential);
        }

        public void DeleteCredentialAndAccount(Credential credential)
        {
            Delete(credential);
        }

        public Account GetAccountByMailAddress(string mailAddress)
        {
            var found = FindByCondition(c => c.MailAddress.Equals(mailAddress)).FirstOrDefault();
            if(found == null)
            {
                throw DbEx.Instance.GetThrowable<NotFoundAccountException>();
            }
            return found.Account;
        }

        public Account GetAccountWithCredential(Guid? accountId)
        {
            var found = FindByCondition(credential => credential.AccountId.Equals(accountId))
                .SingleOrDefault();

            return GetAccountWithCredential(found);
        }

        public Account GetAccountWithCredential(Credential found)
        {
            if (found != null)
            {
                Wrapper.Account.LoadCredential(found);
                return found.Account;
            }

            throw DbEx.Instance.GetThrowable<NotFoundAccountException>();
        }
    }
}