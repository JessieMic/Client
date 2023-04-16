﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace POC_Client;

[Activity(Theme = "@style/Maui.SplashTheme",
    ScreenOrientation = ScreenOrientation.Landscape,
    MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        Platform.Init(this, savedInstanceState);

        this.Window.AddFlags(WindowManagerFlags.Fullscreen);
    }
}
