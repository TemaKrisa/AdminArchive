namespace AdminArchive.Model;

public partial class Feature
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
    public virtual ICollection<UnitFeatures> UnitFeatures { get; set; } = new List<UnitFeatures>();
    public virtual ICollection<DocumentFeatures> DocumentFeatures { get; set; } = new List<DocumentFeatures>();
    public virtual ICollection<StorageUnit> Units { get; set; } = new List<StorageUnit>();
}