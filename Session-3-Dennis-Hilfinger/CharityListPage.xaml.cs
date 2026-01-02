using Microsoft.UI.Xaml;
using System.IO;

namespace Session_3_Dennis_Hilfinger;

public partial class CharityListPage : ContentPage
{
    DispatcherTimer timer = new DispatcherTimer();
    public CharityListPage()
	{
		InitializeComponent();
		OpenPdf();
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

    private async Task OpenPdf()
	{
		// Important! Take a look at this again because I had to look it up.
		var pdfFilename = "charity_descriptions.pdf";
		var targetPath = Path.Combine(FileSystem.CacheDirectory, pdfFilename);
		if (!File.Exists(targetPath))
		{
			using (Stream stream = await FileSystem.OpenAppPackageFileAsync(pdfFilename))
			using (FileStream fileStream = File.Create(targetPath))
			{

				await stream.CopyToAsync(fileStream);
			}
		}

        PdfViewer.Source = targetPath;
    }
}