using Contracts;
using Entities;
using Entities.EmmBlog.DataModelObjects;
using Entities.EmmBlog.DataModelObjects.dependentInterfaces;
using Entities.EmmBlog.DataModelObjects.Utilities;
using Repository.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;
using static Repository.Exceptions.MessagesExceptions;

namespace Repository
{
    internal class MessageRepository : EmmBlogRepositoryBase<Message>, IMessageRepository
    {
        public MessageRepository(
            RepositoryWrapper repositoryWrapper,
            RepositoryContext repositoryContext)
            : base(repositoryWrapper, repositoryContext)
        { }

        public Message ExchangeMessage(Message message)
        {
            Message toDeliver = new NullNormalizeFactory<Message>(m =>
            {
                m.WriteDate = DateTime.Now;
                m.ToId = message.To.Id;
                m.FromId = message.From.Id;
                m.StateId = M_SENT;
            }).Instance;

            //throws if one person of the relation has blocked the other one
            CheckBlocking(toDeliver);

            Message found = GetMessage(message);
            Reflector.Merge(toDeliver, message);

            toDeliver.MessageId = found != null ? found.MessageId + 1 : 0;

            Create(toDeliver);
            TrySave();

            Wrapper.Notificaton.CreateMessageReceived(message.To,message.From);

            return GetMessage(toDeliver);
        }

        public Message Answer(Message message)
        {
            Message answer = GetMessage(message.Answer);
            Message found = GetMessage(new Message()
            {
                ToId = answer.From.Id,
                FromId = answer.To.Id
            });
            if(FindByCondition(m =>
                m.AnswerFromId.Equals(answer.From.Id) &&
                m.AnswerId.Equals(answer.MessageId) &&
                m.AnswerToId.Equals(answer.To.Id)).FirstOrDefault()
            != null){
                DbEx.Instance.Throw<AnsweredTwiceException>(message);
            }

            Message toDeliver = new NullNormalizeFactory<Message>(m =>
            {
                m.WriteDate = DateTime.Now;

                m.AnswerFromId = answer.From.Id;
                m.AnswerToId = answer.To.Id;
                
                m.ToId = answer.From.Id;
                m.FromId = answer.To.Id;

                m.StateId = M_SENT;
            }).Instance;

            //throws if one person of the relation has blocked the other one
            CheckBlocking(toDeliver);

            Reflector.Merge(toDeliver, message);

            toDeliver.MessageId = found != null ? found.MessageId + 1 : 0;

            Create(toDeliver);
            TrySave();

            return GetMessage(toDeliver);
        }

        private bool IsBlocking(Guid? followedId, Guid? followingId)
        {
            Follow firstFollow = Wrapper.Follow.GetFollow(new Follow()
            {
                FollowedId = followedId,
                FollowingId = followingId
            });

            return firstFollow != null ? firstFollow.IsBlocking : false;
        }

        private void CheckBlocking(Message message)
        {
            if (IsBlocking(message.FromId, message.ToId))
            {
                // the person supposed to receive does not allow
                if(message.To == null)
                {
                    message.To = Wrapper.Account
                        .GetAccountById(message.ToId.Value);
                }
                DbEx.Instance.Throw<NoMessageWantedException>(message);
            }

            if (IsBlocking(message.ToId,message.FromId))
            {
                if (message.From == null)
                {
                    message.From = Wrapper.Account
                        .GetAccountById(message.FromId.Value);
                }
                DbEx.Instance.Throw<NoMessageToBlockedException>(message);
            }
        }

        public Message GetMessage(Message message)
        {
            if (message.FromId == null)
            {
                message.FromId = message.From.Id;
            }

            if (message.ToId == null)
            {
                message.ToId = message.To.Id;
            }

            var query = FindByCondition(m =>
                m.FromId.Equals(message.FromId) &&
                m.ToId.Equals(message.ToId));

            // take the message id in count if specified

            if (message.MessageId != null)
            {
                query = query.Where(m => m.MessageId.Equals(message.MessageId));
            }

            return query.OrderByDescending(m => m.MessageId).FirstOrDefault();
        }

        public ICollection<IWritableDate> GetMessagesBetween(Account from, Account to)
        {
            IEnumerable<IWritableDate> query = RepositoryContext.Messages
                .Where(m =>
                    m.FromId.Equals(from.Id) &&
                    m.ToId.Equals(to.Id) ||

                    // the inverse discussion
                    m.FromId.Equals(to.Id) &&
                    m.ToId.Equals(from.Id)
                ).ToList();

            return DataPresenter.GetSortedByWriteDate(query.ToList()).ToList();
        }

        public void Remove(Message message)
        {
            Message found = GetMessage(message);

            found.StateId = M_REMOVED;
        }
    }
}