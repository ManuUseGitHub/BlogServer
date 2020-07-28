using Contracts;
using Entities;
using Entities.EmmBlog.DataModelObjects;
using Entities.EmmBlog.DataModelObjects.dependentInterfaces;
using Repository.Exceptions;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Runtime.ExceptionServices;
using Utilities;
using static Contracts.INotificationRepository;
using static Repository.Exceptions.MessagesExceptions;

namespace Repository
{
    public partial class NotificationRepository : RepositoryBase<Notification>, INotificationRepository
    {
        public NotificationRepository(
            RepositoryWrapper repositoryWrapper,
            RepositoryContext repositoryContext)
            : base(repositoryWrapper, repositoryContext)
        { }

        public string GetNotificationTopic(NotificationTopic topic)
        {
            string phrase;
            switch (topic)
            {
                case NotificationTopic.BLOG_NAME:
                    phrase = "has changed the name of its blog!!";
                    break;
                case NotificationTopic.ARTICLE_NAME:
                    phrase = "has changed the name of its article";
                    break;
                    
                case NotificationTopic.NEW_ARTICLE:
                    phrase = "has written a newx article";
                    break;
                    
                case NotificationTopic.IMPORTANT_UPDATE:
                    phrase = "has achieved something important";
                    break;
                    
                case NotificationTopic.ANNIVERSARY:
                    phrase = "has its bithday";
                    break;
                default:
                    throw DbEx.Instance.GetThrowable<UnknownTopicException>();
            }

            return phrase;
        }

        private Notification GetNewNotificaion(Action<Notification> action)
        {
            Notification data = new NullNormalizeFactory<Notification>(action)
                .Instance;

            Notification newNotif = new Notification()
            {
                KindId = (int)Kind.NotificationKind.Social,
                NotifId = Guid.NewGuid(),
                WriteDate = DateTime.Now,
                Seen = false
            };

            Reflector.Merge(newNotif, data);

            return newNotif;
        }
    }
}