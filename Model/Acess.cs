﻿using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class Acess
{
    public int AcessId { get; set; }

    public string AcessName { get; set; } = null!;

    public virtual ICollection<Fond> Fonds { get; set; } = new List<Fond>();

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<StorageUnit> StorageUnits { get; set; } = new List<StorageUnit>();
}