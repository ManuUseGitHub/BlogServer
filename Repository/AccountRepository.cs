using Contracts;
using Entities;
using Entities.EmmBlog.DataModelObjects;
using Entities.EmmBlog.DataModelObjects.dependentInterfaces;
using Repository.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;
using static Repository.Exceptions.AccountsExceptions;

namespace Repository
{
    internal class AccountRepository : EmmBlogRepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(
            RepositoryWrapper repositoryWrapper,
            RepositoryContext repositoryContext)
            : base(repositoryWrapper, repositoryContext)
        { }

        public void ChangeInfo(Account changed)
        {
            ICollection<Account> friends = Wrapper.Follow.GetFriendList(changed.Id);

            if(friends.Count == 0 && changed.VisibilityId == FRIENDS)
            {
                DbEx.Instance.Throw<FriendlessHiddingException>();
            }

            Update(changed);
            TrySave();
        }

        public Account CreateAccount(Account accountEntity)
        {
            if (accountEntity.VisibilityId != 1)
            {
                DbEx.Instance.Throw<NonPublicVisibilityException>();
            }
            if (accountEntity.Credential == null)
            {
                DbEx.Instance.Throw<UnpairedAccountAndCredentialException>();
            }
            if (accountEntity.DateOfBirth > DateTime.Now)
            {
                DbEx.Instance.Throw<BornInFuturException>();
            }

            accountEntity.Credential.RegistrationDate = DateTime.Now;
            Create(accountEntity);

            TrySave();
            return GetAccountById(accountEntity.Id);
        }

        public void DeleteAccount(Account account)
        {
            var relatedCredential = Wrapper.Credential
                .FindByCondition(c => c.AccountId.Equals(account.Id))
                .FirstOrDefault();

            if (relatedCredential != null)
            {
                Wrapper.Credential.Delete(relatedCredential);
            }

            Delete(account);
        }

        public Account GetAccountById(Guid? id)
        {
            Account found = FindByCondition(a => a.Id.Equals(id)).FirstOrDefault();

            if (found == null)
            {
                DbEx.Instance.Throw<NotFoundAccountException>();
            }

            return found;
        }

        public Account GetAccountByMailAddress(string mailAddress)
        {
            return Wrapper.Credential.GetAccountByMailAddress(mailAddress);
        }

        public object GetAccountCredential(Guid? id)
        {
            return Wrapper.Credential.FindByCondition(c => c.MailAddress.Equals(id));
        }

        public Account GetEndAccountById(Guid? id)
        {
            Account copy = Util.GetCopyOf(GetAccountById(id));
            copy.Followers = copy.Followers
                       .Where(f => !f.IsBlocking)
                       .ToList();
            return copy;
        }

        public void LoadCredential(Object entity)
        {
            Account account;
            if (entity == null)
            {
                return;
            }

            // if has an account ...
            if (typeof(IHaveAccount).IsAssignableFrom(entity.GetType()))
            {
                // no accopunt loaded so it is a null object at property Account
                if ((entity as IHaveAccount).Account == null)
                {
                    RepositoryContext.Entry(entity as IHaveAccount)
                    .Reference(e => e.Account)
                    .Load();
                }

                account = (entity as IHaveAccount).Account;
            }

            // it is an account
            else
            {
                account = entity as Account;
            }

            // loading of the credential
            RepositoryContext.Entry(account)
                .Reference(a => a.Credential)
                .Load();
        }

        public ICollection<Account> SearchByFullName(
            string fullName, Guid? asId = null
        )
        {
            string[] namesPart = fullName.Split(' ');

            IQueryable<Account> result = FindByCondition(a =>

              a.FirstName.Equals(namesPart[0]) &&
              a.LastName.Equals(namesPart[1]));

            if (asId != null)
            {
                ICollection<Follow> followers = GetAccountById(asId.Value)
                    .Followers;

                foreach (Follow fol in followers)
                {
                    // adding a rule to ignore Blocked accounts
                    result = result.Where(f => !fol.IsBlocking);
                }
            }

            ICollection<Account> list = result.ToList();
            if (list.Count == 0)
            {
                DbEx.Instance.Throw<NotFoundAccountException>();
            }

            return list;
        }
    }
}