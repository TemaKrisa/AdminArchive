namespace AdminArchive.Model;

public partial class FondLog
{
    public int Id { get; set; }

    public int User { get; set; }

    public int Activity { get; set; }

    public DateTime Date { get; set; }

    public int Fond { get; set; }

    public virtual Activity ActivityNavigation { get; set; } = null!;

    public virtual Fond FondNavigation { get; set; } = null!;

    public virtual User UserNavigation { get; set; } = null!;
}
