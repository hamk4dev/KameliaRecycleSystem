namespace KameliaRecycleSystem.Core.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string CreatedBy { get; set; } = "System";
    public DateTime? ModifiedDate { get; set; }
    public string ModifiedBy { get; set; }
    public bool IsActive { get; set; } = true;
}
