using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class Document
{
    public int DocumentId { get; set; }

    public string? DocumentName { get; set; }

    public DateTime? Date { get; set; }

    public int? SecretChar { get; set; }

    public string? Applications { get; set; }

    public int? Reproduction { get; set; }

    public string? Note { get; set; }

    public int? Vol { get; set; }

    public int? VolStart { get; set; }

    public int? VolEnd { get; set; }

    public int? DocType { get; set; }

    public int? StorageUnit { get; set; }

    public virtual DocType? DocTypeNavigation { get; set; }

    public virtual Reproduction? ReproductionNavigation { get; set; }

    public virtual StorageUnit? StorageUnitNavigation { get; set; }

    public virtual ICollection<Feature> Features { get; set; } = new List<Feature>();
}
