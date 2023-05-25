namespace AdminArchive.Model
{
    partial class StorageUnit
    {
        public string FullNumber
        {
            get
            {
                return Number + "" + Literal;
            }
        }
    }
}
