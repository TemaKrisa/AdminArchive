namespace AdminArchive.Model;
partial class Inventory
{
    //Получение минимальной начальной даты, с исключением выбытых единиц хранения
    public int? StartDate
    {
        get
        {
            using ArchiveBdContext context = new();
            int? maxYear = context.StorageUnits.AsEnumerable()
                .Where(i => i.Inventory == this.Id && i.IsRetired == false).Select(i => i.StartDate)
                .DefaultIfEmpty().Min();
            if (maxYear == 0) return null;
            return maxYear;

        }
    }
    //Получение максимальной конечной даты, с исключением выбытых единиц хранения
    public int? EndDate
    {
        get
        {
            using ArchiveBdContext context = new();
            int? minYear = context.StorageUnits.AsEnumerable()
                .Where(i => i.Inventory == this.Id && i.IsRetired == false).Select(i => i.EndDate)
                .DefaultIfEmpty().Max();
            if (minYear == 0) return null;
            return minYear;
        }
    }
    //Получение обьема, с исключением выбытых единиц хранения
    public int? Volume
    {
        get
        {
            using ArchiveBdContext context = new();
            int? volume = context.StorageUnits.AsEnumerable().
                Where(i => i.Inventory == this.Id && i.IsRetired == false).Select(i => i.Volume)
                .DefaultIfEmpty().Sum();
            return volume;
        }
    }
    //Конвертация полного номера
    public string FullNumber { get { return Number + "" + Literal; } }
}
