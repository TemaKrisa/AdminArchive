using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class Document
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime Date { get; set; }

    public int SecretChar { get; set; }

    public string? Applications { get; set; }

    public int? Reproduction { get; set; }

    public string? Note { get; set; }

    public int Vol { get; set; }

    public int VolStart { get; set; }

    public int VolEnd { get; set; }

    public int DocType { get; set; }

    public int StorageUnit { get; set; }

    public string? Annotation { get; set; }

    public int? Authenticity { get; set; }

    public int? Number { get; set; }

    public virtual Authenticity? AuthenticityNavigation { get; set; }

    public virtual DocType DocTypeNavigation { get; set; } = null!;

    public virtual ICollection<DocumentFile> DocumentFiles { get; set; } = new List<DocumentFile>();

    public virtual ICollection<DocumentLog> DocumentLogs { get; set; } = new List<DocumentLog>();

    public virtual Reproduction? ReproductionNavigation { get; set; }

    public virtual SecretChar SecretCharNavigation { get; set; } = null!;

    public virtual StorageUnit StorageUnitNavigation { get; set; } = null!;

    public virtual ICollection<Feature> Features { get; set; } = new List<Feature>();
}
