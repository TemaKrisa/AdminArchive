using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class FondView
{
    public int ViewId { get; set; }

    public string ViewName { get; set; } = null!;

    public virtual ICollection<Fond> Fonds { get; set; } = new List<Fond>();
}
