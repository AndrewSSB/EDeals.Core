namespace EDeals.Core.Domain.Common
{
    public abstract class BaseEntity<T>
    {
        protected BaseEntity(T id)
        {
            Id = id;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            IsDeleted = false;
        }

        public T Id { get; private init; }

        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }
        
        public bool IsDeleted { get; set; }
    }
}
