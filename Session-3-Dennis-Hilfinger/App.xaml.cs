using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.UI.Xaml;

namespace Session_3_Dennis_Hilfinger
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        public App()
        {
            InitializeComponent();
        }
        

        protected override Microsoft.Maui.Controls.Window CreateWindow(IActivationState? activationState)
        {
            Microsoft.Maui.Controls.Window window = new Microsoft.Maui.Controls.Window(new AppShell());
            window.MinimumHeight = 700;
            window.MinimumWidth = 1000;
            return window;
        }
    }
}