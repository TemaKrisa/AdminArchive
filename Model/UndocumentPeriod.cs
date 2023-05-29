namespace AdminArchive.Model;

public partial class UndocumentPeriod
{
    public int Id { get; set; }

    public int Fond { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Reason { get; set; } = null!;

    public string DocumentLocation { get; set; } = null!;

    public string? Note { get; set; }

    public virtual Fond FondNavigation { get; set; } = null!;
}
