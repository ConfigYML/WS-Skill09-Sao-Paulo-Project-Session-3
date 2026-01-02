namespace Session_3_Dennis_Hilfinger
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SponsorRunnerPage), typeof(SponsorRunnerPage));
            Routing.RegisterRoute(nameof(SponsorshipConfirmationPage), typeof(SponsorshipConfirmationPage));
            Routing.RegisterRoute(nameof(FindOutMorePage), typeof(FindOutMorePage));
            Routing.RegisterRoute(nameof(CharityListPage), typeof(CharityListPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RunnerPage), typeof(RunnerPage));
            Routing.RegisterRoute(nameof(CoordinatorPage), typeof(CoordinatorPage));
            Routing.RegisterRoute(nameof(AdminPage), typeof(AdminPage));
            Routing.RegisterRoute(nameof(CompetedBeforePage), typeof(CompetedBeforePage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(RegisterEventPage), typeof(RegisterEventPage));
            Routing.RegisterRoute(nameof(EditProfilePage), typeof(EditProfilePage));
            Routing.RegisterRoute(nameof(EventConfirmationPage), typeof(EventConfirmationPage));
            Routing.RegisterRoute(nameof(MyRaceResultsPage), typeof(MyRaceResultsPage));
            Routing.RegisterRoute(nameof(AllRaceResultsPage), typeof(AllRaceResultsPage));
        }
    }
}
