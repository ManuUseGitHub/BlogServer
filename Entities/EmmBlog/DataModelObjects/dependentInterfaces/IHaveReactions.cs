using Entities.EmmBlog.DataModelObjects;
using System.Collections.Generic;

namespace Entities.EmmBlog.DataModelObjects.dependentInterfaces
{
    public interface IHaveReactions
    {
        public ICollection<ReactionCounter> ReactionCounts { get; set; }
        public ICollection<React> Reactions { get; set; }
    }
}