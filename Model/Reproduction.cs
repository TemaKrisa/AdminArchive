using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class Reproduction
{
    public int ReproductionId { get; set; }

    public string ReproductionName { get; set; } = null!;

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
}
