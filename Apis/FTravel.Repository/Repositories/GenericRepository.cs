using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.DBContext;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Repository.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly FtravelContext _dbContext;

        public GenericRepository(FtravelContext context)
        {
            _dbSet = context.Set<TEntity>();
            _dbContext = context;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            entity.CreateDate = GetCurrentTime();
            _dbSet.Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task AddRangeAsync(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.CreateDate = GetCurrentTime();
            }
            _dbSet.AddRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            var result = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task<int> SoftDeleteAsync(TEntity entity)
        {
            entity.IsDeleted = true;
            _dbSet.Update(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> SoftDeleteRangeAsync(List<TEntity> entities)
        {
            foreach(var entity in entities)
            {
                entity.IsDeleted = true;
            }
            _dbSet.UpdateRange(entities);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(TEntity entity)
        {
            entity.UpdateDate = GetCurrentTime();
            _dbSet.Update(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateRangeAsync(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.UpdateDate = GetCurrentTime();
            }
            _dbSet.UpdateRange(entities);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<Pagination<TEntity>> ToPagination(PaginationParameter paginationParameter)
        {
            var itemCount = await _dbSet.CountAsync();
            var items = await _dbSet.Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                    .Take(paginationParameter.PageSize)
                                    .AsNoTracking()
                                    .ToListAsync();
            var result = new Pagination<TEntity>(items, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);

            return result;
        }
        public async Task<int> PermanentDeletedAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            return await _dbContext.SaveChangesAsync();
        }

        private DateTime GetCurrentTime() 
        { 
            DateTime utcNow = DateTime.UtcNow;

            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            DateTime vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, vietnamTimeZone);

            return vietnamTime;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _dbContext.Database.BeginTransactionAsync();
        }
    }
}
