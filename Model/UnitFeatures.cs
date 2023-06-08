namespace AdminArchive.Model;

public partial class UnitFeatures
{
    public int UnitId { get; set; }
    public int FeatureId { get; set; }
    public virtual StorageUnit StorageUnit { get; set; } = null!;
    public virtual Feature Feature { get; set; } = null!;
}