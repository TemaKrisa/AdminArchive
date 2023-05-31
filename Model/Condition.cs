using AdminArchive.Classes;

namespace AdminArchive.Model;

public partial class Condition : IHasId
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<UnitCondition> UnitConditions { get; set; } = new List<UnitCondition>();
}
