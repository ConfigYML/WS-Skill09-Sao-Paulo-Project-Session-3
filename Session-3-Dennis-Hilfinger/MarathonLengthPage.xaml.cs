using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.UI.Xaml;

namespace Session_3_Dennis_Hilfinger;

public partial class MarathonLengthPage : ContentPage
{
    DispatcherTimer timer = new DispatcherTimer();
    bool SpeedActive = true;

    List<SpeedDistance> sd = new List<SpeedDistance>()
    {
        new SpeedDistance { Name = "F1 car", Value = 345.00 },
        new SpeedDistance { Name = "Slug", Value = 0.01 },
        new SpeedDistance { Name = "Horse", Value = 15.00 },
        new SpeedDistance { Name = "Sloth", Value = 0.12 },
        new SpeedDistance { Name = "Capybara", Value = 35.00 },
        new SpeedDistance { Name = "Jaguar", Value = 80.00 },
        new SpeedDistance { Name = "Worm", Value = 0.03 },
        new SpeedDistance { Name = "Bus", Value = 10.00 },
        new SpeedDistance { Name = "Pair of havaianas", Value = 0.245 },
        new SpeedDistance { Name = "Airbus A380", Value = 73.00 },
        new SpeedDistance { Name = "Football field", Value = 105.00 },
        new SpeedDistance { Name = "Ronaldinho", Value = 1.81 }
    };
    
	public MarathonLengthPage()
	{
        try
        {
            InitializeComponent();
        } catch(Exception ex)
        {
            Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
        }
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

    private void SpeedClicked(object sender, EventArgs e)
    {
        if (Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("ButtonBlue", out object style))
        {
            DistanceBtn.Style = null;
            SpeedBtn.Style = (Microsoft.Maui.Controls.Style) style;
        }
        DistanceLayout.IsVisible = false;
        SpeedLayout.IsVisible = true;
        SpeedActive = true;
    }
    private void DistanceClicked(object sender, EventArgs e)
    {
        if (Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("ButtonBlue", out object style))
        {
            DistanceBtn.Style = (Microsoft.Maui.Controls.Style) style;
            SpeedBtn.Style = null;
        }
        SpeedLayout.IsVisible = false;
        DistanceLayout.IsVisible = true;
        SpeedActive = false;
    }

    private void ItemSelected(object sender, EventArgs e)
    {
        HorizontalStackLayout hor = sender as HorizontalStackLayout;
        if (hor != null)
        {
            Image img = hor.Children[0] as Image;
            MainImage.Source = img.Source;

            Label label = hor.Children[1] as Label;
            HeadingLabel.Text = label.Text;

            if (SpeedActive)
            {
                var speedObj = sd.FirstOrDefault(s => s.Name == label.Text);
                int minutes = (int)(42000.00 / (speedObj.Value * 3.6));
                int hours = minutes / 60;
                int actualMinutes = minutes % 60;
                string commentText = "";
                if (hours > 24)
                {
                    int days = hours / 24;
                    int actualHours = hours % 24;
                    commentText = $"The top speed of a {speedObj.Name} is {speedObj.Value} km/h. It would take {days} days {actualHours} hours and {actualMinutes} minutes to complete a 42km marathon.";
                }
                else
                {
                    commentText = $"The top speed of a {speedObj.Name} is {speedObj.Value} km/h. It would take {hours} hours and {actualMinutes} minutes to complete a 42km marathon.";
                }
                FactLabel.Text = commentText;
            } else
            {
                var distanceObj = sd.FirstOrDefault(d => d.Name == label.Text);
                int amount = (int)(42000.00 / distanceObj.Value);
                if ((42000.00 % distanceObj.Value) != 0.00)
                {
                    amount++;
                }
                string commentText = $"The length of a {distanceObj.Name} is {distanceObj.Value}. It would take {amount} of them to cover the track of a 42km marathon.";
                FactLabel.Text = commentText;
            }
        }
    }
    class SpeedDistance
    {
        public string Name { get; set; }
        public double Value { get; set; }
    }
}