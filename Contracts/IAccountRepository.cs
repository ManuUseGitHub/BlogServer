using Entities.EmmBlog.DataModelObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Contracts
{
    public interface IAccountRepository
    {
        public Account GetAccountById(Guid? id);
        public void LoadCredential(Object entity);
        Account CreateAccount(Account accountEntity);
        void DeleteAccount(Account account);
        object GetAccountCredential(Guid? id);
        Account GetAccountByMailAddress(string v);
        ICollection<Account> SearchByFullName(string fullName, Guid? asId = null);
        Account GetEndAccountById(Guid? id);
        void ChangeInfo(Account changed);
    }
}
