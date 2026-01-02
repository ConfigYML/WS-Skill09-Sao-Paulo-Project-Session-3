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
                    .ToList();
                foreach (var registration in results)
                {
                    if (!runnerRegistrations.Any(r => r.RegistrationId == registration.RegistrationId))
                    {
                        results.Remove(registration);
                    }
                }
                
                List<RegistrationDTO> races = new List<RegistrationDTO>();
                foreach (var result in results)
                {
                    RegistrationDTO race = new RegistrationDTO();
                    var marathon = db.Marathons
                        .Include(m => m.CountryCodeNavigation)
                        .FirstOrDefault(m => m.MarathonId == result.Event.MarathonId);
                    if (marathon != null)
                    {
                        race.MarathonName = $"{marathon.YearHeld} - {marathon.CityName}, {marathon.CountryCodeNavigation.CountryName}";
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
                            var positions = new Dictionary<int, RegistrationEvent>();
                            for (int i = 1; i <= overallRankSelect.Count(); i++)
                            {
                                var rank = i;
                                if (i > 1)
                                {
                                    var previousPos = positions.FirstOrDefault(p => p.Key == i - 1);
                                    if (previousPos.Value.RaceTime == overallRankSelect.ElementAt(i - 1).RaceTime)
                                    {
                                        rank = previousPos.Key;
                                    }
                                }
                                positions.Add(rank, overallRankSelect.ElementAt(i - 1));
                            }
                            race.OverallRank = "#" + positions.FirstOrDefault(p => p.Value.RegistrationEventId == result.RegistrationEventId).Key.ToString();
                        }
                    }
                }
            }
        } catch (Exception ex)
        {
            DisplayAlert("Error occurred", "Something went wrong while loading your race results", "Ok");
        }
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

    class RegistrationDTO
    {
        public string MarathonName { get; set; }
        public string EventType { get; set; }
        public string RaceTime { get; set; }
        public string OverallRank { get; set; }
        public string CategoryRank { get; set; }
    }
}