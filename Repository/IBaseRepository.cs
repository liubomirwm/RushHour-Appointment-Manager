using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    interface IBaseRepository<T>
    {
        IEnumerable<T> GetAll(Func<T, bool> filter = null);
        T GetById(int id);
        void Add(T entity);
        void Edit(T entity);
        void Delete(T entity);

    }
}
