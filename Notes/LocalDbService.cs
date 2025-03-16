using Notes.Models;
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
            _connection.CreateTableAsync<Notify>();
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




        public async Task<List<Notify>> GetNotify()
        {
            var notify = await _connection.Table<Notify>().ToListAsync();
            return notify.OrderByDescending(c => c.Id).ToList();
        }

        public async Task<Notify> GetNotifyById(int id)
        {
            return await _connection.Table<Notify>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateNotify(Notify notify)
        {
            var validationContext = new ValidationContext(notify);
            var validationResults = new List<ValidationResult>();

            // Validate object notify
            bool isValid = Validator.TryValidateObject(notify, validationContext, validationResults, true);

            if (!isValid)
            {
                // Show errors validation alert
                string errorMessage = "No se puede guardar la notificación debido a los siguientes errores:\n";
                foreach (var validationResult in validationResults)
                {
                    errorMessage += $"- {validationResult.ErrorMessage}\n";
                }

                // Show alert user
                await Shell.Current.DisplayAlert("Error", errorMessage, "Aceptar");
                return;
            }
            await _connection.InsertAsync(notify);
            await Shell.Current.DisplayAlert("Éxito", "Notificación programada correctamente.", "Aceptar");
            
        }

        public async Task UpdateNotify(Notify notify)
        {
            await _connection.UpdateAsync(notify);
            await Shell.Current.DisplayAlert("Éxito", "Notificación editada correctamente.", "Aceptar");
        }

        public async Task DeleteNotify(Notify notify)
        {
            var response = await Shell.Current.DisplayAlert("Cuidado", "Esta seguro de eliminar la notificación?", "Aceptar", "Cancelar");
            if (response)
            {
                await _connection.DeleteAsync(notify);
                await Shell.Current.DisplayAlert("Éxito", "Notificación eliminada correctamente.", "Aceptar");
            }
        }

    }
}
