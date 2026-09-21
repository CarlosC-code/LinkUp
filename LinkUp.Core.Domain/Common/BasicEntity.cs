
namespace LinkUp.Core.Domain.Common
{
   public class BasicEntity<Tkey>
    {

        public  Tkey Id { get; set; } 
        public  DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public  DateTime UpdatedAtUtc { get; set; }

    }
}
