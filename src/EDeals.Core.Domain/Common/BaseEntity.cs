using System.ComponentModel.DataAnnotations.Schema;

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

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
        
        public bool IsDeleted { get; set; }
    }
}
