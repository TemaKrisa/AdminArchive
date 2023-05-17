using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class Inventory
{
    public int InventoryId { get; set; }

    public int? Fond { get; set; }

    public string? Name { get; set; }

    public int? Type { get; set; }

    public string? InventoryNumber { get; set; }

    public int? Volume { get; set; }

    public string? Title { get; set; }

    public int? DocType { get; set; }

    public int? Carrier { get; set; }

    public int? Acess { get; set; }

    public int? SecretChar { get; set; }

    public string? Note { get; set; }

    public int? Movement { get; set; }

    public int? MovementType { get; set; }

    public int? Category { get; set; }

    public int? ReceiptReason { get; set; }

    public int? IncomeSource { get; set; }

    public int? StorageTime { get; set; }

    public int? CharRestrict { get; set; }

    public string? MovementNote { get; set; }

    public virtual Acess? AcessNavigation { get; set; }

    public virtual Carrier? CarrierNavigation { get; set; }

    public virtual Category? CategoryNavigation { get; set; }

    public virtual CharRestrict? CharRestrictNavigation { get; set; }

    public virtual Fond? FondNavigation { get; set; }

    public virtual IncomeSource? IncomeSourceNavigation { get; set; }

    public virtual ICollection<InventoryLog> InventoryLogs { get; set; } = new List<InventoryLog>();

    public virtual Movement? MovementNavigation { get; set; }

    public virtual MovementType? MovementTypeNavigation { get; set; }

    public virtual ReceiptReason? ReceiptReasonNavigation { get; set; }

    public virtual StorageTime? StorageTimeNavigation { get; set; }

    public virtual ICollection<StorageUnit> StorageUnits { get; set; } = new List<StorageUnit>();

    public virtual InventoryType? TypeNavigation { get; set; }
}
