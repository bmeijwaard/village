using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Village.Data.Domain;
using Village.Data.Identity;

namespace Village.Data
{
    public class SqlContext : IdentityDbContext<User, Role, Guid>
    {
        public SqlContext(DbContextOptions<SqlContext> options)
            : base(options)
        {
        }
   
        public virtual DbSet<Example> Examples { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //override for aspnetuser tables
            builder.CreateCustomSecurityModel();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return Database.BeginTransaction();
        }

        public IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return Database.BeginTransaction(isolationLevel);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await Database.BeginTransactionAsync();
        }


        /// <summary>
        /// Override to hide the parameter for cancellation tokens iot prevent mannually setting an empty token for every savechanges.
        /// </summary>
        /// <returns>int  result.</returns>
        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync(CancellationToken.None);
        }

        /// <summary>
        /// Excecutes (custom) raw stored-procedures, use return parameters to retrieve information back from the STP.
        /// </summary>
        /// <param name="stp">The STP.</param>
        /// <param name="parameters">Array of SqlParameter.</param>
        /// <returns>int result.</returns>
        public async Task<int> ExecuteSqlCommandAsync(RawSqlString stp, params object[] parameters)
        {
            return await Database.ExecuteSqlCommandAsync(stp, parameters);
        }

    }
}
