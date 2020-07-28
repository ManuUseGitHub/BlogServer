using Entities.EmmBlog.DataModelObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static Entities.EmmBlog.DataModelObjects.ReactionType;
using static Utilities.Reflector;

namespace UnitTestProject.Tests.ConcreteTests
{
    [TestClass]
    public partial class ReactUTest : UnitTest
    {
        private T ChangeEntity<T>(T entity, Action<T> c) where T : class, new()
        {
            T modified = NullNormalized(c);
            MergeOptions keepTarget = MergeOptions.KEEP_TARGET;

            if (entity is Comment)
            {
                (modified as Comment).RDepth = null;
            }
            else if (entity is Article)
            {
                (modified as Article).VDepth = null;
            }

            Utilities.Reflector.Merge(modified, entity, keepTarget);

            object result = null;
            if (entity is Comment)
                result = Wrapper.Comment.ChangeComment(modified as Comment);
            else if (entity is Article)
                result = Wrapper.Article.UpdateArticle(modified as Article);

            return (T)result;
        }

        public static Article CreateArticle(Account account, Action<Article> a)
        {
            Blog blog = Wrapper.Blog.GetBlogOfAccount(account.Id);
            Article article = NullNormalized(a);
            article.Blog = blog;

            // creation of the article
            return Wrapper.Article.CreateArticle(article);
        }

        public Comment CreateCommentedArticleByManu(
            out Article article, Action<Article> a, string text)
        {
            Article created = CreateArticle(JeanLucEmmanuel, a);

            Comment result = Wrapper.Comment.CommentArticle(
                NullNormalized<Comment>(c =>
                {
                    c.Content = text;
                    c.AccountId = Manu.Id;
                    c.Article = created;
                })
            );

            article = Wrapper.Article.GetArticle(created);

            return result;
        }

        public Comment CreateCommentedArticleByManu(Action<Article> a, string text)
        {
            return CreateCommentedArticleByManu(out Article article, a, text);
        }
        private void AssignReactionsToArticle(
            int reactionId, string[] mailAddresses, Article article
        )
        {
            foreach (string mail in mailAddresses)
            {
                Account account = Wrapper.Account
                .GetAccountByMailAddress(mail);

                Wrapper.React.React(NullNormalized<React>(r =>
                {
                    r.Article = article;
                    r.Account = account;
                    r.Reaction = new ReactionType { TypeId = reactionId };
                }));
            }
        }
        private void AssignReactionsToComment(
            int reactionId, string[] mailAddresses, Comment comment
        )
        {
            foreach (string mail in mailAddresses)
            {
                Account account = Wrapper.Account
                .GetAccountByMailAddress(mail);

                Wrapper.React.React(NullNormalized<React>(r =>
                {
                    r.Comment = comment;
                    r.Account = account;
                    r.Reaction = new ReactionType { TypeId = reactionId };
                }));
            }
        }

        private (string[], int)[] GetAttributions()
        {
            return new (string[], int)[]
            {
                (new string[]{
                    "j.doe@gmail.com","j.fehtou@gmail.com",
                    "t.ato@gmail.com", "m.kojiro@gmail.com",
                    "g.Gomez@gmail.com", "j.Gustavo@gmail.com"
                }, LIKE),

                (new string[]{
                    "m.ado@gmail.com", "c.salad@gmail.com",
                    "r.rogue@gmail.com", "l.rheto@gmail.com",
                    "m.oeth@gmail.com", "s.stiim@gmail.com",
                    "r.beaf@gmail.com", "j.gnack@gmail.com",
                    "j.bonvin@gmail.com",
                }, HAHA),

                (new string[]{
                    "e.ditz@gmail.com", "j.fehtou@gmail.com"
                }, ANGRY),

                (new string[]{
                    "l.morning-stark@gmail.com", "c.doe@gmail.com",
                    "a.bonaventure@gmail.com"
                }, SURPRISED),
            };
        }
    }
}