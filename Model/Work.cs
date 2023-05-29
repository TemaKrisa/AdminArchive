namespace AdminArchive.Model;
public partial class Work
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public virtual ICollection<UnitCompletedWork> UnitCompletedWorks { get; set; } = new List<UnitCompletedWork>();
    public virtual ICollection<UnitRequiredWork> UnitRequiredWorks { get; set; } = new List<UnitRequiredWork>();
}
