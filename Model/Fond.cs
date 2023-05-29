namespace AdminArchive.Model;
public partial class Fond
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string ShortName { get; set; } = null!;
    public int View { get; set; }
    public int SecretChar { get; set; }
    public int Acess { get; set; }
    public int Category { get; set; }
    public int Type { get; set; }
    public int DocType { get; set; }
    public int OwnerShip { get; set; }
    public int StorageTime { get; set; }
    public int ReceiptReason { get; set; }
    public int Movement { get; set; }
    public int? MovementType { get; set; }
    public int? Number { get; set; }
    public int HistoricalPeriod { get; set; }
    public int? CharRestrict { get; set; }
    public int IncomeSource { get; set; }
    public DateTime ReceiptDate { get; set; }
    public short? LastReconcilation { get; set; }
    public short? LastCheck { get; set; }
    public string? MovementNote { get; set; }
    public string? Annotation { get; set; }
    public string? HistoricalOverview { get; set; }
    public string? Index { get; set; }
    public string? Literal { get; set; }
    public virtual Acess AcessNavigation { get; set; } = null!;
    public virtual Category CategoryNavigation { get; set; } = null!;
    public virtual CharRestrict? CharRestrictNavigation { get; set; }
    public virtual DocType DocTypeNavigation { get; set; } = null!;
    public virtual ICollection<FondLog> FondLogs { get; set; } = new List<FondLog>();
    public virtual ICollection<FondName> FondNames { get; set; } = new List<FondName>();
    public virtual HistoricalPeriod HistoricalPeriodNavigation { get; set; } = null!;
    public virtual IncomeSource IncomeSourceNavigation { get; set; } = null!;
    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
    public virtual Movement MovementNavigation { get; set; } = null!;
    public virtual MovementType? MovementTypeNavigation { get; set; }
    public virtual Ownership OwnerShipNavigation { get; set; } = null!;
    public virtual ReceiptReason ReceiptReasonNavigation { get; set; } = null!;
    public virtual SecretChar SecretCharNavigation { get; set; } = null!;
    public virtual StorageTime StorageTimeNavigation { get; set; } = null!;
    public virtual FondType TypeNavigation { get; set; } = null!;
    public virtual ICollection<UndocumentPeriod> UndocumentPeriods { get; set; } = new List<UndocumentPeriod>();
    public virtual FondView ViewNavigation { get; set; } = null!;
}
