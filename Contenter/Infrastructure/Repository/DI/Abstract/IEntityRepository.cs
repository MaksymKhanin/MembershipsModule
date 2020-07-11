using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Contenter.Infrastructure.Repository.DI.Abstract
{
    public interface IEntityRepository<T> : IDisposable
         where T : class
    {
        IQueryable<T> GetItems();
        Task<T> GetItemAsync(int id);
        IQueryable<T> GetItems(int id);
        IQueryable<T> Get(Func<T, bool> predicate);
        IQueryable<T> GetWithInclude(params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> GetWithInclude(Func<T, bool> predicate, params Expression<Func<T, object>>[] includeProperties);
        void Create(T item);
        void Update(T item);
        Task DeleteAsync(int id);
        Task SaveAsync();

    }
}
