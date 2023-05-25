namespace AdminArchive.Model
{
    partial class Inventory
    {
        public int? StartDate
        {
            get
            {
                using (var context = new ArchiveBdContext())
                {
                    int? maxYear = context.StorageUnits
                        .AsEnumerable()
                        .Where(i => i.Inventory == this.Id)
                        .Select(i => i.StartDate) // Add this line
                        .DefaultIfEmpty() // Add this line
                        .Min();
                    if (maxYear == 0) return null;
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
                    int? minYear = context.StorageUnits
                        .AsEnumerable()
                        .Where(i => i.Inventory == this.Id)
                        .Select(i => i.EndDate) // Add this line
                        .DefaultIfEmpty() // Add this line
                        .Max();
                    if (minYear == 0) return null;
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
                    int? volume = context.StorageUnits
                        .AsEnumerable()
                        .Where(i => i.Inventory == this.Id)
                        .Select(i => i.Volume)
                        .DefaultIfEmpty()
                        .Sum();
                    return volume;
                }
            }
        }

        public string FullNumber
        {
            get
            {
                return Number + "" + Literal;
            }
        }
    }
}
