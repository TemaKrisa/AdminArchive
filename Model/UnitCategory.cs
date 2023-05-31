using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class UnitCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<StorageUnit> StorageUnits { get; set; } = new List<StorageUnit>();
}
