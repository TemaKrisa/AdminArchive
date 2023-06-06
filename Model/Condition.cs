namespace AdminArchive.Model;

public partial class Condition
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<UnitCondition> UnitConditions { get; set; } = new List<UnitCondition>();
}
