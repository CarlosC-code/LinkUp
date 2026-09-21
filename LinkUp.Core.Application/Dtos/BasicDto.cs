namespace LinkUp.Core.Application.Dtos
{
    public class BasicDto<Tkey>
    {
        public Tkey Id { get; set; } = default!;
        public  DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public  DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
