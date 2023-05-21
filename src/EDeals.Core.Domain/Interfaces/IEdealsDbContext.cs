namespace EDeals.Core.Domain.Interfaces
{
    public interface IEdealsDbContext
    {

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
