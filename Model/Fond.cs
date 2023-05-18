using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class Fond
{
    public int FondId { get; set; }

    public string? FondName { get; set; }

    public string? FondShortName { get; set; }

    public int? View { get; set; }

    public int? SecretChar { get; set; }

    public int? Acess { get; set; }

    public int? Category { get; set; }

    public int? Type { get; set; }

    public int? DocType { get; set; }

    public int? OwnerShip { get; set; }

    public int? StorageTime { get; set; }

    public int? ReceiptReason { get; set; }

    public short? StartDate { get; set; }

    public short? EndDate { get; set; }

    public int? Volume { get; set; }

    public int? Movement { get; set; }

    public int? MovementType { get; set; }

    public int? FondNumber { get; set; }

    public int? HistoricalPeriod { get; set; }

    public int? CharRestrict { get; set; }

    public int? IncomeSource { get; set; }

    public short? ReceiptDate { get; set; }

    public short? LastReconcilation { get; set; }

    public short? LastCheck { get; set; }

    public string? MovementNote { get; set; }

    public string? Annotation { get; set; }

    public string? HistoricalOverview { get; set; }

    public string? FondIndex { get; set; }

    public string? FondLiteral { get; set; }

    public virtual Acess? AcessNavigation { get; set; }

    public virtual Category? CategoryNavigation { get; set; }

    public virtual CharRestrict? CharRestrictNavigation { get; set; }

    public virtual DocType? DocTypeNavigation { get; set; }

    public virtual ICollection<FondLog> FondLogs { get; set; } = new List<FondLog>();

    public virtual ICollection<FondName> FondNames { get; set; } = new List<FondName>();

    public virtual HistoricalPeriod? HistoricalPeriodNavigation { get; set; }

    public virtual IncomeSource? IncomeSourceNavigation { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual Movement? MovementNavigation { get; set; }

    public virtual MovementType? MovementTypeNavigation { get; set; }

    public virtual Ownership? OwnerShipNavigation { get; set; }

    public virtual ReceiptReason? ReceiptReasonNavigation { get; set; }

    public virtual SecretChar? SecretCharNavigation { get; set; }

    public virtual StorageTime? StorageTimeNavigation { get; set; }

    public virtual FondType? TypeNavigation { get; set; }

    public virtual ICollection<UndocumentPeriod> UndocumentPeriods { get; set; } = new List<UndocumentPeriod>();

    public virtual FondView? ViewNavigation { get; set; }
}
