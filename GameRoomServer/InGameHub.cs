
using Microsoft.AspNetCore.SignalR;
using System.Drawing;
using DTOs;
using GameRoomServer;
namespace GameRoomServer;

public class InGameHub : Hub
{
    private static int[,] r_PlayersPressedButtons = new int[4,3];


    public async Task UpdatePlayerSelection(int i_PlayerID, int i_button, int i_X, int i_Y)
    {

        Console.WriteLine($"{i_PlayerID} sent {i_button}");
        //if (i_button != 0)
        //{
        r_PlayersPressedButtons[i_PlayerID, 0] = i_button;
        r_PlayersPressedButtons[i_PlayerID, 1] = i_X;
        r_PlayersPressedButtons[i_PlayerID, 2] = i_Y;



        //}
        //else
        //{
        //  await Clients.All.SendAsync("GameUpdateReceived", i_PlayerID, i_button, i_X, i_Y);
        //}
    }

    public int[,] GetPlayersData()
    {
        //Console.WriteLine("got an update");

        return r_PlayersPressedButtons;
        //await Clients.All.SendAsync("GetPlayersData", r_PlayersPressedButtons);
        //await Clients.Caller.SendAsync("GameStateReceived", r_PlayersPressedButtons);
    }
}