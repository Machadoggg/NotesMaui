using Notes.Models;
using Plugin.LocalNotification;

namespace Notes;

public partial class DatePage : ContentPage
{
    private readonly LocalDbService _localDbService;
    private int _editNotifyId;

    public DatePage(LocalDbService localDbService)
	{
		InitializeComponent();
        _localDbService = localDbService;
        Task.Run(async () => listView.ItemsSource = await _localDbService.GetNotify());
    }


    private async void OnScheduleNotificationClicked(object sender, EventArgs e)
    {
        var selectedDate = datePickerfield.Date;
        var selectedTime = timePickerfield.Time;

        var notificationTime = new DateTime(
            selectedDate.Year,
            selectedDate.Month,
            selectedDate.Day,
            selectedTime.Hours,
            selectedTime.Minutes,
            selectedTime.Seconds
        );

        if (notificationTime > DateTime.Now)
        {
            ScheduleNotification(notificationTime);




            if (_editNotifyId == 0)
            {
                //add notify
                await _localDbService.CreateNotify(new Notify
                {
                    NotifyName = nameEntryfield.Text,
                    Date = selectedDate.Date,
                    Hour = selectedTime,
                    NotificationTime = notificationTime,
                });
            }
            else
            {
                //edit notify
                await _localDbService.UpdateNotify(new Notify
                {
                    Id = _editNotifyId,
                    NotifyName = nameEntryfield.Text,
                    Date = selectedDate.Date,
                    Hour = selectedTime,
                    NotificationTime = notificationTime,
                });

                _editNotifyId = 0;
            }

            nameEntryfield.Text = string.Empty;
            datePickerfield.Date = DateTime.Now.Date;
            timePickerfield.Time = new TimeSpan(0, 0, 0);

            listView.ItemsSource = await _localDbService.GetNotify();   
        }
        else
        {
            await DisplayAlert("Error", "La fecha y hora deben ser futuras.", "OK");
        }
    }

    private async void ScheduleNotification(DateTime notificationTime)
    {
        var notification = new NotificationRequest
        {
            NotificationId = 1001,
            Title = "Notes",
            Subtitle = $"Notificación: {nameEntryfield.Text}",
            Description = nameEntryfield.Text,
            BadgeNumber = 42,
            CategoryType = NotificationCategoryType.Alarm,
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = notificationTime
            },
            Android = new Plugin.LocalNotification.AndroidOption.AndroidOptions
            {
                AutoCancel = true,
                IconSmallName = { ResourceName = "icon_notes" },
                IconLargeName = { ResourceName = "icon_about" }
            },
            Silent = false,
            Image = { ResourceName = "dotnet_bot" }
        };

        // Program notify
        await LocalNotificationCenter.Current.Show(notification);
    }

    private async void listView_ItemTapped2(object sender, ItemTappedEventArgs e)
    {
        var notify = (Notify)e.Item;
        var action = await DisplayActionSheet("Action", "Cancel", null, "Edit", "Delete");

        switch (action)
        {
            case "Edit":
                _editNotifyId = notify.Id;
                nameEntryfield.Text = notify.NotifyName;
                datePickerfield.Date = notify.Date;
                timePickerfield.Time = notify.Hour;
                break;

            case "Delete":
                await _localDbService.DeleteNotify(notify);
                listView.ItemsSource = await _localDbService.GetNotify();
                break;
        }
    }
}