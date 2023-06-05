namespace AdminArchive.Classes
{
    public interface Log
    {
        public int Id { get; set; }

        public int User { get; set; }

        public int Activity { get; set; }

        public DateTime Date { get; set; }
    }
}
