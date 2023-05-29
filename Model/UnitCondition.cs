namespace AdminArchive.Model;

public partial class UnitCondition
{
    public int Id { get; set; }

    public int Condition { get; set; }

    public int SheetsNumber { get; set; }

    public string? Note { get; set; }

    public int Unit { get; set; }

    public virtual Condition ConditionNavigation { get; set; } = null!;

    public virtual StorageUnit UnitNavigation { get; set; } = null!;
}
