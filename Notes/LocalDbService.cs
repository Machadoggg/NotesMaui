using Notes;
using SQLite;
using System.ComponentModel.DataAnnotations;

namespace Notes
{
    public class LocalDbService
    {
        private const string DB_NAME = "demo_local_db.db3";
        private readonly SQLiteAsyncConnection _connection;

        public LocalDbService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
            _connection.CreateTableAsync<Customer>();
        }


        public async Task<List<Customer>> GetCustomers()
        {
            var customer = await _connection.Table<Customer>().ToListAsync();
            return customer.OrderByDescending(c => c.Id).ToList();
        }

        public async Task<Customer> GetById(int id)
        {
            return await _connection.Table<Customer>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task Create(Customer customer)
        {
            var validationContext = new ValidationContext(customer);
            var validationResults = new List<ValidationResult>();

            // Validar el objeto Customer
            bool isValid = Validator.TryValidateObject(customer, validationContext, validationResults, true);

            if (!isValid)
            {
                // Mostrar errores de validación en una alerta
                string errorMessage = "No se puede guardar el cliente debido a los siguientes errores:\n";
                foreach (var validationResult in validationResults)
                {
                    errorMessage += $"- {validationResult.ErrorMessage}\n";
                }

                // Mostrar la alerta al usuario
                await Shell.Current.DisplayAlert("Error", errorMessage, "Aceptar");
                return;
            }
            await _connection.InsertAsync(customer);
            await Shell.Current.DisplayAlert("Éxito", "Cliente guardado correctamente.", "Aceptar");
        }

        public async Task Update(Customer customer)
        {
            await _connection.UpdateAsync(customer);
        }

        public async Task Delete(Customer customer)
        {
            await _connection.DeleteAsync(customer);
        }
    }
}
