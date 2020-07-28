using Contracts;
using Entities;
using Entities.EmmBlog.DataModelObjects;
using Entities.EmmBlog.DataModelObjects.dependentInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public partial class ReactRepository : EmmBlogRepositoryBase<React>, IReactRepository
    {
        public React React(React reaction)
        {
            // set a reaction on the version/revision 0
            SetReferenceKeys(reaction);

            Create(reaction);

            TrySave();

            React result = FindByCondition(r =>
                    r.ItemId.Equals(reaction.ItemId) &&
                    r.TypeId.Equals(reaction.TypeId))
                    .Include(r => r.Reaction)
                .FirstOrDefault();

            if (result.Article != null)
            {
                Wrapper.Notificaton
                    .CreateReactionArticle(result.Article.Blog.Account, result);
            }

            else if (result.Comment != null)
            {
                Wrapper.Notificaton
                    .CreateReactionComment(result.Comment.Account, result);
            }


            return result;
        }
    }
}