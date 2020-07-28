using Contracts;
using Entities.EmmBlog.DataModelObjects;
using Entities.EmmBlog.DataModelObjects.dependentInterfaces;
using System.Collections;
using static Contracts.INotificationRepository;

namespace Repository
{
    public partial class NotificationRepository : RepositoryBase<Notification>, INotificationRepository
    {
        public Notification CreateAFriendRequest(Follow follow)
        {
            Notification newNotif = GetNewNotificaion(n => {
                n.AccountId = follow.FollowedId;

                n.Content = 
                    $"{follow.Following.FirstName} " +
                    $"{follow.Following.LastName} " +
                    "Wants to be a one of your friends.";
            });

            Create(newNotif);
            TrySave();

            return GetNotification(newNotif.NotifId);

        }

        public Notification CreateAcceptedFriendship(Follow friendship)
        {
            Notification newNotif = GetNewNotificaion(n => {
                n.AccountId = friendship.FollowingId;

                n.Content =
                    $"{friendship.Followed.FirstName} " +
                    $"{friendship.Followed.LastName} " +
                    "Has accepted to be your friend.";
            });

            Create(newNotif);
            TrySave();

            return GetNotification(newNotif.NotifId);
        }

        public Notification CreateAShare(Share newShare)
        {
            Account acc = newShare.Blog.Account;
            
            Account author = newShare
                .Article
                .Blog
                .Account;
            
            Notification newNotif = GetNewNotificaion(n => {
                n.AccountId = author.Id;

                n.Content =
                    $"{acc.FirstName} " +
                    $"{acc.LastName} " +
                    "Has shared your article.";
            });

            Create(newNotif);
            TrySave();

            return GetNotification(newNotif.NotifId);
        }

        public void CreateArticle(Article article)
        {
            Blog blog = article.Blog;
            ICollection allowed = Wrapper.Blog.GetAllowedAccounts(blog);

            Account author = blog.Account;

            foreach (Account account in allowed)
            {

                Create(GetNewNotificaion(n =>
                {
                    n.AccountId = account.Id;
                    n.Content = $"{author.FirstName} Has writen a new article on {blog.BlogName}";
                }));
            }

            TrySave();

        }

        public void NotifyAllowedAccountOf<T>(T item, NotificationTopic topic) where T : IHaveAccount
        {
            Blog blog = Wrapper.Blog.GetBlogOfAccount(item.Account.Id);
            ICollection allowed = Wrapper.Blog.GetAllowedAccounts(blog);

            Account author = blog.Account;
            string subject = GetNotificationTopic(topic);

            foreach (Account account in allowed)
            {

                Create(GetNewNotificaion(n =>
                {
                    n.AccountId = account.Id;
                    n.Content = $"{author.FirstName} {author.LastName} {subject}";
                }));
            }

            TrySave();
        }

        public void CreateCommented(Account author, Account commenter)
        {
            Create(GetNewNotificaion(n =>
            {
                n.AccountId = author.Id;
                n.Content = commenter.FirstName + " has commented your article";
            }));

            TrySave();
        }

        public void CreateMessageReceived(Account to, Account from)
        {
            Create(GetNewNotificaion(n =>
            {
                n.AccountId = to.Id;
                n.Content = "You have got a message from " + from.FirstName;
            }));

            TrySave();
        }

        public void CreateReactionArticle(Account account, React react)
        {
            Create(GetNewNotificaion(n =>
            {
                n.AccountId = account.Id;
                n.Content =
                    $"{react.Account.FirstName} reacted " +
                    $"{react.Reaction.Label} to your article";
            }));

            TrySave();
        }

        public void CreateReactionComment(Account account, React react)
        {
            Create(GetNewNotificaion(n =>
            {
                n.AccountId = account.Id;
                n.Content =
                    $"{react.Account.FirstName} reacted " +
                    $"{react.Reaction.Label} to your comment";
            }));

            TrySave();
        }

    }
}
