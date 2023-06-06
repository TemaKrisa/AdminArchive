namespace AdminArchive.Model;
partial class Fond
{
    public string FullNumber { get => Index + "" + Number + "" + Literal; }
    public int? StartDate
    {
        get
        {
            using (var context = new ArchiveBdContext())
            {
                int? maxYear = context.Inventories
                    .AsEnumerable().Where(i => i.Fond == this.Id).Select(i => i.StartDate)
                    .DefaultIfEmpty().Min();
                return maxYear;
            }
        }
    }
    public int? EndDate
    {
        get
        {
            using (var context = new ArchiveBdContext())
            {
                int? minYear = context.Inventories.AsEnumerable().Where(i => i.Fond == this.Id)
                    .Select(i => i.EndDate).DefaultIfEmpty().Max();
                return minYear;
            }
        }
    }
    public int? Volume
    {
        get
        {
            using (var context = new ArchiveBdContext())
            {
                int? volume = context.Inventories.AsEnumerable().Where(i => i.Fond == this.Id)
                    .Select(i => i.Volume).DefaultIfEmpty().Sum();
                return volume;
            }
        }
    }
}