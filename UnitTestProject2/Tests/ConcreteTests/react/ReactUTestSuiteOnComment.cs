using Entities.EmmBlog.DataModelObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace UnitTestProject.Tests.ConcreteTests
{
    public partial class ReactUTest : UnitTest
    {
        [TestMethod]
        public void AModifiedCommentReflectItsReactionsOnTheMostUpToDateVersion()
        {
            Comment commented = CreateCommentedArticleByManu(a =>
            {
                a.Content = "The need of coupling tehcnologies is a thing!";
                a.Title = "A 9th Article, Yeep yeep !!!";
                a.Slug = "Ninth_Article";
                a.VDepth = 0;
            }, "You made a typo");

            React reacted = Wrapper.React.React(NullNormalized<React>(r =>
            {
                r.Comment = commented;
                r.Account = Manu;
                r.Reaction = new ReactionType { TypeId = LIKE };
            }));

            Comment updated = commented;

            string lastModification =
                "Of course it has to be that way but every strate can be choose I think!" +
                " (Re changend)";

            string[] modifs = new string[]
            {
                "Of course it has to be that way but every strate can be choose I think!",
                "I am kidding ...",
                "No I am not, my young brother changed my comment",
                lastModification
            };

            foreach (string modif in modifs)
            {
                updated = ChangeEntity(updated, c => { c.Content = modif; });
            }

            React reaction = Wrapper.React.GetReact(updated, Manu);

            Assert.AreEqual(lastModification, updated.Content);
        }

        [TestMethod]
        public void ACreatedAfterModifiedCommentReactionPointsOnOriginal()
        {
            Article article = null;
            Comment commented = CreateCommentedArticleByManu(out article, a =>
            {
                a.Content = "Who do loves pizza?";
                a.Title = "Being well within yourself is important";
                a.Slug = "Eleventh_Article";
            }, "I do actually");

            Comment updated = ChainModify(new string[]
            {
                "Welle who do not love pizza her",
                "Well, I chalenge you to find someone who do not love pizza here :p ."
            }, commented);

            React reacted = Wrapper.React.React(NullNormalized<React>(r =>
            {
                r.Comment = updated;
                r.Account = Manu;
                r.Reaction = new ReactionType { TypeId = LOVE };
            }));

            Assert.AreEqual(reacted.RDepth, 0);
        }

        [TestMethod]
        public void RemoveAnAnsweredCommentRemovesItsReactions()
        {
            Comment comment = NullNormalized<Comment>(c =>
            {
                c.Content = "I think that you are a sick person second";
                c.AccountId = Manu.Id;
                c.Article = ArticleZero;
            });

            Comment commented = Wrapper.Comment.CommentArticle(comment);

            React reacted = Wrapper.React.React(NullNormalized<React>(r =>
            {
                r.Comment = commented;
                r.Account = JeanLucEmmanuel;
                r.Reaction = new ReactionType { TypeId = SAD };
            }));

            Comment reply = NullNormalized<Comment>(c =>
            {
                c.Content = "Hum pardon, What do you mean ? ... second also";
                c.AccountId = JeanLucEmmanuel.Id;
                c.Answer = commented;
            });

            Comment answere = Wrapper.Comment.AnswerComment(reply);

            Comment copy = Wrapper.Comment.RemoveComment(commented);

            Comment removed = GetCommentLike(copy);
            React unexistant = Wrapper.React.GetReact(commented,JeanLucEmmanuel);

            Assert.AreEqual(removed.VisibilityId,REMOVED);
            Assert.IsNull(unexistant);
        }

        [TestMethod]
        public void OnlyATop3OfReactionsCanBeGetFromComment()
        {
            Article article = null;
            Comment commented = CreateCommentedArticleByManu(out article, a =>
            {
                a.Content = "Who do loves pets?";
                a.Title = "Being well with animals is important";
                a.VDepth = 0;
            }, "I do actually");

            foreach ((string[], int) attribution
                in GetAttributions())
            {
                int reactionId = attribution.Item2;
                string[] mailAddresses = attribution.Item1;

                AssignReactionsToComment(reactionId, mailAddresses, commented);
            };

            ICollection<ReactionCounter> resulted = GetCommentLike(commented)
                .ReactionCounts;

            Assert.AreEqual(resulted.Count, 3);
        }
    }
}