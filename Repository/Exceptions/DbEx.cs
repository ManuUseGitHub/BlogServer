using System;
using System.Collections.Generic;
using static Repository.Exceptions.AccountsExceptions;
using static Repository.Exceptions.ArticlesExceptions;
using static Repository.Exceptions.BlogsExceptions;
using static Repository.Exceptions.CommentsExceptions;
using static Repository.Exceptions.FollowStatusExceptions;
using static Repository.Exceptions.MessagesExceptions;

namespace Repository.Exceptions
{
    public class DbEx : Singleton<DbEx>
    {
        private Dictionary<Type, string> messages;

        public DbEx()
        {
            messages = new ErrorMessageBuilder()
                .Add<NonPublicVisibilityException>("This new Entity has to be added as a public one first")
                .Add<UnpairedAccountAndCredentialException>("An new account should be paired with credentials")
                .Add<BornInFuturException>("We cannot create an account for someone born in the futur... Check your birth date")
                .Add<NotFoundAccountException>("We could not find an account with the data given")
                .Add<FriendlessHiddingException>("A friendless account can only be visible as public")

                .Add<NotFoundBlogException>("We could not find a blog with the data given")

                .Add<NoCommentFoundException>("We could not find a comment with the data given")
                .Add<NoAnsweredCommentCanBeDeletedException>("You cannot delete a comment that is answered")

                .Add<TwiceAddingArticleException>("An already existing article is prensent. Thus, it cannot be add twice")
                .Add<NoArticleFoundException>("We could not find a blog with the data given")
                .Add<NoReferencedArticleCanBeDeletedException>("You cannot delete an article that has deêndent entries")

                .Add<NoMessageWantedException>("")
                .Add<NoMessageToBlockedException>("")
                .Add<AnsweredTwiceException>("Already answered message")

                .Add<WrongBlockingException>("Blocking to Blocking")
                .Add<WrongAllowingException>("Allowing an Unblocking")
                .Add<WrongClaimingException>("Can only Claim a friendship if there is a follow or nothing")
                .Add<WrongCancellationRequest>("Can only Cancel pending requests")
                .Add<WrongFriendshipRemoval>("Can only remove friends")
                .Add<WrongFriendshipCreation>("No object relation for the fresh new friendship only")
                .Add<WrongFollowException>("No object relation for the fresh new following only")
                .Add<WrongUnfollowingException>("Only a folloing state can stop a following")
                .Add<WrongFriendshipAcceptation>("Only pending friendships can be candidate for acceptance")

                .Add<UnknownTopicException>("Unknown Notification topic")
                


                .Build();
        }

        public void Throw<TCustomException>(Object entity = null) where TCustomException : Exception
        {
            string exceptionMessage = messages[typeof(TCustomException)];
            
            if(entity != null)
            {
                /* append a complement exception message defined
                 * by the exception class TCustomException given
                 * if the entity is given by this Call of throw */

                TCustomException ex = (TCustomException)Activator
                .CreateInstance(typeof(TCustomException), entity);

                /* concatenate the default exception message with
                 * the complement message*/

                exceptionMessage += $"\r\n{ex.Message}";
            }

            throw (TCustomException)Activator
                .CreateInstance(typeof(TCustomException), exceptionMessage);
        }

        public Exception GetThrowable<TCustomException>() where TCustomException : Exception
        {
            string exceptionMessage = messages[typeof(TCustomException)];

            return (TCustomException)Activator
                .CreateInstance(typeof(TCustomException), exceptionMessage);
        }

        private class ErrorMessageBuilder
        {
            private Dictionary<Type, string> exceptions = new Dictionary<Type, string>();
            
            /// <summary>
            /// Adds an entry to the messages dictionary
            /// </summary>
            /// <typeparam name="TCustomException"> [Key] The type of the exception </typeparam>
            /// <param name="exceptionMessage"> [Value] The message for the exception </param>
            /// <returns>The same instance of the ErrorMessageBuilder</returns>
            public ErrorMessageBuilder Add<TCustomException>(string exceptionMessage) 
                where TCustomException : Exception
            {
                exceptions.Add(typeof(TCustomException), exceptionMessage);
                return this;
            }

            /// <summary>
            /// Returns the message dictionary
            /// </summary>
            /// <returns>The message dictionary</returns>
            public Dictionary<Type, string> Build()
            {
                return exceptions;
            }
        }
    }
}
