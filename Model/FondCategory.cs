using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class FondCategory
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Fond> Fonds { get; set; } = new List<Fond>();
}
