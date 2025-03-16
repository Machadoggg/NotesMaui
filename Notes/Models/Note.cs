namespace Notes.Models
{
    public class Note
    {
        public string Filename { get; set; } = default!;
        public string Text { get; set; } = default!;
        public DateTime Date { get; set; }
    }
}
