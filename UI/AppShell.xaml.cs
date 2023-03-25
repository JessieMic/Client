using UI.Pages;

namespace UI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Routing.RegisterRoute(nameof(PartPage), typeof(PartPage)); 
            Routing.RegisterRoute(nameof(EnterNamePage), typeof(EnterNamePage));
            Routing.RegisterRoute(nameof(EnterRoomCodePage), typeof(EnterRoomCodePage));
        }
    }
}