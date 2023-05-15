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

    public virtual Acess? AcessNavigation { get; set; }

    public virtual Carrier? CarrierNavigation { get; set; }

    public virtual Fond? FondNavigation { get; set; }

    public virtual ICollection<InventoryLog> InventoryLogs { get; set; } = new List<InventoryLog>();

    public virtual ICollection<StorageUnit> StorageUnits { get; set; } = new List<StorageUnit>();

    public virtual InventoryType? TypeNavigation { get; set; }
}
