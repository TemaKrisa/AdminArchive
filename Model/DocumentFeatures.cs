namespace AdminArchive.Model;

public partial class DocumentFeatures
{
    public int DocumentId { get; set; }
    public int FeatureId { get; set; }
    public virtual Document Documents { get; set; } = null!;
    public virtual Feature Feature { get; set; } = null!;
}