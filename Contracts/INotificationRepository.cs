using Entities.EmmBlog.DataModelObjects;
using Entities.EmmBlog.DataModelObjects.dependentInterfaces;
using System.Collections.Generic;

namespace Contracts
{
    public interface INotificationRepository : IRepositoryBase<Notification>
    {
        public enum NotificationTopic
        {
            BLOG_NAME,
            NEW_ARTICLE,
            ARTICLE_NAME,
            IMPORTANT_UPDATE,
            ANNIVERSARY
        }

        ICollection<Notification> GetNotoficationsOf(Account account);
        Notification CreateAFriendRequest(Follow follow);
        Notification CreateAcceptedFriendship(Follow friendship);
        void CreateMessageReceived(Account from, Account from1);
        void CreateCommented(Account account1, Account account2);
        void CreateReactionArticle(Account account, React react);
        void NotifyAllowedAccountOf<T>(T item, NotificationTopic topic) where T : IHaveAccount;
        void CreateReactionComment(Account account, React react);
        int GetNotoficationsCount();
        void CreateArticle(Article article);
        Notification CreateAShare(Share newShare);
    }
}
