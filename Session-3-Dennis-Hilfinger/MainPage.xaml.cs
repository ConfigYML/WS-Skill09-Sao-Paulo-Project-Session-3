using Microsoft.UI.Xaml;
using System.Threading.Tasks;

namespace Session_3_Dennis_Hilfinger
{
    public partial class MainPage : ContentPage
    {
        DispatcherTimer timer = new DispatcherTimer();
        public MainPage()
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

        private async void ImportStaffData(object sender, EventArgs e)
        {
            try
            {
                GetStaffDataFromFile();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Error occurred while importing staff data", "Ok");
            }
        }

        private async void GetStaffDataFromFile()
        {

            var file = await FilePicker.PickAsync(PickOptions.Default);
            if (file != null)
            {
                var filePath = file.FullPath;
                if (filePath.Trim().EndsWith(".csv"))
                {
                    var lines = File.ReadAllLines(filePath);
                    if (lines[0].Split(';').Length == 10)
                    {

                        var staffDataList = lines.ToList();
                        staffDataList.RemoveAt(0);

                        using (var db = new MarathonDB())
                        {

                            foreach (var line in staffDataList)
                            {
                                var data = line.Split(';');

                                var names = data[1].Replace('*', ' ').Replace('#', ' ').Replace('|', ' ');
                                var splittedName = names.Split(' ');
                                var firstname = splittedName[0].Trim();
                                var lastname = splittedName[splittedName.Length - 1].Trim();

                                var gender = data[3];
                                if (data[3].Trim().ToLower().StartsWith("fema"))
                                    gender = "F";
                                else if (data[3].Trim().ToLower().StartsWith("male"))
                                    gender = "M";

                                var period = "Y";
                                if (data[7].Trim().ToLower().StartsWith("h"))
                                {
                                    period = "H";
                                }

                                var staffPositionExists = db.StaffPositions.Any(p => p.PositionId == int.Parse(data[4]));
                                if (!staffPositionExists)
                                {
                                    db.StaffPositions.Add(new StaffPosition()
                                    {
                                        PositionId = int.Parse(data[4]),
                                        PositionName = data[5],
                                        PositionDescription = data[6],
                                        PayPeriod = period,
                                        PayRate = data[8].Replace('$', ' '),
                                    });
                                    db.SaveChanges();
                                }

                                var staffExists = db.Staff.Any(s => s.StaffId == int.Parse(data[0]));
                                if (!staffExists)
                                {
                                    db.Staff.Add(new Staff()
                                    {
                                        StaffId = int.Parse(data[0]),
                                        Firstname = firstname,
                                        Lastname = lastname,
                                        DateOfBirth = DateOnly.Parse(data[2]),
                                        Gender = gender,
                                        PositionId = int.Parse(data[4]),
                                        Email = data[9],
                                    });

                                    db.SaveChanges();
                                }
                            }
                        }

                    }
                    else if (lines[0].Split(';').Length == 5)
                    {
                        var timesheetDataList = lines.ToList();
                        timesheetDataList.RemoveAt(0);
                        using (var db = new MarathonDB())
                        {
                            List<StaffTimesheet> timesheets = db.StaffTimesheets.ToList();
                            foreach (var line in timesheetDataList)
                            {
                                var data = line.Split(';');

                                var timesheet = new StaffTimesheet()
                                {
                                    TimesheetId = int.Parse(data[0]),
                                    StaffId = int.Parse(data[1]),
                                    StartDateTime = DateTime.Parse(data[2]),
                                    EndDateTime = DateTime.Parse(data[3]),
                                    PayAmount = data[4]
                                };

                                if (!timesheets.Contains(timesheet))
                                {
                                    timesheets.Add(timesheet);
                                    db.StaffTimesheets.Add(timesheet);
                                }

                            }
                            db.SaveChanges();
                        }

                    }
                    else
                    {
                        await DisplayAlert("Incorrect format", "Data was not in correct format to import staff or staff timesheet data. View README for more info.", "Ok");
                        return;
                    }

                }
                else
                {
                    await DisplayAlert("Wrong file selected", "Please select a .csv file for importing staff data!", "Close");
                    return;
                }
            }
        }


        private async void BecomeRunner(object sender, EventArgs e)
        {
            AppShell.Current.GoToAsync("CompetedBeforePage");
        }

        private async void SponsorRunner(object sender, EventArgs e)
        {
            AppShell.Current.GoToAsync("SponsorRunnerPage");
        }

        private async void FindOutMore(object sender, EventArgs e)
        {
            AppShell.Current.GoToAsync("FindOutMorePage");
        }

        private async void Login(object sender, EventArgs e)
        {
            AppShell.Current.GoToAsync("LoginPage");
        }
    }
}
