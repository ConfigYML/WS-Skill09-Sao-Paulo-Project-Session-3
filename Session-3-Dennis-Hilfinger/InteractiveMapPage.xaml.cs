using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Session_3_Dennis_Hilfinger;

public partial class InteractiveMapPage : ContentPage
{
    DispatcherTimer timer = new DispatcherTimer();
    List<Checkpoint> checkpoints = new List<Checkpoint>();
	public InteractiveMapPage()
	{
		InitializeComponent();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += timerTick;
        timer.Start();

        AddCheckpoints();
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

    private void CheckpointClicked(object? sender, EventArgs e)
    {
        Microsoft.Maui.Controls.Button checkBtn = sender as Microsoft.Maui.Controls.Button;
        var selected = checkpoints.FirstOrDefault(c => c.Btn.Id == checkBtn.Id);
        CheckpointLabel.Text = $"Checkpoint {selected.CheckpointNumber}: {selected.Name}";
        ImageLayout.Children.Clear();
        foreach (var service in selected.ServicesProvided)
        {
            Microsoft.Maui.Controls.Image img = new Microsoft.Maui.Controls.Image();
            img.Source = ImageSource.FromFile($"map_icon_{service}.png");
            img.WidthRequest = 100;
            img.HeightRequest = 100;
            img.HorizontalOptions = LayoutOptions.Start;
            ImageLayout.Children.Add(img);
        }
    }

    private void AddCheckpoints()
    {
        string[] names = { 
            "Avenida Rudge", 
            "Theatro Municipal", 
            "Parque do Ibirapuera", 
            "Jardim Luzitania", 
            "Iguatemi",
            "Rua Lisboa",
            "Cemitério da Consolação 1",
            "Cemitério da Consolação 2"
        };
        for (int i = 1; i <= 8; i++)
        {
            Microsoft.Maui.Controls.Button ch = FindByName($"Checkpoint_{i}") as Microsoft.Maui.Controls.Button;
            checkpoints.Add(new Checkpoint()
            {
                CheckpointNumber = i,
                Btn = ch,
                Name = names[i - 1],
                ServicesProvided = new List<string>()
            });
        }

        checkpoints[0].ServicesProvided = new List<string> { "drinks", "energy_bars" };
        checkpoints[1].ServicesProvided = new List<string> { "drinks", "energy_bars", "information", "medical", "toilets" };
        checkpoints[2].ServicesProvided = new List<string> { "drinks", "energy_bars", "toilets" };
        checkpoints[3].ServicesProvided = new List<string> { "drinks", "energy_bars", "medical", "toilets" };
        checkpoints[4].ServicesProvided = new List<string> { "drinks", "energy_bars", "information", "toilets" };
        checkpoints[5].ServicesProvided = new List<string> { "drinks", "energy_bars", "toilets" };
        checkpoints[6].ServicesProvided = new List<string> { "drinks", "energy_bars", "information", "medical", "toilets" };
        checkpoints[7].ServicesProvided = new List<string> { "drinks", "energy_bars", "information", "medical", "toilets" };
    }

    class Checkpoint()
    {
        public int CheckpointNumber { get; set; }
        public Microsoft.Maui.Controls.Button Btn { get; set; }
        public string Name { get; set; }
        public List<string> ServicesProvided { get; set; }
    }
}