using Microsoft.Maui.LifecycleEvents;
using Microsoft.UI.Xaml;
using System.ComponentModel.Design;
using Windows.ApplicationModel.Contacts;

namespace Session_3_Dennis_Hilfinger;

public partial class RegisterPage : ContentPage
{
    DispatcherTimer timer = new DispatcherTimer();
	public RegisterPage()
	{
		InitializeComponent();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += timerTick;
        timer.Start();
        FillSelections();
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

    private void FillSelections()
    {
        using (var db = new MarathonDB())
        {
            var countries = db.Countries.ToList();
            foreach(var c in countries)
            {
                CountryPicker.Items.Add($"{c.CountryName} - {c.CountryCode}");
            }
            var genders = db.Genders.ToList();
            foreach (var g in genders)
            {
                GenderPicker.Items.Add(g.Gender1);
            }
        }
    }

    private void Register(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(EmailEntry.Text) &&
            !String.IsNullOrEmpty(PasswordEntry.Text) &&
            !String.IsNullOrEmpty(PasswordAgainEntry.Text) &&
            !String.IsNullOrEmpty(FirstnameEntry.Text) &&
            !String.IsNullOrEmpty(LastnameEntry.Text) &&
            GenderPicker.SelectedIndex >= 0 &&
            CountryPicker.SelectedIndex >= 0)
        {
            if (CheckMail())
            {
                if (PasswordAgainEntry.Text.ToString() == PasswordEntry.Text.ToString())
                {
                    if (CheckPasswordRequirements() && CheckBirthdate())
                    {
                        //Register user
                        try
                        {
                            using (var db = new MarathonDB())
                            {
                                var genders = db.Genders.ToList();
                                var userGender = genders.FirstOrDefault(g => g.Gender1 == GenderPicker.SelectedItem.ToString());
                                var countries = db.Countries.ToList();
                                var userCountry = countries.FirstOrDefault(c => c.CountryName + " - " + c.CountryCode == CountryPicker.SelectedItem.ToString());
                                var roles = db.Roles.ToList();
                                var userRole = roles.FirstOrDefault(r => r.RoleId == "R");
                                var userToRegister = new User
                                {
                                    Email = EmailEntry.Text.ToString(),
                                    Password = PasswordEntry.Text.ToString(),
                                    FirstName = FirstnameEntry.Text.ToString(),
                                    LastName = LastnameEntry.Text.ToString(),
                                    Role = userRole,
                                    Runners = new List<Runner>
                                    {
                                        new Runner
                                        {
                                            Email = EmailEntry.Text.ToString(),
                                            DateOfBirth = BirthdatePicker.Date,
                                            GenderNavigation = userGender,
                                            Gender = userGender.Gender1,
                                            CountryCodeNavigation = userCountry,
                                            CountryCode = userCountry.CountryCode,
                                        }
                                    }
                                };
                                db.Users.Add(userToRegister);
                                db.SaveChanges();
                                var userData = new ShellNavigationQueryParameters()
                                {
                                    { "User", userToRegister }
                                };
                                DisplayAlert("Registration successful!", "User was registered", "Ok");
                                AppShell.Current.GoToAsync("RegisterEventPage", userData);
                            }

                        }
                        catch
                        {
                            DisplayAlert("Error", "An error occurred while registering. Please try again.", "OK");
                            return;
                        }
                    }
                }
                else
                {
                    DisplayAlert("Error", "Passwords do not match.", "OK");
                }
            }
            else
            {
                DisplayAlert("Error", "Please enter a valid email address.", "OK");
            }

        }
        else
        {
            DisplayAlert("Error", "Please fill in all fields.", "OK");
        }
    }

    private bool CheckMail()
    {
        string email = EmailEntry.Text.ToString();
        string[] at_split = email.Split('@');
        if (at_split.Length != 2)
        {
            return false;   
        }
        if (at_split[0].Trim().Length == 0 || at_split[0].Trim().Length == 0)
        {
            return false;
        }
        string[] domain_split = at_split[1].Split('.');
        if (domain_split.Length == 2)
        {
            if(domain_split[0].Trim().Length == 0 || domain_split[1].Trim().Length == 0)
            {
                return false;
            }
            return true;
        }
        return false;  
    }

    private bool CheckPasswordRequirements()
    {
               string password = PasswordEntry.Text.ToString();
        if (password.Length < 6)
        {
            DisplayAlert("Error", "Password must be at least 6 characters long.", "OK");
            return false;
        }
        if (!password.Any(char.IsUpper))
        {
            DisplayAlert("Error", "Password must contain at least one uppercase letter.", "OK");
            return false;
        }
        if (!password.Any(char.IsLower))
        {
            DisplayAlert("Error", "Password must contain at least one lowercase letter.", "OK");
            return false;
        }
        if (!password.Any(char.IsDigit))
        {
            DisplayAlert("Error", "Password must contain at least one digit.", "OK");
            return false;
        }
        var specialCharacters = "!@#$%^";
        if (!password.Any(ch => specialCharacters.Contains(ch)))
        {
            DisplayAlert("Error", "Password must contain at least one special character.", "OK");
            return false;
        }
        return true;
    }

    private bool CheckBirthdate()
    {
        DateTime birthdate = BirthdatePicker.Date;
        DateTime currentDate = DateTime.Now;
        int age = currentDate.Year - birthdate.Year;
        if (birthdate > currentDate.AddYears(-age)) age--;
        if (age < 10)
        {
            DisplayAlert("Error", "You must be at least 10 years old to register.", "OK");
            return false;
        }
        return true;
    }

    private void Cancel(object sender, EventArgs e)
    {
        AppShell.Current.GoToAsync("//MainPage");
    }
}