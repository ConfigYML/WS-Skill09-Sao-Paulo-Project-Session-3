using Microsoft.UI.Xaml;

namespace Session_3_Dennis_Hilfinger;

public partial class FindOutMorePage : ContentPage
{
    DispatcherTimer timer = new DispatcherTimer();
    public FindOutMorePage()
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

    private async void AboutMarathonSkills(object sender, EventArgs e)
	{
        await Shell.Current.GoToAsync("AboutMarathonSkillsPage");
    }
    private async void PreviousRaceResults(object sender, EventArgs e)
    {
        await DisplayAlert("Functionality not implemented", "This feature is not implemented yet.", "OK");
    }
    private async void BmiCalc(object sender, EventArgs e)
    {
        await DisplayAlert("Functionality not implemented", "This feature is not implemented yet.", "OK");
    }
    private async void MarathonInfo(object sender, EventArgs e)
    {
        await DisplayAlert("Functionality not implemented", "This feature is not implemented yet.", "OK");
    }
    private async void ListOfCharities(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("CharityListPage");
    }
    private async void BmrCalc(object sender, EventArgs e)
    {
        await DisplayAlert("Functionality not implemented", "This feature is not implemented yet.", "OK");
    }
}