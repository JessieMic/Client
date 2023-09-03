using Microsoft.AspNetCore.SignalR;
using System.Drawing;
using DTOs;
using GameRoomServer;

public class GameHub : Hub
{
    public static string[] buttonsThatAreOccupied = new string[4];
    public static int[] screenSizeWidth = new int[4];
    public static int[] screenSizeHeight = new int[4];
    public static double[] density = new double[4];
    private readonly int[] r_PlayersPressedButtons = new int[4];

    public async Task TryPickAScreenSpot(string i_NameOfPlayer, String i_NumberOfButton,int i_ScreenWidth, int i_ScreenHeight,double i_Density)
    {
        int chosenButtonNumber;

        if (int.TryParse(i_NumberOfButton, out chosenButtonNumber))
        {
            chosenButtonNumber--;

            if (buttonsThatAreOccupied[chosenButtonNumber] == String.Empty
                || buttonsThatAreOccupied[chosenButtonNumber] == null)
            {
                //Player can pick the spot so we will update all of the Players
                buttonsThatAreOccupied[chosenButtonNumber] = i_NameOfPlayer;
                screenSizeWidth[chosenButtonNumber] = i_ScreenWidth;
                screenSizeHeight[chosenButtonNumber] = i_ScreenHeight;
                density[chosenButtonNumber] = i_Density;
                await Clients.All.SendAsync("PlacementUpdateRecevied", i_NameOfPlayer,
                chosenButtonNumber);
            }
        }
    }

    public async Task TryToDeselectScreenSpot(string nameOfPlayer, String numberOfpreviousChosenButton,
        String buttonThatPlayerWantsToDeselect)
    {
        int chosenButtonNumber;

        if (int.TryParse(numberOfpreviousChosenButton, out chosenButtonNumber))
        {
            chosenButtonNumber--;

            if (buttonThatPlayerWantsToDeselect == nameOfPlayer)
            {
                buttonsThatAreOccupied[chosenButtonNumber] = string.Empty;
                await Clients.All.SendAsync("DeSelectPlacementUpdateReceived", nameOfPlayer,
                chosenButtonNumber);
            }
        }
    }

    public async Task RequestScreenUpdate(string PlayerId)
    {
        await Clients.Client(PlayerId).SendAsync("RecieveScreenUpdate", buttonsThatAreOccupied);
    }

    public async Task GameIsAboutToStart()
    {
        await Clients.All.SendAsync("StartGame", buttonsThatAreOccupied, screenSizeWidth, screenSizeHeight, density);
    }

    public void ResetHub()
    {
        buttonsThatAreOccupied = new string[4];
    }

    public async Task UpdatePlayerSelection(int i_PlayerID, int i_button, int i_X, int i_Y)
    {
        if(i_button != 0)
        {
            r_PlayersPressedButtons[i_PlayerID] = i_button;
        }
        else
        {
            await Clients.All.SendAsync("GameUpdateReceived", i_PlayerID, i_button, i_X, i_Y);
        }
    }

    public async Task GetPlayersData()
    {
        await Clients.Caller.SendAsync("GameStateReceived", r_PlayersPressedButtons);
    }
}