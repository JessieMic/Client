using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.Activity;
using AndroidX.Browser.Trusted;
using ScreenOrientation = Android.Content.PM.ScreenOrientation;

namespace UI
{
    [Activity(
        Theme = "@style/Maui.SplashTheme",
        ScreenOrientation = ScreenOrientation.Landscape,
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode
                               | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            OnBackPressedDispatcher.AddCallback(this, new BackPress());

            this.Window.AddFlags(WindowManagerFlags.Fullscreen);
        }

        public override bool DispatchKeyEvent(KeyEvent key)
        {
            if(key.KeyCode == Keycode.Back && key.Action ==KeyEventActions.Down)
            {
                return false;
            }

            return base.DispatchKeyEvent(key);
        }
    }

    public class BackPress : OnBackPressedCallback
    {
        public BackPress()
            : base(true)
        {
        }

        public override void HandleOnBackPressed()
        {
            //throw new NotImplementedException();
            System.Diagnostics.Debug.WriteLine("Back button pressed");
        }
    }


}