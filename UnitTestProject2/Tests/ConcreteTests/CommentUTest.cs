using Entities.EmmBlog.DataModelObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static Utilities.Reflector;

namespace UnitTestProject.Tests.ConcreteTests
{
    [TestClass]
    public class CommentUTest : UnitTest
    {
        public static Comment FirstComment { get; private set; }

        [ClassInitialize]
        public static void InitializeFirstComment(TestContext testContext)
        {
            Blog blog = Wrapper.Blog.GetBlogOfAccount(JeanLucEmmanuel.Id);

            Comment comment = NullNormalized<Comment>(c =>
            {
                c.Content = "First Comment. by my self haha !";
                c.AccountId = JeanLucEmmanuel.Id;
            });

            comment.Article = ArticleZero;

            FirstComment = Wrapper.Comment.CommentArticle(comment);

            Wrapper.Article.CreateArticle(NullNormalized<Article>(a =>
            {
                a.Content = "Angular is pretty great !";
                a.Title = "Discovered something !";
                a.Slug = "Fifth_Article";
                a.Blog = blog;
                a.VisibilityId = 1;
            }));
        }

        [TestMethod]
        public void ACommentCanBAnswered()
        {
            Account acc = Wrapper.Account.GetAccountByMailAddress("manmdyalw@gmail.com");
            Comment comment = NullNormalized<Comment>(c =>
            {
                c.Content = "Oh my gosh you weirdo !";
                c.AccountId = acc.Id;
                c.Answer = FirstComment;
            });

            Comment answere = Wrapper.Comment.AnswerComment(comment);

            Comment answered = Wrapper.Comment.GetComment(FirstComment);

            Assert.AreEqual(answered.Content, answere.Answer.Content);
        }

        [TestMethod]
        public void ACommentRefersTheLastVersionOfAModifiedArticle()
        {
            Blog blog = Wrapper.Blog.GetBlogOfAccount(JeanLucEmmanuel.Id);
            Article article = NullNormalized<Article>(a =>
                {
                    a.Content = "Dot Net Core 3 Is fun";
                    a.Title = "A Fourth Article, Yay !!!";
                    a.Slug = "Fourth_Article";
                    a.Blog = blog;
                    a.VisibilityId = 1;
                });

            // creation of thee article
            Wrapper.Article.CreateArticle(article);

            Comment comment = NullNormalized<Comment>(c =>
            {
                c.Content = "First!";
                c.AccountId = Manu.Id;
                c.Article = article;
            });

            Comment commented = Wrapper.Comment.CommentArticle(comment);

            // replace article by a copy of itself
            ReferShalowedCopy(article);

            article.Content = ".Net Core 3 is fun !";

            Wrapper.Article.UpdateArticle(article);

            commented = Wrapper.Comment.GetComment(commented);

            Assert.AreEqual(0, commented.Article.VDepth);
        }

        [TestMethod]
        public void AModificationThreadCanBeGetSortedFromTheOldest()
        {

            Comment original = ManusBadReplies(new string[] {
                // the reply
                "Again answering to yourself? You weirdo !!!",

                // the modifications
                "Man go consulting, you weirdo !!!",
                "How funny ^^' you are weird !",
                "How funny you ^^ <3 !"
            });
            
            // check succession
            DateTime previousDate = DateTime.MinValue;
            var thread = Wrapper.Comment.GetCommentRevisions(original);

            foreach (Comment com in thread)
            {
                bool isLess = previousDate.Ticks < com.WriteDate.Value.Ticks;
                Assert.IsTrue(isLess);

                previousDate = com.WriteDate.Value;
            }
        }

        [TestMethod]
        public void ADeletedCommentDeletesAllItsOldVersionsUntilTheFirstOne()
        {
            Comment comment = ManusBadReplies(new string[] {
                // the reply
                "Hey I can do the same ? Wanna see, you weirdo ?!!",

                // the modifications
                "Hey I can do the same ?",
                "Not funny you",
                "..."
            });
            Comment answer = NullNormalized<Comment>(c =>
            {
                c.Content = "Oh my gosh you weirdo !";
                c.AccountId = Manu.Id;
                c.Answer = comment;
            });

            Comment answere = Wrapper.Comment.AnswerComment(answer);
            Comment answered = Wrapper.Comment.GetComment(comment);

            Wrapper.Comment.RemoveComment(comment);

            Comment justHidden = Wrapper.Comment.GetComment(comment);
            Assert.AreEqual(justHidden.VisibilityId,REMOVED);
        }
        [TestMethod]
        public void ADeletedModifiedAnsweredCommentTurnsVisibilitiesOfItsRevisionsToRemoved()
        {
            Comment comment = ManusBadReplies(new string[] {
                // the reply
                "Hey I can do the same ? Wanna see, you weirdo ?!!",

                // the modifications
                "Hey I can do the same ?",
                "Not funny you",
                "..."
            });

            Wrapper.Comment.RemoveComment(comment);
        }
        [TestMethod]
        public void ADeletedAnsweredCommentTurnsItsVisibilitiesToRemoved()
        {
            Comment comment = NullNormalized<Comment>(c =>
            {
                c.Content = "I think that you are a sick person";
                c.AccountId = Manu.Id;
                c.Article = ArticleZero;
            });

            Comment secondComment = Wrapper.Comment.CommentArticle(comment);

            Comment reply = NullNormalized<Comment>(c =>
            {
                c.Content = "Hum pardon, What do you mean ? ...";
                c.AccountId = JeanLucEmmanuel.Id;
                c.Answer = secondComment;
            });

            Comment answere = Wrapper.Comment.AnswerComment(reply);

            Wrapper.Comment.RemoveComment(secondComment);
           Comment removed = Wrapper.Comment.GetComment(secondComment);
            Assert.AreEqual(removed.VisibilityId, REMOVED);
        }

        private Comment ManusBadReplies(string[] replies)
        {
            var modifs = new ArraySegment<string>(replies, 1, replies.Length - 1)
                .ToArray();

            Comment comment = NullNormalized<Comment>(c =>
            {   
                c.Content = replies[0];

                c.AccountId = Manu.Id;
                c.Answer = FirstComment;
            });

            return ChainModify(modifs, Wrapper.Comment.AnswerComment(comment));
        }
    }
}