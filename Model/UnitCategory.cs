using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class UnitCategory
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<StorageUnit> StorageUnits { get; set; } = new List<StorageUnit>();
}
