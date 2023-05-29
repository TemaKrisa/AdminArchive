namespace AdminArchive.Model;

public partial class InventoryType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}
