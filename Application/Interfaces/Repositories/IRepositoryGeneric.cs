using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IRepositoryGeneric<T> where T : class
    {
        T Add(T entity);

        void Remove(T entity);
        
        void Update(T entity);

        Task<T?> GetByIdAsync(Guid id,CancellationToken cancellationToken);
    }
}
