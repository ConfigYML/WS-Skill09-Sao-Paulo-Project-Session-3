using Microsoft.UI.Xaml;
using System.Reflection.Metadata;

namespace Session_3_Dennis_Hilfinger;

public partial class LoginPage : ContentPage
{
    DispatcherTimer timer = new DispatcherTimer();
    public LoginPage()
	{
		InitializeComponent();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += timerTick;
        timer.Start();
    }

    private void timerTick(object? sender, object e)
    {
        DateTime targetTime = new DateTime(2026, 9, 5, 6, 0, 0);
        DateTime currentTime = DateTime.Now;
        TimeSpan timeDiff = targetTime - currentTime;

        TimerLabel.Text = string.Format("{0} days {1} hours and {2} minutes until the race starts!",
            timeDiff.Days,
            timeDiff.Hours,
            timeDiff.Minutes);
    }

    private void CheckLogin(object sender, EventArgs e)
    {
        string? mail = EmailEntry.Text;
        string? password = PasswordEntry.Text;

        if (String.IsNullOrEmpty(mail) || String.IsNullOrEmpty(password))
        {
            DisplayAlert("Missing credentials", "Please fill out all required credentials", "Ok");
            return;
        }
        using(var db = new MarathonDB())
        {
            var user = db.Users.FirstOrDefault(u => u.Email == mail);
            if (user != null)
            {
                if (user.Password == password)
                {
                    DisplayAlert("Login successful", "Login has succeeded", "Ok");
                    var userData = new ShellNavigationQueryParameters()
                    {
                        {"User", user }
                    };
                    switch (user.RoleId)
                    {
                        case "R":
                            AppShell.Current.GoToAsync("RunnerPage", userData);
                            break;
                        case "C":
                            AppShell.Current.GoToAsync("CoordinatorPage", userData);
                            break;
                        case "A":
                            AppShell.Current.GoToAsync("AdminPage", userData);
                            break;
                    }
                    return;
                }
            }
            DisplayAlert("Login unsuccessful", "Please check the credentials you entered and correct them", "Ok");
            return;
        }
    }

    private void Cancel(object sender, EventArgs e)
    {
        AppShell.Current.GoToAsync("//MainPage");
    }
}