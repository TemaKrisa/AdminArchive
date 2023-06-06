﻿namespace AdminArchive.Model;

public partial class CharRestrict
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Fond> Fonds { get; set; } = new List<Fond>();

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<StorageUnit> StorageUnits { get; set; } = new List<StorageUnit>();
}
