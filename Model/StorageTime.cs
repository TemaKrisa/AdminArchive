using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class StorageTime
{
    public int TymeId { get; set; }

    public string TymeName { get; set; } = null!;

    public virtual ICollection<Fond> Fonds { get; set; } = new List<Fond>();
}
