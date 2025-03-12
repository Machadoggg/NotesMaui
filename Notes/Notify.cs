

using SQLite;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ColumnAttribute = SQLite.ColumnAttribute;
using TableAttribute = SQLite.TableAttribute;

namespace Notes
{
    [Table("notify")]
    public class Notify
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [Column("notify_name")]
        public string NotifyName { get; set; } = default!;

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        [Column("date")]
        public DateTime Date { get; set; } = default!;

        [Required(ErrorMessage = "La hora es obligatoria.")]

        [Column("hour")]
        public TimeSpan Hour { get; set; } = default!;

        
        [Column("notification_time ")]
        public DateTime NotificationTime { get; set; } = default!;
    }
}

