namespace UI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);

            //const int k_NewWidth = 700;
            const int k_NewWidth = 600;
            //const int k_NewHeight = 400;
            const int k_NewHeight = 350;

            window.Width = k_NewWidth;
            window.Height = k_NewHeight;

            return window;
        }
    }
}