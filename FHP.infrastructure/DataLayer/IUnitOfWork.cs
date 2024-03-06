using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.infrastructure.DataLayer
{
    public interface IUnitOfWork
    {
        void BeginTransaction();

        Task<IDbContextTransaction> BeginTransactionAsync();

        int SaveChanges();

        Task<int> SaveChangesAsync();

        void Commit();

        Task CommitTransactionAsync();

        void Rollback();

        Task RollbackTransactionAsync();

        Task<int> ExecuteSqlCommandAsync(string sqlCommand, params object[] parameters);


    }
}
