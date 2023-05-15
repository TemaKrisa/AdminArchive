using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class SecretChar
{
    public int CharId { get; set; }

    public string CharName { get; set; } = null!;

    public virtual ICollection<Fond> Fonds { get; set; } = new List<Fond>();
}
