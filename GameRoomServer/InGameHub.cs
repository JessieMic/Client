
using Microsoft.AspNetCore.SignalR;
using System.Drawing;
using DTOs;
using GameRoomServer;
namespace GameRoomServer;

public class InGameHub : Hub
{
    private static int[] s_PlayersPressedButtons = new int[12];

    public async Task UpdatePlayerSelection(int i_PlayerID, int i_button, int i_X, int i_Y)
    {

        Console.WriteLine($"{i_PlayerID} sent {i_button}");
        //if (i_button != 0)
        //{
        s_PlayersPressedButtons[i_PlayerID] = i_button;
        s_PlayersPressedButtons[i_PlayerID + 4] = i_X;
        s_PlayersPressedButtons[i_PlayerID + 8] = i_Y;

        //}
        //else
        //{
        //await Clients.All.SendAsync("GameUpdateReceived", i_PlayerID, i_button, i_X, i_Y);
        //}
    }

    public int[] GetPlayersData()
    {
        //Console.WriteLine("got an update");

        return s_PlayersPressedButtons;
        //await Clients.All.SendAsync("GetPlayersData", r_PlayersPressedButtons);
        //await Clients.Caller.SendAsync("GameStateReceived", r_PlayersPressedButtons);
    }

    public void ResetHub()
    {
        s_PlayersPressedButtons = new int[12];
    }

    public DateTime Ping()
    {
        return DateTime.Now;
    }
}
