using Microsoft.AspNetCore.SignalR;
using System.Drawing;
using DTOs;

public class GameHub : Hub
{
    public static string[] buttonsThatAreOccupied = new string[4];
    public static int[] screenSizeWidth = new int[4];
    public static int[] screenSizeHeight = new int[4];

    public async Task TryPickAScreenSpot(string i_NameOfPlayer, String i_NumberOfButton,int i_ScreenWidth, int i_ScreenHeight)
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
        await Clients.All.SendAsync("StartGame", buttonsThatAreOccupied, screenSizeWidth, screenSizeHeight);
        buttonsThatAreOccupied[0] = string.Empty;
        buttonsThatAreOccupied[1] = string.Empty;
        buttonsThatAreOccupied[2] = string.Empty;
        buttonsThatAreOccupied[3] = string.Empty;
        buttonsThatAreOccupied[4] = string.Empty;
        buttonsThatAreOccupied[5] = string.Empty;
    }
}