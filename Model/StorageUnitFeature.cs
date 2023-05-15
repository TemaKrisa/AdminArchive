using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class StorageUnitFeature
{
    public int Unit { get; set; }

    public int Feature { get; set; }

    public virtual Feature FeatureNavigation { get; set; } = null!;

    public virtual StorageUnit UnitNavigation { get; set; } = null!;
}
