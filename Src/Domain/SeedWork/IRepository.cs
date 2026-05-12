using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coreapi.Domain.SeedWork
{
    public interface IRepository<T>
    {
        T Add(T entity);
        void Update(T entity);
        Task<T> GetById(Guid id);
        Task<T> DeleteById(Guid id);
        Task<bool> IsExist(Guid id);
        Task<IEnumerable<T>> GetAll();
        Task<long> GetCount();
        IUnitOfWork UnitOfWork { get; }

        
    }
}
