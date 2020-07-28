using Entities.EmmBlog.DataModelObjects;
using System;

namespace Entities.EmmBlog.DataTransferObjects.Out.Reduced
{
    public class SubscriptionOfBlogDTO
    {
        public AccountOfSubscriptionDTO Account { get; set; }

        public class AccountOfSubscriptionDTO
        {
            public Guid Id { get; set; }

            public string FirstName { get; set; }
            public string LastName { get; set; }

            public DateTime DateOfBirth { get; set; }
            public string Avatar { get; set; }
            public string UserName { get; set; }
            public bool wantsNewsLetter { get; set; }

            public Credential Credential { private get; set; }

            public string MailAddress { get { return Credential.MailAddress; }  }
            public DateTime RegistrationDate { get { return Credential.RegistrationDate; }  }
        }
    }
}
