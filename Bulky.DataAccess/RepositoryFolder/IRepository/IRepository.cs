using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.RepositoryFolder.IRepository
{
    public interface IRepository<T> where T : class
    {
        //T = Category
        IEnumerable<T> GetAll();
        //below is the expression of link operation u=>u.Id==id but in generic form so called link expression
        T Get(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
