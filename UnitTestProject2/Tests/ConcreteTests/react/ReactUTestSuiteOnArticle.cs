using Entities.EmmBlog.DataModelObjects;
using Entities.EmmBlog.DataModelObjects.dependentInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utilities;
using System.Collections.Generic;
using static Repository.Exceptions.ArticlesExceptions;
using static Repository.Exceptions.CommentsExceptions;

namespace UnitTestProject.Tests.ConcreteTests
{
    public partial class ReactUTest : UnitTest
    {
        [TestMethod]
        public void AnAccountCanReactOnArticle()
        {
            Account account = Manu;
            IHaveReactions reactible = ArticleZero;
            React reacted = Wrapper.React.React(NullNormalized<React>(r =>
            {
                r.Article = ArticleZero;
                r.Account = Manu;
                r.Reaction = new ReactionType { TypeId = CONFUSED };
            }));
        }

        [TestMethod]
        public void AModifiedArticleReflectItsReactionsOnTheMostUpToDateVersion()
        {
            // creation of the article
            Article created = CreateArticle(JeanLucEmmanuel, a =>
            {
                a.Content = "Dot Net Core 7 is not a thing yet";
                a.Title = "A eigth Article, Yeep yeep !!!";
                a.Slug = "Eight_Article";
                a.VisibilityId = PUBLIC;
            });

            React reacted = Wrapper.React.React(NullNormalized<React>(r =>
            {
                r.Article = created;
                r.Account = Manu;
                r.Reaction = new ReactionType { TypeId = HAHA };
            }));

            Article updated = ChangeEntity(created, a =>
             {
                 a.Title = "Does Dot Net Core will land before 2030?";
                 a.Content = "Well, nobody can tell yet but Dot Net Core" +
                 " will certainly evolve to higher levels sucha as a eigth " +
                 "version of it";
             });

            Assert.IsNotNull(updated.Reactions);
        }

        [TestMethod]
        public void ACreatedAfterModifiedArticleReactionPointsOnOriginal()
        {
            Article article = null;
            Comment commented = CreateCommentedArticleByManu(out article, a =>
            {
                a.Content = "Who do loves pizza?";
                a.Title = "Being well within yourself is important 2";
                a.Slug = "Twelveth_Article";
            }, "I do actually");

            Article upgraded = null;
            foreach (string content in new string[] {
                "Who do loves good life?",
                "Who do loves pizza and good life?",
            })
            {
                Article copy = Util.GetCopyOf(article);
                copy.Content = content;

                upgraded = Wrapper.Article.UpdateArticle(copy);
            }

            React reacted = Wrapper.React.React(NullNormalized<React>(r =>
            {
                r.Article = upgraded;
                r.Account = Manu;
                r.Reaction = new ReactionType { TypeId = LIKE };
            }));

            Assert.AreEqual(reacted.VDepth, 0);
        }

        [TestMethod, ExpectedException(typeof(NoCommentFoundException))]
        public void RemoveACommentRemovesItsReactions()
        {
            Article article = null;
            Comment commented = CreateCommentedArticleByManu(out article, a =>
            {
                a.Content = "...";
                a.Title = "Pets are soft";
            }, "Say something");

            React reacted = Wrapper.React.React(NullNormalized<React>(r =>
            {
                r.Comment = commented;
                r.Account = Manu;
                r.Reaction = new ReactionType { TypeId = LOVE };
            }));

            Comment copy = Wrapper.Comment.RemoveComment(commented);
            React removedReaction = Wrapper.React.GetReact(commented, Manu);

            Comment removed = GetCommentLike(copy);

            Assert.IsNull(removedReaction);
        }

        [TestMethod , ExpectedException(typeof(NoArticleFoundException))]
        public void RemoveAnArticleRemovesItsReactions()
        {
            Blog blog = Wrapper.Blog.GetBlogOfAccount(JeanLucEmmanuel.Id);

            Article article = CreateArticle(JeanLucEmmanuel, a =>
            {
                a.Content = "I wonder now if something worser than Covid19 exists";
                a.Title = "Is it realy the end of fear?";
                a.Blog = blog;
            });

            React reacted = Wrapper.React.React(NullNormalized<React>(r =>
            {
                r.Article = article;
                r.Account = Manu;
                r.Reaction = new ReactionType { TypeId = CONFUSED };
            }));

            Article copy = Wrapper.Article.RemoveArticle(article);

            React removedReaction = Wrapper.React.GetReact(article, Manu);

            Assert.IsNull(removedReaction);
            
            // should throw
            GetArticleLike(copy);
        }

        [TestMethod]
        public void RemoveACommentedArticleRemovesItsReactions()
        {
            Blog blog = Wrapper.Blog.GetBlogOfAccount(JeanLucEmmanuel.Id);

            Article article = CreateArticle(JeanLucEmmanuel, a =>
            {
                a.Content = "I wonder now what the futur holds for poor humanity";
                a.Title = "The covid pandemic is close to end";
                a.Blog = blog;
            });

            React reacted = Wrapper.React.React(NullNormalized<React>(r =>
            {
                r.Article = article;
                r.Account = Manu;
                r.Reaction = new ReactionType { TypeId = CONFUSED };
            }));

            Comment commented = Wrapper.Comment
                .CommentArticle(NullNormalized<Comment>(c =>
                {
                    c.Content = "Man, just enjoy your lif already!";
                    c.AccountId = Manu.Id;
                    c.Article = article;
                }
            ));

            Article removedArticle = Wrapper.Article.RemoveArticle(article);
            React removedReaction = Wrapper.React.GetReact(article, Manu);

            Assert.IsNotNull(removedArticle);
            Assert.IsNull(removedReaction);
        }

        [TestMethod]
        public void OnlyATop3OfReactionsCanBeGetFromArticle()
        {
            Article article = null;
            Comment commented = CreateCommentedArticleByManu(out article, a =>
            {
                a.Content = "Who do loves pizza?";
                a.Title = "Being well within yourself is important";
                a.VDepth = 0;
            }, "I do actually");

            foreach ((string[], int) attribution
                in GetAttributions())
            {
                int reactionId = attribution.Item2;
                string[] mailAddresses = attribution.Item1;

                AssignReactionsToArticle(reactionId, mailAddresses, article);
            };

            ICollection<ReactionCounter> resulted = GetArticleLike(article)
                .ReactionCounts;

            Assert.AreEqual(resulted.Count, 3);
        }
    }
}