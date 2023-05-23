using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class DocType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual ICollection<Fond> Fonds { get; set; } = new List<Fond>();
}
