using System.Globalization;

namespace UI
{
    public partial class App : Application
    {
        public App()
        {
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en-US");
            InitializeComponent();

            MainPage = new AppShell();
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en-US");
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);

            const int k_NewWidth = 600;
            //const int k_NewWidth = 700;
            //const int k_NewWidth = 900;
            const int k_NewHeight = 350;
            //const int k_NewHeight = 400;
            //const int k_NewHeight = 400;

            window.Width = k_NewWidth;
            window.Height = k_NewHeight;

            return window;
        }
    }
}