using Entities.EmmBlog.DataModelObjects;
using Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static Contracts.IRecordKind;
using static Utilities.Reflector;

namespace Repository
{
    public partial class CommentRepository : EmmBlogRepositoryBase<Comment>, ICommentRepository
    {
        public Comment ChangeComment(Comment changed)
        {
            Comment oldest = Util.GetCopyOf(GetComment(changed, RecordKind.OLDEST));

            // remove dependencies
            oldest.Reactions = null;
            oldest.Answer = null;
            oldest.Account = null;

            // create an archive (of version ++)
            ++oldest.RDepth;
            Create(oldest);
            TrySave();

            UpdateArchieve(changed);

            Comment stored = GetComment(changed);
            // merge changes with the depth 0
            Reflector.Merge(stored, changed);
            TrySave();

            return GetComment(changed);
        }

        private void UpdateArchieve(Comment changed)
        {
            Guid? commentId = changed.Id;

            var archieve = FindByCondition(c =>
               c.Id.Equals(commentId) &&
               c.RDepth > 0)
                .OrderByDescending(c => c.RDepth)
                .ToList();

            foreach (Comment deeper in archieve)
            {
                Comment comment = Util.GetCopyOf(FindByCondition(c =>
                    c.Id.Equals(deeper.Id) &&
                    c.RDepth.Equals(deeper.RDepth - 1)
                ).FirstOrDefault());

                // remove dependencies

                Reflector.Merge(deeper, comment,

                    // default keep source value when collision
                    MergeOptions.KEEP_SOURCE,

                    // else apply the inversed rule for these as KEEP_TARGET
                    // these fields tend ti attach the entity to existing in
                    // the database. It can lead to dependencies problems otherwise
                    new List<PropertyInfo>() {
                        GetPInfo(comment, a => comment.Reactions),
                        GetPInfo(comment, a => comment.RDepth)
                    });

                TrySave();
            }
        }
    }
}