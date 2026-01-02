using Microsoft.UI.Xaml;

namespace Session_3_Dennis_Hilfinger;

public partial class AboutMarathonSkillsPage : ContentPage
{
    DispatcherTimer timer = new DispatcherTimer(); 
	public AboutMarathonSkillsPage()
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

    private void OpenMap(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("InteractiveMapPage");
    }
}