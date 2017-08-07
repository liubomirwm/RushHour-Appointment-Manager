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
            List<T> items = context.Set<T>().ToList();
            if (filter != null)
            {
                IEnumerable<T> filteredItems = new List<T>();
                filteredItems = items.Where(filter);
                return filteredItems;
            }
            else
            {
                return items;
            }
        }

        public virtual T GetById(int id)
        {
            return context.Set<T>().Find(id);
        }
    }
}
