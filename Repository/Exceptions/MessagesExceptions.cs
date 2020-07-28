using Entities.EmmBlog.DataModelObjects;
using System;
using System.Text;

namespace Repository.Exceptions
{
    public class MessagesExceptions : EmmBlogException<Message>
    {
        public class NoMessageWantedException : InvalidOperationException
        {
            public NoMessageWantedException(string message) : base(message)
            { }

            private static string ExpliciteNotWantedMessage(Message message)
            {
                string identifying = $"{message.To.FirstName}";

                return new StringBuilder()
                    .Append($"Apparently {identifying} does not want to receive your message")
                    .Append("\r\nYou cannot do anything but wait the person allows you the permission")
                    .ToString();
            }

            public NoMessageWantedException(Message message)
                : base(ExpliciteNotWantedMessage(message))
            { }

            public NoMessageWantedException() : base()
            {
            }
        }

        public class NoMessageToBlockedException : InvalidOperationException
        {
            public NoMessageToBlockedException(string message) : base(message)
            { }

            private static string ExpliciteNotWantedMessage(Message message)
            {
                string identifying = $"{message.From.FirstName}";

                return new StringBuilder()
                    .Append($"Apparently you blocked {identifying} you try to reach")
                    .Append("\r\nMaybe consider unblocking the person to send message to him/her.")
                    .ToString();
            }

            public NoMessageToBlockedException(Message message)
                : base(ExpliciteNotWantedMessage(message))
            { }

            public NoMessageToBlockedException() : base()
            {
            }
        }

        public class AnsweredTwiceException : InvalidOperationException
        {
            public AnsweredTwiceException(string message) : base(message)
            { }

            private static string ExpliciteAlreadyAnswered(Message message)
            {
                Message answer = message.Answer;
                string identifying = $"{answer.From.FirstName}";

                return new StringBuilder()
                    .Append($"Apparently this message from {identifying} has already an answer")
                    .ToString();
            }

            public AnsweredTwiceException(Message message)
                : base(ExpliciteAlreadyAnswered(message))
            { }
        }

        public class UnknownTopicException : InvalidOperationException
        {
            public UnknownTopicException(string message) : base (message)
            { }
        }
    }
}