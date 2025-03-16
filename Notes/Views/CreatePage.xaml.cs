using Notes.Models;

namespace Notes;

public partial class CreatePage : ContentPage
{

    private readonly LocalDbService _localDbService;
    private int _editCustomerId;

    public CreatePage(LocalDbService localDbService)
    {
        InitializeComponent();
        _localDbService = localDbService;
        Task.Run(async () => listView.ItemsSource = await _localDbService.GetCustomers());
    }

    private async void saveButton_Clicked(object sender, EventArgs e)
    {
        if (_editCustomerId == 0)
        {
            //add customer
            await _localDbService.Create(new Customer
            {
                CustomerName = nameEntryfield.Text,
                Email = emailEntryfield.Text,
                Mobile = mobileEntryfield.Text,
            });
        }
        else
        {
            //edit customer
            await _localDbService.Update(new Customer
            {
                Id = _editCustomerId,
                CustomerName = nameEntryfield.Text,
                Email = emailEntryfield.Text,
                Mobile = mobileEntryfield.Text,
            });

            _editCustomerId = 0;
        }

        nameEntryfield.Text = string.Empty;
        emailEntryfield.Text = string.Empty;
        mobileEntryfield.Text = string.Empty;

        listView.ItemsSource = await _localDbService.GetCustomers();
    }

    private async void listView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        var customer = (Customer)e.Item;
        var action = await DisplayActionSheet("Action", "Cancel", null, "Edit", "Delete");

        switch (action)
        {
            case "Edit":
                _editCustomerId = customer.Id;
                nameEntryfield.Text = customer.CustomerName;
                emailEntryfield.Text = customer.Email;
                mobileEntryfield.Text = customer.Mobile;
                break;

            case "Delete":
                await _localDbService.Delete(customer);
                listView.ItemsSource = await _localDbService.GetCustomers();
                break;
        }
    }
}