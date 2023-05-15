using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class IncomeSource
{
    public int SourceId { get; set; }

    public string SourceName { get; set; } = null!;

    public virtual ICollection<Fond> Fonds { get; set; } = new List<Fond>();
}
