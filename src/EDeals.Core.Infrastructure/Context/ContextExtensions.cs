using EDeals.Core.Domain.Common;
using EDeals.Core.Infrastructure.EntityConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EDeals.Core.Infrastructure.Context
{
    public static class ContextExtensions
    {
        public static void ApplyEntityConfigurations(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationRoleConfiguration());
        }

        public static void EnableSoftDeleteCascade(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity<Guid>).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.PropertyOrField(parameter, "IsDeleted");
                    var condition = Expression.Equal(property, Expression.Constant(false));

                    var lambda = Expression.Lambda(condition, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }

        public static async Task ExecuteInTransaction<TContext>(this TContext context, Func<Task> inner, ILogger logger) 
            where TContext : DbContext
        {
            var executionStrategy = context.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using var transaction = await context.Database.BeginTransactionAsync();

                try
                {
                    await inner();
                    await transaction.CommitAsync();
                    logger.LogInformation("Transaction committed successfully.");
                }
                catch (Exception ex){
                    await transaction.RollbackAsync();
                    logger.LogError("Transaction failed. Rolling back...", ex.Message);
                }
            });
        }

        // TODO: when creating a generic response uptade the response here
        public static async Task<TResult> ExecuteInTransaction<TContext, TResult>(this TContext context, Func<Task<TResult>> inner, ILogger logger)
            where TContext : DbContext
        {
            var executionStrategy = context.Database.CreateExecutionStrategy();
            return await executionStrategy.ExecuteAsync(async () =>
            {
                using var transaction = await context.Database.BeginTransactionAsync();

                try
                {
                    var result = await inner();
                    await transaction.CommitAsync();
                    logger.LogInformation("Transaction committed successfully.");
                    return result;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    logger.LogError("Transaction failed. Rolling back...", ex.Message);
                    throw;
                }
            });
        }
    }
}
