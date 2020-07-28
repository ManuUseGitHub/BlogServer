using Entities.EmmBlog.DataModelObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.EmmBlog.DataTransferObjects.In
{
    public class AccountForCreationDTO

    {
        private Guid id;
        public Guid Id { get {
                if (id == Guid.Empty)
                {
                    id = Guid.NewGuid();
                }
                return id;
            } set {
                id = value;
            } 
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public string Avatar { get; set; }
        public string UserName { get; set; }
        public bool wantsNewsLetter { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime? DateOfBirth { get; set; }

        public Credential Credential { get; set; }

        public int visibilityId = 0; 
        public Visibility Visibility { get;set;}

        public int VisibilityId {
            get
            {
                if (visibilityId == 0)
                {
                    visibilityId = 1; // default is Public= value;
                }
                return visibilityId;
            }
            set
            {
                visibilityId = value;
            }
        } 

        public class CredentialForCreationDTO
        {
            public String MailAddress { get; set; }

            public DateTime RegistrationDate { get ; set; }

            [Required(ErrorMessage = "The password is required")]
            public String PassWord { get; set; }
        }
    }
}
