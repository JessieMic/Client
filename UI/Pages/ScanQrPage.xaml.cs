using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Camera.MAUI;
using CommunityToolkit.Maui.Views;
using LogicUnit;
using Objects;
using Objects.Enums;
using UI.Pages.LobbyPages;

namespace UI.Pages;

public partial class ScanQrPage : ContentPage
{
    private readonly LogicManager r_LogicManager;
    private readonly GameInformation r_GameInformation = GameInformation.Instance;

    public ScanQrPage()
    {
        r_LogicManager = new LogicManager();
        InitializeComponent();
    }

    private void cameraView_CamerasLoaded(object sender, EventArgs e)
    {
        if (CameraView.Cameras.Count > 0)
        {
            CameraView.Camera = CameraView.Cameras.First();
            MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await CameraView.StopCameraAsync();
                    await CameraView.StartCameraAsync();
                });
        }
    }

    private void cameraView_BarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
    {
        MainThread.BeginInvokeOnMainThread(() =>
            {
                //barcodeResult.Text = $"{args.Result[0].BarcodeFormat}: {args.Result[0].Text}";
                codeEntered(args.Result[0].Text);
            });
    }

    private async void codeEntered(string i_Code)
    {
        eLoginErrors logicResponse = await r_LogicManager.CheckIfValidCode(i_Code);

        if (logicResponse == eLoginErrors.Ok)
        {
            logicResponse = await r_LogicManager.CheckIfHostLeftForLogin(i_Code);
        }

        switch (logicResponse)
        {
            case eLoginErrors.Ok:
                r_GameInformation.Player.RoomCode = i_Code;
                await Shell.Current.GoToAsync(nameof(EnterNamePage));
                break;

            case eLoginErrors.ServerError or eLoginErrors.RoomClosed:

                MessagePopUp messagePopUp = new(goToMainPage, EnumHelper.GetDescription(logicResponse));
                Application.Current?.Dispatcher.Dispatch(() =>
                    {
                        this.ShowPopup(messagePopUp);
                    });
                break;

        }
    }

    private void goToMainPage()
    {
        if (r_GameInformation.Player.PlayerType == PlayerType.Guest)
        {
            Application.Current.Dispatcher.Dispatch(() => Shell.Current.GoToAsync(".."));
        }
        else
        {
            Application.Current.Dispatcher.Dispatch(() => Shell.Current.GoToAsync("../.."));
        }
    }
}