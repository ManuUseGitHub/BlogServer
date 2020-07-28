using Entities.EmmBlog.DataModelObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.EmmBlog.DataTransferObjects.Out.Reduced
{
    public class ProfileAccountDTO
    {
        [Column("Id")]
        public Guid ID { get; set; }

        // unaccessible field (private get) => unlisted via the DTO
        public string FirstName { private get; set; }
        public string LastName { private get; set; }

        public string FullName
        {
            // readonly
            get { return FirstName + " " + LastName; }
        }

        public DateTime DateOfBirth { get; set; }
        public string Avatar { get; set; }
        public string UserName { get; set; }
        public bool wantsNewsLetter { get; set; }

        public string MailAddress { get { return Credential.MailAddress; } }
        public DateTime RegistrationDate { get { return Credential.RegistrationDate; } }

        public Credential Credential { private get; set; }
    }
}
