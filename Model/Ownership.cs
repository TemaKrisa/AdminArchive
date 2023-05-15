using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class Ownership
{
    public int OwnershipId { get; set; }

    public string OwershipName { get; set; } = null!;

    public virtual ICollection<Fond> Fonds { get; set; } = new List<Fond>();
}
