using Store.Data.Entities;
using Store.Repository.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Interfaces
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        Task<TEntity> GetByIdAsync(TKey? id);
        Task<IReadOnlyList<TEntity>> GetAllAsync();
        //Task<TEntity> GetByIdAsNoTrackingAsync(TKey? id);
         
        Task<TEntity> GetWithSpecificationsByIdAsync(ISpecifications<TEntity> specs);
        Task<IReadOnlyList<TEntity>> GetAllWithSpecificationsAsync(ISpecifications<TEntity> specs);
        Task<int> GetCountSpecificationsAsync(ISpecifications<TEntity> specs);

        Task<IReadOnlyList<TEntity>> GetAllAsNoTrackingAsync();
        Task AddAsync(TEntity entity);
        void UpdateAsync(TEntity entity);
        void DeleteAsync(TEntity entity);
    }
}
 