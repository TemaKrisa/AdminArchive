namespace AdminArchive.Model;
public partial class Ownership
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public virtual ICollection<Fond> Fonds { get; set; } = new List<Fond>();
}
