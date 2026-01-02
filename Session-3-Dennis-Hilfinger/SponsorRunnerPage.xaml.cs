using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.UI.Xaml;
using System.Linq;
using System.Threading.Tasks;
using Application = Microsoft.Maui.Controls.Application;

namespace Session_3_Dennis_Hilfinger;

public partial class SponsorRunnerPage : ContentPage
{
    DispatcherTimer timer = new DispatcherTimer();
    public SponsorRunnerPage()
	{
		InitializeComponent();
		PopulateRunnersDropDown();
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

    private void PopulateRunnersDropDown()
	{
		using (var db = new MarathonDB())
		{
			var runners = db.Runners.Include(run => run.EmailNavigation).OrderBy(r => r.RunnerId).ToList();
			foreach (var runner in runners)
			{
				RunnerPicker.Items.Add($"{runner.EmailNavigation.FirstName} {runner.EmailNavigation.LastName} - {runner.RunnerId} ({runner.CountryCode})");
			}
		}
	}

	private void MoreCharityInfo(object sender, EventArgs e)
	{
		Microsoft.Maui.Controls.Window win = new Microsoft.Maui.Controls.Window(new CharityInfoPage());
		Application.Current.OpenWindow(win);
    }


    private async void ChangeValue(object sender, EventArgs e)
	{
		AmountLabel.Text = $"$ {(int)AmountStepper.Value}";
	}
	private async void SetValue(object sender, EventArgs e)
	{
		int SetValue;
		int.TryParse(AmountEntry.Text, out SetValue);
		if (SetValue > 0 && SetValue <= 10000)
		{
			AmountStepper.Value = SetValue;
		} else
		{
			await DisplayAlert("Invalid Amount", "Please enter a valid amount between 1 and 10,000", "OK");
        }
    }

	private async void PayNow(object sender, EventArgs e)
	{
		if (!String.IsNullOrEmpty(NameEntry.Text)
			&& RunnerPicker.SelectedIndex >= 0
			&& !String.IsNullOrEmpty(CardOwnerEntry.Text)
			&& CheckCardNumber()
			&& ExpiryDatePicker.Date.Date >= DateTime.Now.Date
			&& CheckCvCNumber())
		{
			string SelectedRunner = RunnerPicker.SelectedItem.ToString();
			int RunnerId = int.Parse(SelectedRunner.Split('-')[1].Split('(')[0].Trim());
			var navigationParams = new ShellNavigationQueryParameters()
			{
				{ "RunnerId", RunnerId },
				{ "Amount", (int)AmountStepper.Value }
			};
			await Shell.Current.GoToAsync("SponsorshipConfirmationPage", false, navigationParams);
        } else
		{
			await DisplayAlert("Form incomplete", "Please fill out the entire form correctly!", "Ok");
		}
	}

    private bool CheckCardNumber()
    {
        string cardNumber = CardNumberEntry.Text;
        if (string.IsNullOrEmpty(cardNumber))
        {
            DisplayAlert("Problem with card number", "Card number cannot be empty", "Ok");
            return false;
        }
        if (cardNumber.Length == 16)
        {
            foreach (char digit in cardNumber)
            {
                if (!Char.IsDigit(digit))
                {
                    DisplayAlert("Problem with card number", "Card number consists only of digits", "Ok");
                    return false;
                }
            }
            return true;
        }
        DisplayAlert("Problem with card number", "Card number needs 16 digits", "Ok");
        return false;
    }

    private bool CheckCvCNumber()
	{
		string cvc = CvcEntry.Text;
		if (string.IsNullOrEmpty(cvc))
		{
			DisplayAlert("Problem with CVC", "CVC number cannot be empty", "Ok");
			return false;
        }
        if (cvc.Length == 3)
		{
			foreach (char digit in cvc)
			{
				if (!Char.IsDigit(digit))
                {
                    DisplayAlert("Problem with CVC", "CVC number consists only of digits", "Ok");
                    return false;
				}
			}
			return true;
        }
        DisplayAlert("Problem with CVC", "CVC number needs 3 digits", "Ok");
        return false;
    }

	private void Cancel(object sender, EventArgs e)
	{
		Navigation.PopAsync();
    }
}