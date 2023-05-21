using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class StorageTime
{
    public int TimeId { get; set; }

    public string TimeName { get; set; } = null!;

    public virtual ICollection<Fond> Fonds { get; set; } = new List<Fond>();

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}
