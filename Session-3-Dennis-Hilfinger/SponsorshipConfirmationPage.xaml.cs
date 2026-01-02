
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;

namespace Session_3_Dennis_Hilfinger;

public partial class SponsorshipConfirmationPage : ContentPage, IQueryAttributable
{
    DispatcherTimer timer = new DispatcherTimer();
    int runnerId { get; set; }
	public SponsorshipConfirmationPage()
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

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        runnerId = query["RunnerId"] as int? ?? 0;
        AmountLabel.Text = $"$ {query["Amount"]}";
        SetRunnerInfo();
    }

    private void SetRunnerInfo()
    {
        using(var db = new MarathonDB())
        {
            var runner = db.Runners
                .Include(r => r.CountryCodeNavigation)
                .Where(r => r.RunnerId == runnerId)
                .Select(r => new
                {
                    r.EmailNavigation.FirstName,
                    r.EmailNavigation.LastName,
                    r.RunnerId,
                    r.CountryCodeNavigation.CountryName
                })
                .FirstOrDefault();
            if (runner != null)
            {
                RunnerLabel.Text = $"{runner.FirstName} {runner.LastName} ({runner.RunnerId}) from {runner.CountryName}";
            }
        }
    }

    private void BackToMainPage(object sender, EventArgs e)
	{
		AppShell.Current.GoToAsync("//MainPage");
    }
}