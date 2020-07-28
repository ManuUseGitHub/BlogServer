using Contracts;
using Entities;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using Entities.EmmBlog.DataModelObjects.dependentInterfaces;

namespace Repository.Loaders
{
    public class RepoLoaderBase<T> where T : class, new()
    {
        protected RepositoryContext RepositoryContext { get; set; }
        public IRepoLoaderWrapper RLoadr { get; }

        public RepoLoaderBase(IRepoLoaderWrapper loader, RepositoryContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
            RLoadr = loader;
        }

        public void LoadCollection<TEntity, TProperty>([NotNullAttribute] Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression, TEntity entry) where TEntity : class where TProperty : class
        {
            RepositoryContext.Entry(entry).Collection(propertyExpression).Load();
        }

        public void LoadReference<TEntity, TProperty>([NotNullAttribute] Expression<Func<TEntity, TProperty>> propertyExpression, TEntity entry) where TEntity : class where TProperty : class
        {
            RepositoryContext.Entry(entry).Reference(propertyExpression).Load();
        }
    }
}