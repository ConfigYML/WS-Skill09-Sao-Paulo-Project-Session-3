
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Microsoft.UI.Xaml;
using System.Linq;

namespace Session_3_Dennis_Hilfinger;

public partial class RegisterEventPage : ContentPage, IQueryAttributable
{
    DispatcherTimer timer = new DispatcherTimer();
    User user;
    bool eventSelected = false;
    bool kitSelected = false;
    public RegisterEventPage()
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
        user = query["User"] as User;
        FillSelection();
    }

    private void FillSelection()
    {
        using(var db = new MarathonDB())
        {
            var eventTypes = db.EventTypes.Include(et => et.Events.Where(e => e.MarathonId == 5)).ToList();
            foreach(var type in eventTypes)
            {
                Grid grid = new Grid();
                grid.ColumnDefinitions = new ColumnDefinitionCollection()
                {
                    new ColumnDefinition() { Width = Microsoft.Maui.GridLength.Star},
                    new ColumnDefinition() { Width = new Microsoft.Maui.GridLength(6, Microsoft.Maui.GridUnitType.Star)}
                };
                CheckBox checkbox = new CheckBox();
                checkbox.WidthRequest = 40;
                checkbox.CheckedChanged += EventTypeChecked;
                grid.Children.Add(checkbox);

                Label label = new Label();
                label.Text = $"{type.EventTypeName} (${type.Events.First().Cost})";
                label.HorizontalOptions = LayoutOptions.Center;
                Grid.SetColumn(label, 1);
                grid.Children.Add(label);

                EventTypeCheckboxes.Children.Add(grid);
            }

            var racekits = db.RaceKitOptions.ToList();
            foreach(var kit in racekits)
            {
                RadioButton radio = new RadioButton();
                radio.Value = kit.Cost.ToString().Split(',')[0];
                radio.CheckedChanged += EventTypeChecked;
                radio.GroupName = "RaceKitOptionsGroup";
                radio.Content = $"Option {kit.RaceKitOptionId} (${kit.Cost.ToString().Split(',')[0]}): {kit.RaceKitOption1}";

                RaceKitOptionRadioButtons.Children.Add(radio);
            }

            var charities = db.Charities.ToList();
            foreach(var charity in charities)
            {
                CharityPicker.Items.Add($"{charity.CharityName}");
            }
        }
    }

    private void EventTypeChecked(object? sender, CheckedChangedEventArgs e)
    {
        int value = 0;
        eventSelected = false;
        foreach (var child in EventTypeCheckboxes.Children)
        {
            if (child is Grid grid)
            {
                var checkbox = grid.Children[0] as CheckBox;
                var label = grid.Children[1] as Label;
                if (checkbox.IsChecked)
                {
                    var strValue = label.Text.ToString().Split('$')[1].Split(")")[0].Split(',')[0];
                    int.TryParse(strValue, out int eventTypeValue);
                    value += eventTypeValue;
                    eventSelected = true;
                }

            }
        }
        kitSelected = false;
        RaceKitOptionRadioButtons.Children.ToList().ForEach(radioButton =>
        {
            if (radioButton is RadioButton radio && radio.IsChecked)
            {
                var racekitValue = int.Parse(radio.Value.ToString());
                value += racekitValue;
                kitSelected = true;
            }
        });
        CalculatedValueLabel.Text = $"${value}";
    }

    private void Register(object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(SponsorshipTargetEntry.Text))
            {
                int.TryParse(SponsorshipTargetEntry.Text.ToString(), out int charityValue);
                if (charityValue > 0)
                {
                    if (eventSelected)
                    {
                    if (kitSelected)
                    {
                        if (CharityPicker.SelectedIndex >= 0)
                        {
                            ShellNavigationQueryParameters userData = new ShellNavigationQueryParameters()
                                {
                                    { "User", user }
                                };
                            AppShell.Current.GoToAsync("EventConfirmationPage", userData);
                            using (var db = new MarathonDB())
                            {
                                RadioButton? selectedKit = RaceKitOptionRadioButtons.Children.ToList().First(radioButton =>
                                {
                                    if (radioButton is RadioButton radio && radio.IsChecked)
                                    {
                                        return true;
                                    }
                                    return false;
                                }) as RadioButton;

                                var kitId = selectedKit.Content.ToString().Split("Option ")[1].Split(" (")[0].Trim();
                                RaceKitOption raceKitOption = db.RaceKitOptions.First(rk => rk.RaceKitOptionId == kitId);
                                Charity cha = db.Charities.First(c => c.CharityName == CharityPicker.SelectedItem.ToString());

                                db.Registrations.Add(new Registration
                                {
                                    RunnerId = user.Runners.First().RunnerId,
                                    RegistrationDateTime = DateTime.Now,
                                    Cost = decimal.Parse(CalculatedValueLabel.Text.ToString().Split('$')[1]),
                                    CharityId = cha.CharityId,
                                    Charity = cha,
                                    SponsorshipTarget = decimal.Parse(charityValue.ToString()),
                                    RaceKitOptionId = raceKitOption.RaceKitOptionId,
                                    RaceKitOption = raceKitOption,
                                    RegistrationStatusId = 1
                                });
                                db.SaveChanges();
                            }

                        }
                        else
                        {
                            DisplayAlert("Error", "Please select a charity.", "Ok");
                        }
                    }
                    else
                    {
                        DisplayAlert("Error", "Please select a race kit option.", "Ok");
                    }
                    }
                    else
                    {
                        DisplayAlert("Error", "Please select at least one competition event.", "Ok");
                    }
                }
                else
                {
                    DisplayAlert("Error", "Minimum sponsorship value is $1.", "Ok");
                }
            }
            else
            {
                DisplayAlert("Missing input", "Please provide a sponsorship value", "Ok");
            }
        } catch (Exception ex)
        {
            DisplayAlert("Error", "An error occurred during registration, please try again later", "Ok");
        }
    }

    private void Cancel(object sender, EventArgs e)
    {
        ShellNavigationQueryParameters userData = new ShellNavigationQueryParameters()
        {
            { "User", user }
        };
        AppShell.Current.GoToAsync("RunnerPage", userData);
    }
}