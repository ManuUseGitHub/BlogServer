using Entities.EmmBlog.DataModelObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Exceptions
{
    public class AccountsExceptions : EmmBlogException<Account>
    {
        public class NotFoundAccountException : NullReferenceException
        {
            public NotFoundAccountException(string message) : base(message)
            { }

            public NotFoundAccountException(Account account) : base (
                ExpliciteNotFoundMessage(account, new object[] {account.Id}))
            { }
        }

        public class BornInFuturException : InvalidOperationException
        {
            public BornInFuturException(string message) : base(message)
            { }
        }

        public class UnpairedAccountAndCredentialException : InvalidOperationException
        {
            public UnpairedAccountAndCredentialException(string message) : base(message)
            { }
        }

        public class FriendlessHiddingException : InvalidOperationException
        {
            public FriendlessHiddingException(string message) : base(message) { }
        }
            
    }
}
