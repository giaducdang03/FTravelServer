using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Repository.Repositories.Interface
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(int id);
        Task<TEntity> AddAsync(TEntity entity);
        Task<int> UpdateAsync(TEntity entity);
        Task<int> SoftDeleteAsync(TEntity entity);
        Task AddRangeAsync(List<TEntity> entities);
        Task<int> UpdateRangeAsync(List<TEntity> entities);
        Task<int> SoftDeleteRangeAsync(List<TEntity> entities);

        Task<Pagination<TEntity>> ToPagination(PaginationParameter paginationParameter);
    }
}
