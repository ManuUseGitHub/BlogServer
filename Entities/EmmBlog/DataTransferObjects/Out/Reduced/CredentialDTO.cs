using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.EmmBlog.DataTransferObjects.Out.Reduced
{
    public class CredentialDTO
    {
        public string MailAddress { get; set; }

        public DateTime RegistrationDate { get; set; }

        public AccountOfCredentialDTO Account { get; set; }

        public class AccountOfCredentialDTO
        {
            public Guid Id { get; set; }

            public string FirstName { get; set; }
            public string LastName { get; set; }

            public DateTime DateOfBirth { get; set; }
            public string Avatar { get; set; }
            public string UserName { get; set; }
            public bool wantsNewsLetter { get; set; }
        }
    }


}
