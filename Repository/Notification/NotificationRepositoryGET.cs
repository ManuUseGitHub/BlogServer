using Contracts;
using Entities.EmmBlog.DataModelObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public partial class NotificationRepository : RepositoryBase<Notification>, INotificationRepository
    {
        public Notification GetNotification(Guid? id)
        {
            return FindByCondition(n => n.NotifId.Equals(id))
                .FirstOrDefault();
        }

        public ICollection<Notification> GetNotoficationsOf(Account account)
        {
            return FindByCondition(n => n.AccountId.Equals(account.Id))
                .ToList();
        }

        public int GetNotoficationsCount()
        {
            return RepositoryContext.Notifications.Count();
        }
    }
}
