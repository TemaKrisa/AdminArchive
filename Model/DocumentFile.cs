using System;
using System.Collections.Generic;

namespace AdminArchive.Model;

public partial class DocumentFile
{
    public int FileId { get; set; }

    public int? Document { get; set; }

    public byte[] File { get; set; } = null!;

    public string? Description { get; set; }

    public string FileName { get; set; } = null!;

    public virtual Document? DocumentNavigation { get; set; }
}
