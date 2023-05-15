using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class InventoryType
{
    public int TypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}
