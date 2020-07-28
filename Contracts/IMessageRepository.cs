using Entities.EmmBlog.DataModelObjects;
using Entities.EmmBlog.DataModelObjects.dependentInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IMessageRepository : IRepositoryBase<Message>
    {
        Message ExchangeMessage(Message message);
        ICollection<IWritableDate> GetMessagesBetween(Account from, Account to);
        Message Answer(Message answer);
        void Remove(Message message);
    }
}
