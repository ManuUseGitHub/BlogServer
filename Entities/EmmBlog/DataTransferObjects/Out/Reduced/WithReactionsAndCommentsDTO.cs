using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.EmmBlog.DataTransferObjects.Out.Reduced
{
    public abstract class WithReactionsAndCommentsDTO
    {
        public ICollection<ReactionDTO> Reactions { get; set; }
        public Dictionary<string, int> ReactionCounts { private get; set; }
        public Dictionary<string, int> TopReactions
        {
            get
            {
                return ReactionCounts;
            }
        }
    }
}
