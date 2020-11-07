using BeanlancerAPI2.Models;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BeanlancerAPI.Services
{
    public class BaseService<TEntity, TKey> : IBaseService<TEntity, TKey> where TEntity : class
    {
        BeanlancersContext _context;
        DbSet<TEntity> _dbset;


        
        public BaseService(BeanlancersContext context)
        {
            this._context = _context ?? context;
            this._dbset = _context.Set<TEntity>();
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public TEntity Create(TEntity entity)
        {
             _dbset.Add(entity);
            return entity;
        }

        public TEntity DeleteByEntity(TEntity entity)
        {
            _dbset.Remove(entity);
            return entity;
        }

        public TEntity DeleteById(TKey id)
        {
            var entity = GetById(id);
            _dbset.Remove(entity);
            return entity;
        }

        public IEnumerable<TEntity> GetAll(int numpage, int pagesize, Expression<Func<TEntity, bool>> expression = null)
        {
            if(expression == null)
            {
                // return _dbset.Skip((numpage - 1) * pagesize).Take(pagesize);
                return _dbset.ToPagedList(numpage, pagesize);
            }
            // return _dbset.Where(expression).Skip((numpage - 1) * pagesize).Take(pagesize);
            return _dbset.Where(expression).ToPagedList(numpage, pagesize);
        }

        public IEnumerable<TEntity> GetAllNonPaging(Expression<Func<TEntity, bool>> expression = null)
        {
            if(expression == null) return _dbset;           
            return _dbset.Where(expression);
            
        }

        public TEntity GetByExpress(Expression<Func<TEntity, bool>> expression)
        {
            if(expression == null) return null;
            return _dbset.Where(expression).FirstOrDefault(); 
        }

        public TEntity GetById(TKey id)
        {
            return _dbset.Find(id);
        }

        public TEntity Update(TKey id, TEntity entity)
        {
            var entry = _dbset.Find(id);
            _context.Entry(entry).CurrentValues.SetValues(entity);
            return entity;
        }

       
    }
}
