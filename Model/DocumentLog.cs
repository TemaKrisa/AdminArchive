using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class DocumentLog
{
    public int Id { get; set; }

    public int User { get; set; }

    public int Activity { get; set; }

    public DateTime Date { get; set; }

    public int Document { get; set; }

    public virtual Activity ActivityNavigation { get; set; } = null!;

    public virtual Document DocumentNavigation { get; set; } = null!;

    public virtual User UserNavigation { get; set; } = null!;
}
