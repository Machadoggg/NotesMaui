using Plugin.LocalNotification;

namespace Notes;

public partial class DatePage : ContentPage
{
	public DatePage()
	{
		InitializeComponent();
	}

    private void OnScheduleNotificationClicked(object sender, EventArgs e)
    {
        var selectedDate = datePicker.Date;
        var selectedTime = timePicker.Time;

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
            DisplayAlert("Éxito", $"Notificación programada.{notificationTime}", "OK");
        }
        else
        {
            DisplayAlert("Error", "La fecha y hora deben ser futuras.", "OK");
        }
    }

    private void ScheduleNotification(DateTime notificationTime)
    {
        var notification = new NotificationRequest
        {
            NotificationId = 1001,
            Title = "App Name",
            Subtitle = "Hello",
            Description = "Description",
            BadgeNumber = 42,
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = notificationTime
            }
        };

        // Programar la notificación
        LocalNotificationCenter.Current.Show(notification);
    }
}