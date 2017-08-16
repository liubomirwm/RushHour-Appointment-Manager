using DataAccess;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private RushHourContext context = new RushHourContext();

        public BaseRepository(RushHourContext context)
        {
            this.context = context;
        }

        public virtual void Add(T entity)
        {
            context.Set<T>().Add(entity);
        }

        public virtual void Delete(T entity)
        {
            context.Entry(entity).State = EntityState.Deleted;
        }

        public virtual void Edit(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }

        public virtual IEnumerable<T> GetAll(Func<T, bool> filter = null)
        {
            DbSet<T> set = context.Set<T>();
            if (filter != null)
            {
                IEnumerable<T> filteredItems = new List<T>();
                filteredItems = set.Where(filter);
                return filteredItems;
            }
            else
            {
                return set.ToList();
            }
        }

        public virtual T GetById(int id)
        {
            return context.Set<T>().Find(id);
        }

        public virtual bool IsExistingEntity(Func<T, bool> filter)
        {
            bool isExistingEntity = context.Set<T>().Any(filter);
            return isExistingEntity;
        }
    }
}
