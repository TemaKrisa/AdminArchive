using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class InventoryLog
{
    public int LogId { get; set; }

    public int User { get; set; }

    public int Inventory { get; set; }

    public int Activity { get; set; }

    public DateTime Date { get; set; }

    public virtual Activity ActivityNavigation { get; set; } = null!;

    public virtual Inventory InventoryNavigation { get; set; } = null!;

    public virtual User UserNavigation { get; set; } = null!;
}
