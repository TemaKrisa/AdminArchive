using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class Activity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<DocumentLog> DocumentLogs { get; set; } = new List<DocumentLog>();

    public virtual ICollection<FondLog> FondLogs { get; set; } = new List<FondLog>();

    public virtual ICollection<InventoryLog> InventoryLogs { get; set; } = new List<InventoryLog>();

    public virtual ICollection<UnitLog> UnitLogs { get; set; } = new List<UnitLog>();
}
