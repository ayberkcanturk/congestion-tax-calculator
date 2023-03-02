using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volvo.CongestionTax.Data.Core.Repositories;

namespace Volvo.CongestionTax.Data.EFCore.Repository
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly CongestionTaxDbContext _congestionTaxDbContext;

        public GenericRepository(CongestionTaxDbContext congestionTaxDbContext)
        {
            _congestionTaxDbContext = congestionTaxDbContext;
        }

        public async Task<ICollection<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter,
            CancellationToken cancellationToken = default)
        {
            return await _congestionTaxDbContext.Set<TEntity>().Where(filter).ToListAsync(cancellationToken);
        }

        public async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filter,
            CancellationToken cancellationToken = default)
        {
            return await _congestionTaxDbContext.Set<TEntity>().Where(filter).FirstOrDefaultAsync(cancellationToken);
        }
    }
}