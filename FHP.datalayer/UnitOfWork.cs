using FHP.infrastructure.DataLayer;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace FHP.datalayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dataContext;
        private IDbContextTransaction _dbTransaction;

        public UnitOfWork(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void BeginTransaction()
        {
            _dbTransaction = _dataContext.Database.BeginTransaction();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            _dbTransaction = await _dataContext.Database.BeginTransactionAsync();
            return _dbTransaction;
        }

        public int SaveChanges()
        {
            return _dataContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dataContext.SaveChangesAsync();
        }

        public void Commit()
        {
            _dbTransaction.Commit();
        }
        public async Task CommitTransactionAsync()
        {
            await _dataContext.Database.CommitTransactionAsync();
        }
        public void Rollback()
        {
            _dbTransaction.Rollback();
        }

        public async Task RollbackTransactionAsync()
        {
            await _dataContext.Database.RollbackTransactionAsync();
        }

        public async Task<int> ExecuteSqlCommandAsync(string sqlCommand, params object[] parameters)
        {
            return await _dataContext.Database.ExecuteSqlRawAsync(sqlCommand, parameters);
        }
    }
}
