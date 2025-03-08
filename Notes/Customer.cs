using SQLite;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ColumnAttribute = SQLite.ColumnAttribute;
using TableAttribute = SQLite.TableAttribute;

namespace Notes
{
    [Table("customer")]
    public class Customer
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del contacto es obligatorio.")]
        [Column("customer_name")]
        public string CustomerName { get; set; } = default!;

        [Required(ErrorMessage = "El número del teléfono es obligatorio.")]
        [Column("mobile")]
        public string Mobile { get; set; } = default!;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "El formato del correo electrónico no es válido.")]
        [Column("email")]
        public string Email { get; set; } = default!;
    }
}
