using Contracts;
using Entities;
using Entities.EmmBlog.DataModelObjects.dependentInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class, new()
    {
        protected RepositoryContext RepositoryContext { get; set; }
        public IRepositoryWrapper Wrapper { get; }

        public RepositoryBase(IRepositoryWrapper wrapper, RepositoryContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
            Wrapper = wrapper;
        }

        public IQueryable<T> FindAll()
        {
            return RepositoryContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return RepositoryContext.Set<T>().Where(expression);//.AsNoTracking();
        }

        public void Create(T entity)
        {
            RepositoryContext.Set<T>().Add(entity);
        }

        public void Create<TDest>(TDest entity) where TDest : class
        {
            RepositoryContext.Set<TDest>().Add(entity);
        }

        public void Update(T entity)
        {
            RepositoryContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            RepositoryContext.Set<T>().Remove(entity);
        }

        protected EntityEntry<TEntity> With<TEntity>(TEntity entry) where TEntity : class
        {
            return RepositoryContext.Entry(entry);
        }

        public void RollBack()
        {
            var context = RepositoryContext;
            var changedEntries = context.ChangeTracker.Entries();
            
            var list = changedEntries
                .Where(x => x.State != EntityState.Unchanged)
                .ToList();

            foreach (var entry in list)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;

                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }

        protected void TrySave()
        {
            RepositoryContext.SaveChanges();   
        }
    }
}