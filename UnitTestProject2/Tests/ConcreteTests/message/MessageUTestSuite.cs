using Entities.EmmBlog.DataModelObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Repository.Exceptions.MessagesExceptions;

namespace UnitTestProject.Tests.ConcreteTests
{
    public partial class MessageUTest : UnitTest
    {
        [TestMethod]
        public void DeletingAMessageDoesnotDeleteItsAnswer()
        {
            Message first = new Message()
            {
                Content = "Hello me",
                From = Manu,
                To = JeanLucEmmanuel,
                WriteDate = DateTime.Now.AddMinutes(0)
            };

            Message sent = Wrapper.Message.ExchangeMessage(first);

            Wrapper.Message.Answer(new Message()
            {
                Answer = sent,
                Content = "Hey we!"
            });

            Wrapper.Message.Remove(first);
        }
        [TestMethod]
        public void AnAnswerCanBeAnAnswerOfAnAnswerOfAnAnswerOfAMessage()
        {
            Message first = new Message()
            {
                Content = "Hey us",
                From = Manu,
                To = JeanLucEmmanuel,
                WriteDate = DateTime.Now.AddMinutes(0)
            };

            Message answer = Wrapper.Message.ExchangeMessage(first);

            foreach (string content in new string[] {
                "Hey you!", "Testing" , "Something" , "that", "can", "be","of interest"
            })
            {
                Message newanswer = Wrapper.Message.Answer(new Message()
                {
                    Answer = answer,
                    Content = content
                });

                answer = newanswer;
            }

        }
        [TestMethod]
        public void MessagesAreSortedByDate()
        {
            Message first = new Message()
            {
                Content = "Hello, I wanted to reach out to discuss about something :)",
                From = Manu,
                To = JeanLucEmmanuel,
                WriteDate = DateTime.Now.AddMinutes(0)
            };

            Message secound = new Message()
            {
                Content = "Hi, of course, go on",
                From = JeanLucEmmanuel,
                To = Manu,
                WriteDate = DateTime.Now.AddMinutes(5)
            };

            Message third = new Message()
            {
                Content = "I expected it would not take to long to answer tho",
                From = JeanLucEmmanuel,
                To = Manu,
                WriteDate = DateTime.Now.AddMinutes(6)

            };

            Wrapper.Message.ExchangeMessage(first);
            Wrapper.Message.ExchangeMessage(secound);
            Wrapper.Message.ExchangeMessage(third);

            // check succession
            DateTime previousDate = DateTime.MinValue;
            var thread = Wrapper.Message
                .GetMessagesBetween(Manu, JeanLucEmmanuel);

            foreach (Message mess in thread)
            {
                bool isLess = previousDate.Ticks < mess.WriteDate.Value.Ticks;
                Assert.IsTrue(isLess);

                previousDate = mess.WriteDate.Value;
            }
        }

        [TestMethod, ExpectedException(typeof(AnsweredTwiceException))]
        public void AMessageCannotBeAnAnsweredTwice()
        {
            Message first = new Message()
            {
                Content = "Hey me",
                From = Manu,
                To = JeanLucEmmanuel,
                WriteDate = DateTime.Now.AddMinutes(0)
            };

            Message sent = Wrapper.Message.ExchangeMessage(first);

            Wrapper.Message.Answer(new Message()
            {
                Answer = sent,
                Content = "Hey you!"
            });

            Wrapper.Message.Answer(new Message()
            {
                Answer = sent,
                Content = "Testing"
            });
        }

        [TestMethod]
        public void AMessageCanBeSentAsAnAnswer()
        {
            Message first = new Message()
            {
                Content = "Hey me",
                From = Manu,
                To = JeanLucEmmanuel,
                WriteDate = DateTime.Now.AddMinutes(0)
            };

            Message sent = Wrapper.Message.ExchangeMessage(first);

            Wrapper.Message.Answer(new Message()
            {
                Answer = sent,
                Content = "Hey you!"
            });
        }
        
        [TestMethod, ExpectedException(typeof(NoMessageWantedException))]
        public void ABlockedAccountCannotSendAnyMessage()
        {
            Account Rosy = Wrapper
                    .Account
                    .SearchByFullName("Rose Beaf").FirstOrDefault();

            Follow blocking = NullNormalized<Follow>(f =>
            {

                // who blocks
                f.Following = Manu;

                // who is blocked
                f.Followed = Rosy;
            });

            Wrapper.Follow.MakeABlock(blocking);

            Message message = new Message()
            {
                Content = "Hey why do you stay so silent ?!",
                From = Rosy,
                To = Manu,
                WriteDate = DateTime.Now.AddMinutes(6)

            };

            Wrapper.Message.ExchangeMessage(message);
        }

        [TestMethod, ExpectedException(typeof(NoMessageToBlockedException))]
        public void ABlockingAccountCannotSendAnyMessage()
        {
            Account Crishian = Wrapper
                    .Account
                    .SearchByFullName("Crishian Doe").FirstOrDefault();

            Follow blocking = NullNormalized<Follow>(f =>
            {
                // who blocks
                f.Following = Manu;

                // who is blocked
                f.Followed = Crishian;
            });

            Wrapper.Follow.MakeABlock(blocking);

            Message message = new Message()
            {
                Content = "I want to give you another chance",
                From = Manu,
                To = Crishian,
                WriteDate = DateTime.Now.AddMinutes(6)

            };

            Wrapper.Message.ExchangeMessage(message);
        }
    }
}
