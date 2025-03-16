namespace Notes.Models
{
    public class Note
    {
        public string Filename { get; set; } = default!;
        public string Text { get; set; } = default!;
        public DateTime Date { get; set; }


        public Note()
        {
            Filename = $"{Path.GetRandomFileName()}.notes.txt";
            Date = DateTime.Now;
            Text = "";
        }

        public void Save() =>
            File.WriteAllText(System.IO.Path.Combine(FileSystem.AppDataDirectory, Filename), Text);

        public void Delete() => 
            File.Delete(System.IO.Path.Combine(FileSystem.AppDataDirectory, Filename));

        public static Note Load(string filename)
        {
            filename = System.IO.Path.Combine(FileSystem.AppDataDirectory, filename);

            if (!File.Exists(filename))
                throw new FileNotFoundException("Unable to find file on local storage.", filename);

            return
                new()
                {
                    Filename = Path.GetFileName(filename),
                    Text = File.ReadAllText(filename),
                    Date = DateTime.Now,
                };  
        }
    }
}
