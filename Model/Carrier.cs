using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class Carrier
{
    public int CarrierId { get; set; }

    public string CarrierName { get; set; } = null!;

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<StorageUnit> StorageUnits { get; set; } = new List<StorageUnit>();
}
