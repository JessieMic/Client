using Microsoft.AspNetCore.SignalR;

public class GameHub : Hub
{
    public static string[] m_ButtonsThatAreOccupied = new string[4];
    //public static int amountOfPlayersThatAreReady = 0;

    public async Task TryPickAScreenSpot(string nameOfClient, String numberOfButton,string screenWidth, string screenHeight)
    {
        int chosenButtonNumber;
        double width = double.Parse(screenWidth);
        double height = double.Parse(screenHeight);

        if (int.TryParse(numberOfButton, out chosenButtonNumber))
        {
            chosenButtonNumber--;

            if (m_ButtonsThatAreOccupied[chosenButtonNumber] == String.Empty
                || m_ButtonsThatAreOccupied[chosenButtonNumber] == null)
            {
                //Client can pick the spot so we will update all of the clients
                m_ButtonsThatAreOccupied[chosenButtonNumber] = nameOfClient;
                                
                await Clients.All.SendAsync("PlacementUpdateRecevied", nameOfClient,
                chosenButtonNumber, width,height);
                //amountOfPlayersThatAreReady++;
            }

            //if (amountOfPlayersThatAreReady >= 4)
            //{
            //    await Clients.All.SendAsync("GameIsAboutToStart");
            //}
        }
    }

    public async Task TryToDeselectScreenSpot(string nameOfClient, String numberOfpreviousChosenButton,
        String buttonThatClientWantsToDeselect)
    {
        int chosenButtonNumber;

        if (int.TryParse(numberOfpreviousChosenButton, out chosenButtonNumber))
        {
            chosenButtonNumber--;

            if (buttonThatClientWantsToDeselect == nameOfClient)
            {
                m_ButtonsThatAreOccupied[chosenButtonNumber] = string.Empty;
                await Clients.All.SendAsync("DeSelectPlacementUpdatReceived", nameOfClient,
                chosenButtonNumber);
                //amountOfPlayersThatAreReady--;
            }
        }
    }

    public async Task RequestScreenUpdate(string clientId)
    {
        await Clients.Client(clientId).SendAsync("RecieveScreenUpdate", m_ButtonsThatAreOccupied);
    }

    public async Task GameIsAboutToStart()
    {
        m_ButtonsThatAreOccupied[0] = string.Empty;
        m_ButtonsThatAreOccupied[1] = string.Empty;
        m_ButtonsThatAreOccupied[2] = string.Empty;
        m_ButtonsThatAreOccupied[3] = string.Empty;
        await Clients.All.SendAsync("StartGame");
    }


}