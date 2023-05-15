using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class Activity
{
    public int ActivityId { get; set; }

    public string ActivityName { get; set; } = null!;

    public virtual ICollection<FondLog> FondLogs { get; set; } = new List<FondLog>();

    public virtual ICollection<InventoryLog> InventoryLogs { get; set; } = new List<InventoryLog>();

    public virtual ICollection<UnitLog> UnitLogs { get; set; } = new List<UnitLog>();
}
