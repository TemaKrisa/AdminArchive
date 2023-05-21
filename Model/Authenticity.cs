using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class Authenticity
{
    public int AuthenticityId { get; set; }

    public string AuthenticityName { get; set; } = null!;

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
}
