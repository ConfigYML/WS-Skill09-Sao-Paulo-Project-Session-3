using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;
using System.Reflection.Metadata;
using WinRT.Session_3_Dennis_HilfingerVtableClasses;

namespace Session_3_Dennis_Hilfinger;

public partial class EditProfilePage : ContentPage, IQueryAttributable
{
    DispatcherTimer timer = new DispatcherTimer();
    User? user;
    public EditProfilePage()
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
        FillData();
    }

    private void FillData()
    {
        using (var db = new MarathonDB())
        {
            var countries = db.Countries.ToList();
            foreach (var c in countries)
            {
                CountryPicker.Items.Add($"{c.CountryName} - {c.CountryCode}");
            }

            var countryIndex = countries.FindIndex(c => c.CountryCode == user.Runners.First().CountryCode);
            CountryPicker.SelectedIndex = countryIndex;

            var genders = db.Genders.ToList();
            foreach (var g in genders)
            {
                GenderPicker.Items.Add(g.Gender1);
            }

            var genderIndex = genders.FindIndex(g => g.Gender1 == user.Runners.First().Gender);
            GenderPicker.SelectedIndex = genderIndex;

            BirthdatePicker.Date = (DateTime) user.Runners.First().DateOfBirth;

            EmailLabel.Text = user.Email;
        }
    }

    private void SaveData(object sender, EventArgs e)
    {
        using(var db = new MarathonDB())
        {
            if (!String.IsNullOrEmpty(FirstnameEntry.Text))
            {
                user.FirstName = FirstnameEntry.Text.ToString();
            }
            if (!String.IsNullOrEmpty(LastnameEntry.Text))
            {
                user.LastName = LastnameEntry.Text.ToString();
            }

            var genders = db.Genders.ToList();
            var userGender = genders.FirstOrDefault(g => g.Gender1 == GenderPicker.SelectedItem.ToString());
            var countries = db.Countries.ToList();
            var userCountry = countries.FirstOrDefault(c => c.CountryName + " - " + c.CountryCode == CountryPicker.SelectedItem.ToString());

            if (CheckBirthdate())
            {
                user.Runners.First().DateOfBirth = BirthdatePicker.Date;
            } else
            {
                return;
            }
            user.Runners.First().GenderNavigation = userGender;
            user.Runners.First().Gender = userGender.Gender1;
            user.Runners.First().CountryCodeNavigation = userCountry;
            user.Runners.First().CountryCode = userCountry.CountryCode;

            if (!String.IsNullOrEmpty(PasswordEntry.Text) ||
                !String.IsNullOrEmpty(PasswordAgainEntry.Text))
            {
                if (PasswordAgainEntry.Text.ToString() == PasswordEntry.Text.ToString())
                {
                    if (CheckPasswordRequirements())
                    {
                        user.Password = PasswordEntry.Text.ToString();
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    DisplayAlert("Error", "Passwords do not match.", "OK");
                    return;
                }
                } else if (!(String.IsNullOrEmpty(PasswordEntry.Text) && String.IsNullOrEmpty(PasswordAgainEntry.Text)))
            {
                DisplayAlert("Error", "Please enter a value for both password fields to change your password.", "OK");
                return;
            }
            db.Update(user);
            db.SaveChanges();
            DisplayAlert("Success", "Profile updated successfully.", "OK");
        }
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
        ShellNavigationQueryParameters userData = new ShellNavigationQueryParameters()
        {
            { "User", user }
        };
        AppShell.Current.GoToAsync("RunnerPage", userData);
    }

}