using Entities.EmmBlog.DataModelObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace UnitTestProject.Tests.ConcreteTests
{
    public partial class ReactUTest : UnitTest
    {
        [TestMethod]
        public void AReactionCanBeRemoved()
        {
            Article article = null;
            Comment commented = CreateCommentedArticleByManu(out article, a =>
             {
                 a.Content = "Who do loves pizza?";
                 a.Title = "Being well within yourself is important";
                 a.Slug = "Tenth_Article";
                 a.VDepth = 0;
             }, "I do actually");

            React reacted = Wrapper.React.React(NullNormalized<React>(r =>
            {
                r.Comment = commented;
                r.Account = Manu;
                r.Reaction = new ReactionType { TypeId = LIKE };
            }));

            React reacted2 = Wrapper.React.React(NullNormalized<React>(r =>
            {
                r.Article = article;
                r.Account = Manu;
                r.Reaction = new ReactionType { TypeId = LIKE };
            }));

            Comment updated = commented;
            string[] modifs = new string[]
            {
                "Welle who do not love pizza her",
                "Well, I chalenge you to find someone who do not love pizza here :p ."
            };

            foreach (string modif in modifs)
            {
                updated = ChangeEntity(updated, c => { c.Content = modif; });
            }
            Account account = Manu;
            React unlinked = Wrapper.React.RemoveReaction(commented, account);
        }
    }
}