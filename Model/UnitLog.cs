using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class UnitLog
{
    public int Id { get; set; }

    public int? User { get; set; }

    public int? Activity { get; set; }

    public DateTime? Date { get; set; }

    public int? Unit { get; set; }

    public virtual Activity? ActivityNavigation { get; set; }

    public virtual StorageUnit? UnitNavigation { get; set; }

    public virtual User? UserNavigation { get; set; }
}
