﻿using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class FondName
{
    public int NamesId { get; set; }

    public int Fond { get; set; }

    public string Name { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Note { get; set; } = null!;

    public virtual Fond FondNavigation { get; set; } = null!;
}