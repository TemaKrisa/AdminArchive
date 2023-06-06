namespace AdminArchive.Model;

public partial class UnitLog
{
    public int Id { get; set; }

    public int User { get; set; }

    public int Activity { get; set; }

    public DateTime Date { get; set; }

    public int Unit { get; set; }

    public virtual Activity ActivityNavigation { get; set; } = null!;

    public virtual StorageUnit UnitNavigation { get; set; } = null!;

    public virtual User UserNavigation { get; set; } = null!;
}
