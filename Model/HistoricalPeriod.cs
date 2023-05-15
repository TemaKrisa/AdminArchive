using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class HistoricalPeriod
{
    public int PeriodId { get; set; }

    public string PeriodName { get; set; } = null!;

    public virtual ICollection<Fond> Fonds { get; set; } = new List<Fond>();
}
