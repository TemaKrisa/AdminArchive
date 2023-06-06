namespace AdminArchive.Model;

public partial class UnitRequiredWork
{
    public int Id { get; set; }

    public int Unit { get; set; }

    public int Work { get; set; }

    public string? Note { get; set; }

    public DateTime? CheckDate { get; set; }

    public virtual StorageUnit UnitNavigation { get; set; } = null!;

    public virtual Work WorkNavigation { get; set; } = null!;
}
