using AutoMapper;
using Entities.EmmBlog.DataModelObjects;
using Entities.EmmBlog.DataTransferObjects;
using Entities.EmmBlog.DataTransferObjects.In;
using Entities.EmmBlog.DataTransferObjects.Out.Reduced;
using System.Collections.Generic;
using static Entities.EmmBlog.DataTransferObjects.Out.Reduced.ArticlesOfBlogDTO;

namespace EmmBlog
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Article, ArticleRevisionDTO>();
            CreateMap<Article, ArticlesOfBlogDTO>();
            

            CreateMap<Comment, BlogWithArticlesDTO.CommentOfArticleDTO>();
            CreateMap<Comment, BlogWithArticlesDTO.AnswereOfCommentDTO>();
        
            // get
            CreateMap<Account, AccountDTO>();
            CreateMap<Account, ProfileAccountDTO>();
            CreateMap<Account, CredentialDTO.AccountOfCredentialDTO>();
            CreateMap<Account, SubscriptionOfBlogDTO.AccountOfSubscriptionDTO>();
            
            //set
            CreateMap<AccountForCreationDTO, Account>();

            CreateMap<Blog, BlogWithSubscription>();
            CreateMap<Blog, BlogWithArticlesDTO>();
            CreateMap<Blog, ListedBlogDTO>();

            CreateMap<Credential, CredentialDTO>();

            CreateMap<Subscribe, SubscriptionOfBlogDTO>();

            CreateMap<React, ReactionDTO>();
        }
    }
}
