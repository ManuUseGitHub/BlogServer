using Entities.EmmBlog.DataModelObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utilities;
using Repository.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnitTestProject.CustomTypes;
using static Contracts.IRecordKind;
using static Repository.Exceptions.ArticlesExceptions;

namespace UnitTestProject.Tests.ConcreteTests
{
    [TestClass]
    public class ArticleUTest : UnitTest
    {
        private static string[][] contentUpdates = new string[][] {
            new string[]{
                "Hey I hate people who are sceptic !",
                "Hey I hate people who are not open to different points of view !",
                "Listen guie, I kinda feel bad because of people who are sceptics...",
                "Dear friends, I do not feel good mately please excuse my mood, I am sorry",
                "Dear friends, I haven't feel that great lately, because of lot of things ... :'( please excuse my mood, I am sorry. I love you even if you are sceptic some times"
            },
            new string[]{
                "Bonsoir",
                "Eug bonjour, sorry >.<!",
                "test"
            }
        };

        [TestMethod]
        public void CreateAnAccountWithNoSlugGivesTheArticleASlugBasedOnItsTitle()
        {
            Blog blog = BlogOfAccount(JeanLucEmmanuel);
            Blog blog2 = Wrapper.Blog.GetBlogOfAccount(Manu.Id);
            Article article = NullNormalized<Article>(a =>
            {
                a.Content = "Great Content";
                a.Title = "Ceçi peut - être créé sans slug !";
                a.Blog = blog;
            });

            // creation of thee article
            article = Wrapper.Article.CreateArticle(article);
            Assert.AreEqual("ceci-peut-etre-cree-sans-slug".ToUpper(), article.Slug);
        }
        [TestMethod]
        public void AnAccountCanShareAnArticleOnItsBlog()
        {
            Sharable sharable = new Sharable(
                // From
                "jean.luc.e.verhan@gmail.com", 
                
                // TO
                "manmdyalw@gmail.com",
                
                // Article to create then share
                NullNormalized<Article>(a =>
                {
                    a.Slug = "Article_After_Life";
                })
            );

            // Share
            Share shared = Share(sharable, sharable.BlogDestin);

            // Check
            Assert.AreNotEqual(sharable.BlogSource.Id, shared.Blog.Id);
        }

        [TestMethod, ExpectedException(typeof(TwiceAddingArticleException))]
        public void AnArticleCannotBeAddTwice()
        {
            Sharable sharable = new Sharable(
                // From
                "jean.luc.e.verhan@gmail.com",

                // TO
                "manmdyalw@gmail.com",

                // Article to create then share
                NullNormalized<Article>(a =>
                {
                    a.Title = "should not be add twice article";
                })
            );

            Wrapper.Article.CreateArticle(sharable);

            Wrapper.Article.CreateArticle(sharable);
        }

        [TestMethod]
        public void ASharedArticleCreatesANewArticleReferingToTheOriginal()
        {
            Sharable sharable = new Sharable(
                // From
                "jean.luc.e.verhan@gmail.com", 
                
                // To
                "manmdyalw@gmail.com",

                // Article to create then share
                NullNormalized(a =>
                {
                    a.Slug = "Article_Zero";
                })
            );

            // Share
            Article new_ = GetArticleLike(sharable);
            sharable.MergeWith(new_);

            // count before the operation
            int count1 = GetBlogArticlesCount(sharable.BlogDestin);
            
            // Operate
            Share shared = Wrapper.Share.ShareArticle(sharable, sharable.BlogDestin);
            
            // Count after the operation
            int count2 = GetBlogArticlesCount(sharable.BlogDestin);

            Assert.AreNotEqual(shared.BlogId, shared.SharingBlogId);
            Assert.AreNotEqual(count1, count2);
        }

        [TestMethod]
        public void ASharedArticleStaysUptoDate()
        {
            Sharable sharable = new Sharable(
                // From
                "jean.luc.e.verhan@gmail.com",

                // To
                "manmdyalw@gmail.com",

                // Article to create then share
                NullNormalized(a =>
                {
                    a.Title = "A Third Article, yupiie !!!";
                    a.Slug = "Third_Article";
                })
            );

            sharable.MergeWith(Wrapper.Article.CreateArticle(sharable));

            Share shared = Share(sharable, sharable.BlogDestin);

            // loop to create modifications over the third article
            LoopModify(sharable, contentUpdates[1]);

            Share resultedShare = Wrapper.Share.getShare(shared);
        }

        [TestMethod]
        public void AModifiedArticleCreatesANewVersion()
        {
            Blog blog = BlogOfAccount(JeanLucEmmanuel);

            // count before the operation
            int count1 = GetBlogArticlesCount(blog);

            // getting the "first" slugged article
            Article fetched = getAFirstArticle(blog.Id);

            // get a modifiable instance of it
            Article modifiable = Util
                .GetModifiable(fetched, a =>
                {
                    a.Content = "This is new!";
                });

            // operate
            Article modified = UpdateArticle(modifiable);

            // Count after the operation
            int count2 = GetBlogArticlesCount(blog);

            Assert.AreNotEqual(count1, count2);
            Assert.AreNotEqual(fetched.Content, modified.Content);
        }

        [TestMethod]
        public void AModificationThreadCanBeGetFromVersions()
        {
            Blog blog = BlogOfAccount(JeanLucEmmanuel);
            Article article = GetArticleLike(getAFirstArticle(blog.Id));

            // apply modification of Content property with a loop
            LoopModify(article, contentUpdates[0]);

            Article search = GetArticleLike(Util.GetCopyOf(article));

            List<Article> thread = GetThreadOfArticle(search);

            Assert.AreEqual(thread.Count, 6);
        }

        [TestMethod]
        public void ADeletedCommentedArticleTurnsItsVisibilitiesToRemoved()
        {
            Blog blog = Wrapper.Blog.GetBlogOfAccount(JeanLucEmmanuel.Id);
            Article article = NullNormalized<Article>(a =>
            {
                a.Content = "I just want to know what you think about react";
                a.Title = "Your thoughs about React";
                a.Slug = "React_Article";
                a.Blog = blog;
            });

            // creation of thee article
            Article published = Wrapper.Article.CreateArticle(article);

            Comment commented = Wrapper.Comment
                .CommentArticle(NullNormalized<Comment>(c =>
                {
                    c.Content = "React ?! What a pointless article, remove it";
                    c.AccountId = Manu.Id;
                    c.Article = article;
                }
            ));

            Article removed = Wrapper.Article.RemoveArticle(published);
        }

        [TestMethod]
        public void ADeletedSharedArticleTurnsItsVisibilitiesToRemoved()
        {
            Blog blog = BlogOfAccount(JeanLucEmmanuel);
            Blog blog2 = Wrapper.Blog.GetBlogOfAccount(Manu.Id);
            Article article = NullNormalized<Article>(a =>
            {
                a.Content = "Great Content";
                a.Title = "Something";
                a.Slug = "Delete_Shared_Modified_Article";
                a.Blog = blog;
            });

            // creation of thee article
            article = Wrapper.Article.CreateArticle(article);
            Share shared = Wrapper.Share.ShareArticle(article, blog2);

            LoopModify(article, new string[] {
                "After life 1",
                "After life 2"
            });

            Article removed = Wrapper.Article.RemoveArticle(article);
            Assert.IsNotNull(removed);
            Assert.IsNotNull(removed.VisibilityId == 99);

        }

        [TestMethod, ExpectedException(typeof(NoArticleFoundException))]
        public void ADeletedSharingArticleIsRemoved()
        {
            Blog blog = BlogOfAccount(JeanLucEmmanuel);
            Blog blog2 = Wrapper.Blog.GetBlogOfAccount(Manu.Id);
            Article article = NullNormalized<Article>(a =>
            {
                a.Content = "Great Content";
                a.Title = "Something";
                a.Slug = "Delete_Shared_Article";
                a.Blog = blog;
            });

            // creation of thee article
            article = Wrapper.Article.CreateArticle(article);
            Share shared = Wrapper.Share.ShareArticle(article, blog2);

            Article removed = Wrapper.Article.RemoveArticle(shared);

            // should throw
            GetArticleLike(removed);
        }

        [TestMethod]
        public void ADeletedCommentedModifiedArticleTurnsVisibilitiesOfItsVersionsToRemoved()
        {
            Blog blog = Wrapper.Blog.GetBlogOfAccount(JeanLucEmmanuel.Id);
            Article article = NullNormalized<Article>(a =>
            {
                a.Content = "I think it's better to have vue with a cli...";
                a.Title = "Your thoughs about Vue";
                a.Slug = "Vue_Article";
                a.Blog = blog;
            });

            // creation of thee article
            Article published = Wrapper.Article.CreateArticle(article);

            foreach (string content in new string[] {
                "Vue has been rolling over the last years. It is now something to consider",
                "Vue is a library that has a lot of success now, it deserve more highlight"
            })
            {
                Article copy = Util.GetCopyOf(article);
                copy.Content = content;

                Wrapper.Article.UpdateArticle(copy);
            }

            Comment commented = Wrapper.Comment
                .CommentArticle(NullNormalized<Comment>(c =>
                {
                    c.Content = "I am of this point of view myself since months";
                    c.AccountId = Manu.Id;
                    c.Article = article;
                }
            ));

            Article removed = Wrapper.Article.RemoveArticle(published);
        }

        [TestMethod]
        public void AModifiedCommentIsAllwaysReferenced()
        {
            Blog blog = Wrapper.Blog.GetBlogOfAccount(JeanLucEmmanuel.Id);
            Article article = NullNormalized<Article>(a =>
            {
                a.Content = "Dot Net Core 4 is next to come";
                a.Title = "A seventh Article, Yeee !!!";
                a.Slug = "Seventh_Article";
                a.Blog = blog;
                a.VisibilityId = 1;
            });

            // creation of the article
            Article created = Wrapper.Article.CreateArticle(article);

            Comment comment = NullNormalized<Comment>(c =>
            {
                c.Content = "First!";
                c.AccountId = Manu.Id;
                c.Article = article;
            });

            Comment comment2 = NullNormalized<Comment>(c =>
            {
                c.Content = "Now I will subscribe !";
                c.AccountId = Manu.Id;
                c.Article = article;
            });

            Comment commented = Wrapper.Comment.CommentArticle(comment);
            Comment commented2 = Wrapper.Comment.CommentArticle(comment2);

            Comment modified = ChainModify(new string[] {
                "second",
                "I am looking forward!"
            }, commented);

            Article fetched = Wrapper.Article.GetEndArticle(created);
            int? revision = fetched.Comments.First().RDepth;

            Assert.AreEqual(2, fetched.Comments.Count);
            Assert.AreNotEqual(0, revision);
        }

        private Share Share(Article sharable, Blog blog2)
        {
            return Wrapper.Share.ShareArticle(sharable, blog2);
        }

        private List<Article> GetThreadOfArticle(Article article)
        {
            return Wrapper.Article.GetThreadOfArticle(article);
        }
        private Article UpdateArticle(Article modifiable)
        {
            return Wrapper.Article.UpdateArticle(modifiable);
        }

        private int GetBlogArticlesCount(Blog blog)
        {
            return Wrapper.Blog.GetBlogArticles(blog.Id).Articles.Count;
        }

        private Blog BlogOfAccount(Account acc)
        {
            return Wrapper.Blog.GetBlogOfAccount(acc.Id);
        }
        public static Article getAFirstArticle(Guid? blogId)
        {
            return new NullNormalizeFactory<Article>(a =>
            {
                a.Title = "A random title";
                a.Content = "What an interressant content !!!";
                a.Slug = "Article_Zero";
                a.BlogId = blogId;
            }).Instance;
        }

        public static Article getAFirstArticle(Blog blog)
        {
            Article result = getAFirstArticle(blog.Id);

            result.Blog = blog;
            return result;
        }

        private Article NullNormalized(Action<Article> initializer)
        {
            return NullNormalized<Article>(initializer);
        }


        private void LoopModify(Article article, ICollection<string> contents)
        {
            int cpt = 1;
            foreach (string content in contents)
            {
                Article changed = Util.GetModifiable(
                    Wrapper.Article.GetArticle(article), a =>
                    {
                        a.Content = content;
                        a.WriteDate = DateTime.Now.AddMinutes(0+ ++cpt * 5);
                    });

                Wrapper.Article.UpdateArticle(changed);
            }
        }
    }
}