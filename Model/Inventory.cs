using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class Inventory
{
    public int Id { get; set; }

    public int Fond { get; set; }

    public string Name { get; set; } = null!;

    public int Type { get; set; }

    public string Number { get; set; } = null!;

    public int DocType { get; set; }

    public int Carrier { get; set; }

    public int Acess { get; set; }

    public int SecretChar { get; set; }

    public string? Note { get; set; }

    public int Movement { get; set; }

    public int? MovementType { get; set; }

    public int Category { get; set; }

    public int ReceiptReason { get; set; }

    public int IncomeSource { get; set; }

    public int StorageTime { get; set; }

    public int? CharRestrict { get; set; }

    public string? MovementNote { get; set; }

    public string? Literal { get; set; }

    public string? Annotation { get; set; }

    public virtual Acess AcessNavigation { get; set; } = null!;

    public virtual Carrier CarrierNavigation { get; set; } = null!;

    public virtual Category CategoryNavigation { get; set; } = null!;

    public virtual CharRestrict? CharRestrictNavigation { get; set; }

    public virtual DocType DocTypeNavigation { get; set; } = null!;

    public virtual Fond FondNavigation { get; set; } = null!;

    public virtual IncomeSource IncomeSourceNavigation { get; set; } = null!;

    public virtual ICollection<InventoryLog> InventoryLogs { get; set; } = new List<InventoryLog>();

    public virtual Movement MovementNavigation { get; set; } = null!;

    public virtual MovementType? MovementTypeNavigation { get; set; }

    public virtual ReceiptReason ReceiptReasonNavigation { get; set; } = null!;

    public virtual SecretChar SecretCharNavigation { get; set; } = null!;

    public virtual StorageTime StorageTimeNavigation { get; set; } = null!;

    public virtual ICollection<StorageUnit> StorageUnits { get; set; } = new List<StorageUnit>();

    public virtual InventoryType TypeNavigation { get; set; } = null!;
}
