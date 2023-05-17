using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Fond> Fonds { get; set; } = new List<Fond>();

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}
