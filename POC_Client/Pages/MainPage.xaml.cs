﻿using System.Threading;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.VisualBasic;

namespace POC_Client;

public partial class MainPage : ContentPage
{
    private readonly HubConnection r_Connection;
	public MainPage()
	{
        InitializeComponent();
 
    }

    async void OnReadyClicked(object sender, EventArgs e)
    {   
        if (!entry.Text.Equals(string.Empty))
        {
            await Shell.Current.GoToAsync("ScreenPlacementSelectingPage");//$"SelectPhonePlacementPage?name={DateAndTime.TimeString}");
        }
    }

    void OnEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        string oldText = e.OldTextValue;
        string newText = e.NewTextValue;
        string myText = entry.Text;
    }

    void OnEntryCompleted(object sender, EventArgs e)
    {
        string text = ((Entry)sender).Text;
    }
}

