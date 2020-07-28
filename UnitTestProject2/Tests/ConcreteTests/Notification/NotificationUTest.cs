using Entities.EmmBlog.DataModelObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using UnitTestProject.CustomTypes;
using Utilities;

namespace UnitTestProject.Tests.ConcreteTests
{
    [TestClass]
    public class NotificationUTest : UnitTest
    {
        [TestMethod]
        public void ANewMessageGeneratesANotificationToTheReceiver()
        {
            int c1 = Wrapper.Notificaton.GetNotoficationsOf(JeanLucEmmanuel).Count;

            Message first = new Message()
            {
                Content = "Test !",
                From = Manu,
                To = JeanLucEmmanuel,
                WriteDate = DateTime.Now.AddMinutes(0)
            };

            Wrapper.Message.ExchangeMessage(first);

            int c2 = Wrapper.Notificaton.GetNotoficationsOf(JeanLucEmmanuel).Count;

            Assert.IsTrue(c1 < c2);
        }

        [TestMethod]
        public void AddAnArticleGeneratesANotificationForAllowed()
        {
            Account Cezar = GetAccountByFullName("Cezar Salad");
            Blog blog = Wrapper.Blog.GetBlogOfAccount(Cezar.Id);

            int c1 = Wrapper.Notificaton.GetNotoficationsCount();

            string[] addresses = new string[]{"l.morning-stark@gmail.com",
                "s.stiim@gmail.com",
                "a.bonaventure@gmail.com",
                "t.ato@gmail.com",
                "m.oeth@gmail.com"};

            MakeSubscriptions(blog.Id, addresses);

            // creation of thee article
            Wrapper.Article.CreateArticle(
                NullNormalized<Article>(a =>
                {
                    a.Content = "Hi peeps";
                    a.Title = "Articling like if my life depends on it";
                    a.Blog = blog;
                    a.VisibilityId = 1;
                })
            );

            int c2 = Wrapper.Notificaton.GetNotoficationsCount();
            Assert.AreEqual(c1 + addresses.Length, c2);
        }

        [TestMethod]
        public void AddAnArticleDoesNotGeneratesANotificationForNotAllowed()
        {
            Account Alex = GetAccountByFullName("Alex Bonaventure");

            Account Lucilia = GetAccountByFullName("Lucilia Morning-Stark");
            Blog blog = Wrapper.Blog.GetBlogOfAccount(Alex.Id);

            int c1 = Wrapper.Notificaton.GetNotoficationsCount();

            string[] addresses = new string[]{
                "l.morning-stark@gmail.com",
                "s.stiim@gmail.com",
                "t.ato@gmail.com",
                "m.oeth@gmail.com"
            };

            MakeSubscriptions(blog.Id, addresses);

            Wrapper.Follow.MakeABlock(new Follow()
            {
                Followed = Alex,
                Following = Lucilia,
            });

            // creation of thee article
            Wrapper.Article.CreateArticle(
                NullNormalized<Article>(a =>
                {
                    a.Content = "Hi peeps";
                    a.Title = "Articling a secret from my blocked contacts";
                    a.Blog = blog;
                    a.VisibilityId = 1;
                })
            );

            int c2 = Wrapper.Notificaton.GetNotoficationsCount();
            Assert.AreEqual(c1 + addresses.Length - 1, c2);
        }

        [TestMethod]
        public void ACommentedArticleGeneratesAnotificationToTheAuthorOfTheArticle()
        {
            int c1 = Wrapper.Notificaton.GetNotoficationsOf(JeanLucEmmanuel).Count;

            Blog blog = Wrapper.Blog.GetBlogOfAccount(JeanLucEmmanuel.Id);
            Article article = NullNormalized<Article>(a =>
            {
                a.Content = "Hi peeps";
                a.Title = "Articling like a boss";
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

            Wrapper.Comment.CommentArticle(comment);

            Assert.IsTrue(c1 < Wrapper
                .Notificaton
                .GetNotoficationsOf(JeanLucEmmanuel)
                .Count
            );
        }

        [TestMethod]
        public void AnAnsweredCommentGeneratesAnotificationToTheAuthorOfTheBaseComment()
        {
            int c1 = Wrapper.Notificaton.GetNotoficationsOf(JeanLucEmmanuel).Count;

            Blog blog = Wrapper.Blog.GetBlogOfAccount(JeanLucEmmanuel.Id);
            Article article = NullNormalized<Article>(a =>
            {
                a.Content = "Hi peeps";
                a.Title = "Articling be like";
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

            Comment answere = Wrapper.Comment.AnswerComment(
                NullNormalized<Comment>(c =>
                {
                    c.Answer = commented;
                    c.Content = "First!";
                    c.AccountId = Manu.Id;
                    c.Article = article;
                })
            );

            Comment answered = Wrapper.Comment.GetComment(answere);

            Assert.IsTrue(c1 < Wrapper
                .Notificaton
                .GetNotoficationsOf(JeanLucEmmanuel)
                .Count
            );
        }

        [TestMethod]
        public void AReactionGeneratesANotificationToTheAuthorOfTheArticle()
        {
            int c1 = Wrapper.Notificaton.GetNotoficationsOf(JeanLucEmmanuel).Count;
            Blog blog = Wrapper.Blog.GetBlogOfAccount(JeanLucEmmanuel.Id);
            Article article = NullNormalized<Article>(a =>
            {
                a.Content = "Hi peeps";
                a.Title = "Articling like a noob";
                a.Blog = blog;
                a.VisibilityId = 1;
            });

            // creation of thee article
            Article created = Wrapper.Article.CreateArticle(article);

            Wrapper.React.React(NullNormalized<React>(r =>
            {
                r.Article = created;
                r.Account = Manu;
                r.Reaction = new ReactionType { TypeId = HAHA };
            }));

            Assert.IsTrue(c1 < Wrapper
                .Notificaton
                .GetNotoficationsOf(JeanLucEmmanuel)
                .Count
            );
        }

        [TestMethod]
        public void AReactionGeneratesANotificationToTheAuthorOfTheComment()
        {
            int c1 = Wrapper.Notificaton.GetNotoficationsOf(JeanLucEmmanuel).Count;
            Blog blog = Wrapper.Blog.GetBlogOfAccount(JeanLucEmmanuel.Id);
            Article article = NullNormalized<Article>(a =>
            {
                a.Content = "Hi peeps";
                a.Title = "Articling like I dont care";
                a.Blog = blog;
                a.VisibilityId = 1;
            });

            // creation of thee article
            Article created = Wrapper.Article.CreateArticle(article);
            Comment comment = NullNormalized<Comment>(c =>
            {
                c.Content = "First!";
                c.AccountId = Manu.Id;
                c.Article = created;
            });

            Comment commented = Wrapper.Comment.CommentArticle(comment);
            Wrapper.React.React(NullNormalized<React>(r =>
            {
                r.Comment = commented;
                r.Account = Manu;
                r.Reaction = new ReactionType { TypeId = HAHA };
            }));

            Assert.IsTrue(c1 < Wrapper
                .Notificaton
                .GetNotoficationsOf(JeanLucEmmanuel)
                .Count + 1
            );
        }

        [TestMethod]
        public void AShareGeneratesANotificationToTheAuthorOfTheArticle()
        {
            int c1 = Wrapper.Notificaton.GetNotoficationsOf(JeanLucEmmanuel).Count;
            Blog blog = Wrapper.Blog.GetBlogOfAccount(JeanLucEmmanuel.Id);
            Article article =
                Wrapper.Article.CreateArticle(
                    NullNormalized<Article>(a =>
                    {
                        a.Content = "Hi peeps";
                        a.Title = "Articling very spetial";
                        a.Blog = blog;
                        a.VisibilityId = 1;
                    })
                );

            Sharable sharable = new Sharable(
               // From
               "jean.luc.e.verhan@gmail.com",

               // To
               "manmdyalw@gmail.com",

               NullNormalized<Article>(a =>
               {
                   a.Slug = article.Slug;
               })
           );

            // Share
            Article new_ = GetArticleLike(sharable);
            sharable.MergeWith(new_);

            // Operate
            Share shared = Wrapper.Share.ShareArticle(sharable, sharable.BlogDestin);

            int c2 = Wrapper.Notificaton.GetNotoficationsOf(JeanLucEmmanuel).Count;

            Assert.IsTrue(c1 + 1 == c2);
        }

        [TestMethod]
        public void ChangeTheNameOfABlogNotifiesAuthorizedSubscribers()
        {
            Account Selfe = GetAccountByFullName("Selfe Stiim");

            Account Lucilia = GetAccountByFullName("Ella Ditz");


            Blog blog = Wrapper.Blog.GetBlogOfAccount(Selfe.Id);

            int c1 = Wrapper.Notificaton.GetNotoficationsCount();

            string[] addresses = new string[]{
                "j.doe@gmail.com",
                "j.bonvin@gmail.com",
                "j.gnack@gmail.com",
                "e.ditz@gmail.com"
            };

            MakeSubscriptions(blog.Id, addresses);

            Wrapper.Follow.MakeABlock(new Follow()
            {
                Followed = Selfe,
                Following = Lucilia,
            });

            Blog modifications = NullNormalized<Blog>( b => {
                b.BlogName = "Alex fan Kid blog";
                b.AccountId = Selfe.Id;
            });

            Wrapper.Blog.ChangeBlogInfos(modifications);

            int c2 = Wrapper.Notificaton.GetNotoficationsCount();
            Assert.AreEqual(c1 + addresses.Length - 1, c2);
        }

        [TestMethod]
        public void ChangeATitleUpTo50PercentsOfThePreviousTitleNotifiesAuthorizedSubscribers()
        {
            Account Alex = GetAccountByFullName("Alex Bonaventure");

            Account Crishian = GetAccountByFullName("Crishian Doe");
            Blog blog = Wrapper.Blog.GetBlogOfAccount(Alex.Id);

            string[] addresses = new string[]{
                "a.bonaventure@gmail.com",
                "c.doe@gmail.com",
                "g.Gomez@gmail.com",
                "j.gnack@gmail.com"
            };

            MakeSubscriptions(blog.Id, addresses);

            Wrapper.Follow.MakeABlock(new Follow()
            {
                Followed = Alex,
                Following = Crishian,
            });


            Article article =
                Wrapper.Article.CreateArticle(
                    NullNormalized<Article>(a =>
                    {
                        a.Content = "Hi peeps";
                        a.Title = "Articling is fun";
                        a.Blog = blog;
                        a.VisibilityId = 1;
                    })
                );

            int c1 = Wrapper.Notificaton.GetNotoficationsCount();

            Wrapper.Article.UpdateArticle(NullNormalized<Article>(a => {
                a.Slug = article.Slug;
                a.Title = "Articling is totaly boring !";
                a.VDepth = article.VDepth;
                a.BlogId = article.BlogId;
            }));

            int c2 = Wrapper.Notificaton.GetNotoficationsCount();

            Assert.IsTrue( c2-c1 >= 3);
        }

        [TestMethod]
        public void TwoDifferentStringCanBeCompared()
        {
            Comparator comp = new Comparator();

            Assert.AreEqual(comp.SentenceCompare("", ""), 100); //100
            Assert.AreEqual(comp.SentenceCompare("", "abcd"), 0); //0

            string str1 = "ðə ɻɛd fɑks ɪz hʌŋgɻi";
            string str2 = "ðæt ɪt foks ɪn ðʌ sʌn ɻe͡i";

            Assert.AreEqual(comp.SentenceCompare(str1, str2), 35);
            Assert.AreEqual(comp.SentenceCompare("same", "same"), 100); //100

            double result = comp.SentenceCompare("Cette poubelle est de couleur chou vert", "J'aime manger du chou vert");
            Assert.AreEqual(result, 32);

            Assert.AreEqual(comp.SentenceCompare(
                "un articel qui parle des indiens",
                "Un article qui parle des natifs américains"),
                63
            );
        }

        public void MakeSubscriptions(Guid? blogId, params string[] adresses)
        {
            foreach (string address in adresses)
            {
                Wrapper.Blog.CreateSubscription(address, blogId);
            }
        }
    }
}