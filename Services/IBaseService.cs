using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BeanlancerAPI.Services
{
    public interface IBaseService<TEntity, TKey> where TEntity : class
    {

        void Commit();
        TEntity Create(TEntity entity);
        TEntity Update(TKey id, TEntity entity);
        
        TEntity DeleteById(TKey id);
        TEntity DeleteByEntity(TEntity entity);
        TEntity GetById(TKey id);
        IEnumerable<TEntity> GetAll(int numpage, int pagesize, Expression<Func<TEntity, bool>> expression);
        IEnumerable<TEntity> GetAllNonPaging(Expression<Func<TEntity, bool>> expression);

        TEntity GetByExpress(Expression<Func<TEntity, bool>> expression);
    }
}