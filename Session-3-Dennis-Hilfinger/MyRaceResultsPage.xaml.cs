using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.UI.Xaml;
using Windows.ApplicationModel.Activation;
using Windows.Devices.Geolocation;
using Windows.System;

namespace Session_3_Dennis_Hilfinger;

public partial class MyRaceResultsPage : ContentPage, IQueryAttributable
{
    DispatcherTimer timer = new DispatcherTimer();
    User user;
	public MyRaceResultsPage()
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
        user = (User) query["User"];
        LoadResults();
    }

    private void LoadResults()
    {
        try
        {
            using(var db = new MarathonDB())
            {

                SetAgeCategory();
                GenderLabel.Text = user.Runners.First().Gender;

                var runnerRegistrations = db.Registrations
                    .Where(r => r.RunnerId == user.Runners.First().RunnerId)
                    .ToList();

                var results = db.RegistrationEvents
                    .Include(r => r.Registration)
                    .Include(r => r.Event)
                    .Include(r => r.Registration)
                    .Where(r => r.Registration.RunnerId == user.Runners.First().RunnerId)
                    .ToList();
                foreach (var result in results)
                {
                    RegistrationDTO race = new RegistrationDTO();
                    var marathon = db.Marathons
                        .Include(m => m.CountryCodeNavigation)
                        .FirstOrDefault(m => m.MarathonId == result.Event.MarathonId);
                    if (marathon != null)
                    {
                        race.MarathonName = $"{marathon.YearHeld} - {marathon.CityName}, {marathon.CountryCodeNavigation.CountryName}";
                        race.EventType = result.Event.EventName;
                        if (result.RaceTime < 0 || result.RaceTime == null)
                        {
                            race.RaceTime = "N/A";
                            race.OverallRank = "N/A";
                            race.CategoryRank = "N/A";
                        }
                        else
                        {
                            var raceTimeSpan = TimeSpan.FromSeconds((long)result.RaceTime);
                            var raceTimeFormatted = raceTimeSpan.ToString(@"hh\:mm\:ss");
                            race.RaceTime = raceTimeFormatted;

                            var overallRankSelect = db.RegistrationEvents
                                .Where(r => r.EventId == result.EventId && r.RaceTime.HasValue && r.RaceTime > 0)
                                .OrderBy(r => r.RaceTime);
                            List<RankDTO> positions = new List<RankDTO>();
                            for (int i = 1; i <= overallRankSelect.Count(); i++)
                            {
                                var rank = i;
                                if (i > 1)
                                {
                                    var previousPos = positions.ElementAt(i - 2);
                                    var targetPos = overallRankSelect.ElementAt(i - 2);
                                    if (previousPos.Registration.RaceTime == targetPos.RaceTime)
                                    {
                                        rank = previousPos.rank;
                                    } 
                                }
                                positions.Add(new RankDTO(){
                                    rank = rank, 
                                    Registration = overallRankSelect.ElementAt(i - 1) 
                                });
                            }
                            race.OverallRank = "#" + positions.FirstOrDefault(p => p.Registration.RegistrationId == result.RegistrationId).rank.ToString();
                        }
                    }
                    AddResult(race);
                }
            }
        } catch (Exception ex)
        {
            DisplayAlert("Error occurred", "Something went wrong while loading your race results", "Ok");
        }
    }

    private void AddResult(RegistrationDTO race)
    {
        var row = ResultsGrid.RowDefinitions.Count;
        ResultsGrid.AddRowDefinition(new RowDefinition());

        Label marathonLabel = new Label();
        marathonLabel.Text = race.MarathonName;
        Grid.SetRow(marathonLabel, row);
        Grid.SetColumn(marathonLabel, 0);
        ResultsGrid.Children.Add(marathonLabel);

        Label EventLabel = new Label();
        EventLabel.Text = race.EventType;
        Grid.SetRow(EventLabel, row);
        Grid.SetColumn(EventLabel, 1);
        ResultsGrid.Children.Add(EventLabel);

        Label TimeLabel = new Label();
        TimeLabel.Text = race.RaceTime;
        Grid.SetRow(TimeLabel, row);
        Grid.SetColumn(TimeLabel, 2);
        ResultsGrid.Children.Add(TimeLabel);

        Label OverallLabel = new Label();
        OverallLabel.Text = race.OverallRank;
        Grid.SetRow(OverallLabel, row);
        Grid.SetColumn(OverallLabel, 3);
        ResultsGrid.Children.Add(OverallLabel);

        Label CategoryLabel = new Label();
        CategoryLabel.Text = race.CategoryRank;
        Grid.SetRow(CategoryLabel, row);
        Grid.SetColumn(CategoryLabel, 4);
        ResultsGrid.Children.Add(CategoryLabel);
    }

    private void SetAgeCategory()
    {
        DateTime currentDate = DateTime.Now;
        DateTime birthdate = (DateTime)user.Runners.First().DateOfBirth;
        int age = currentDate.Year - birthdate.Year;
        if (birthdate > currentDate.AddYears(-age)) age--;

        switch (age)
        {
            case <= 17:
                AgeCategoryLabel.Text = "Under 18";
                break;
            case >= 18 and <= 29:
                AgeCategoryLabel.Text = "18-29";
                break;
            case >= 30 and <= 39:
                AgeCategoryLabel.Text = "30-39";
                break;
            case >= 40 and <= 55:
                AgeCategoryLabel.Text = "40-55";
                break;
            case >= 56 and <= 70:
                AgeCategoryLabel.Text = "56-70";
                break;
            case > 70:
                AgeCategoryLabel.Text = "Over 70";
                break;
        }
    }

    private void ViewAllRaceResults(object? sender, EventArgs e)
    {
        ShellNavigationQueryParameters userData = new ShellNavigationQueryParameters()
        {
            { "User", user }
        };
        AppShell.Current.GoToAsync("AllRaceResultsPage", userData);
        
    }

    class RankDTO
    {
        public int rank { get; set; }
        public RegistrationEvent Registration { get; set; }
    }
    class RegistrationDTO
    {
        public string MarathonName { get; set; }
        public string EventType { get; set; }
        public string RaceTime { get; set; }
        public string OverallRank { get; set; }
        public string CategoryRank { get; set; }
    }
}