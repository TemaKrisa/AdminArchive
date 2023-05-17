using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class Movement
{
    public int MovementId { get; set; }

    public string MovementName { get; set; } = null!;

    public virtual ICollection<Fond> Fonds { get; set; } = new List<Fond>();

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}
