using Contracts;
using Entities.EmmBlog.DataModelObjects;
using Utilities;
using static Contracts.INotificationRepository;
using static Contracts.IRecordKind;

namespace Repository
{
    public partial class ArticleRepository : EmmBlogRepositoryBase<Article>, IArticleRepository
    {
        public Article UpdateArticle(Article changed)
        {
            Article oldest = Util.GetCopyOf(GetArticle(changed, RecordKind.OLDEST));
            Article actual = Util.GetCopyOf(GetArticle(changed, RecordKind.UPTODATE));

            // remove dependencies
            oldest.Shares = null;
            oldest.Comments = null;
            oldest.Blog = null;

            // create an archive (of version ++)
            ++oldest.VDepth;
            Create(oldest);
            TrySave();

            UdateArchieve(changed);

            // merge changes with the depth 0
            Reflector.Merge(GetArticle(changed), changed);
            TrySave();

            changed = Util.GetCopyOf(GetArticle(changed, RecordKind.UPTODATE));

            if (!actual.Title.Equals(changed.Title))
            {
                int similarity = new Comparator().SentenceCompare(actual.Title, changed.Title);

                if (similarity <= 50)
                {
                    NotificationTopic topic = NotificationTopic.ARTICLE_NAME;
                    Wrapper
                        .Notificaton
                        .NotifyAllowedAccountOf(changed.Blog, topic);
                }
            }
            return GetArticle(changed);
        }
    }
}