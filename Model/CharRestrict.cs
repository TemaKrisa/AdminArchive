﻿using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class CharRestrict
{
    public int RestrictId { get; set; }

    public string RestrictName { get; set; } = null!;

    public virtual ICollection<Fond> Fonds { get; set; } = new List<Fond>();
}