using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class FondType
{
    public int TypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<Fond> Fonds { get; set; } = new List<Fond>();
}
