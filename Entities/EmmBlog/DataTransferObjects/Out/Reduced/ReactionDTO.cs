using Entities.EmmBlog.DataModelObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.EmmBlog.DataTransferObjects.Out.Reduced
{
    public class ReactionDTO
    {
        public string Label { get { return Reaction.Label; } }
        public ReactionType Reaction { private get; set; }

        public Account Account { private get; set; }

        public string UserName { get { return Account.UserName; } }
        public Guid? AccountId { get { return Account.Id; } }
    }
}
