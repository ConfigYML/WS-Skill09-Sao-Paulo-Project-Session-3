using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;

namespace Session_3_Dennis_Hilfinger;

public partial class RunnerPage : ContentPage, IQueryAttributable
{
    DispatcherTimer timer = new DispatcherTimer();
    User user;
	public RunnerPage()
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

    private void Logout(object? sender, EventArgs e)
    {
        AppShell.Current.GoToAsync("//MainPage");
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        using (var db = new MarathonDB())
        {
            user = db.Users.FirstOrDefault(u => u.Email == (query["User"] as User).Email);
            if (user != null)
            {
                var users = db.Users.Where(u => u.Email == user.Email)
                    .Include(u => u.Runners)
                        .ThenInclude(r => r.CountryCodeNavigation)
                    .ToList();
                user = users.First();
            }
            else
            {
                DisplayAlert("Error occurred", "User data could not be loaded.", "Ok");
                return;
            }
        }
    }

    public async void RegisterForEvent(object? sender, EventArgs e)
    {
        ShellNavigationQueryParameters userData = new ShellNavigationQueryParameters()
        {
            { "User", user }
        };
        await AppShell.Current.GoToAsync("RegisterEventPage", userData);
    }

    public async void MyRaceResults(object? sender, EventArgs e)
    {
        ShellNavigationQueryParameters userData = new ShellNavigationQueryParameters()
        {
            { "User", user }
        };
        await AppShell.Current.GoToAsync("MyRaceResultsPage", userData);
    }

    public async void EditYourProfile(object? sender, EventArgs e)
    {
        ShellNavigationQueryParameters userData = new ShellNavigationQueryParameters()
        {
            { "User", user }
        };
        await AppShell.Current.GoToAsync("EditProfilePage", userData);
    }

    public async void MySponsorship(object? sender, EventArgs e)
    {
        await DisplayAlert("Work in progress", "This feature is not implemented yet", "Ok");
        //await AppShell.Current.GoToAsync("MySponsorshipPage");
    }

    public async void ContactInformation(object? sender, EventArgs e)
    {
        Microsoft.Maui.Controls.Window window = new Microsoft.Maui.Controls.Window(new ContactInformationPage());
        window.Width = 600;
        window.Height = 400;
        Microsoft.Maui.Controls.Application.Current.OpenWindow(window);
    }
}