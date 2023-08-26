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
        //Console.WriteLine($"{i_PlayerID} sent {i_button}");
        if (i_button != -1)
        {
            s_PlayersPressedButtons[i_PlayerID] = i_button;
        }

        if (i_X != -1)
        {
            s_PlayersPressedButtons[i_PlayerID + 4] = i_X;
            s_PlayersPressedButtons[i_PlayerID + 8] = i_Y;
        }
        //await Clients.All.SendAsync("GameUpdateReceived", i_PlayerID, i_button, i_X, i_Y);
    }

    public async Task SpecialUpdate(int i_WhatHappened, int i_PlayerID)
    {
        await Clients.All.SendAsync("SpecialUpdateReceived", i_WhatHappened, i_PlayerID);
    }

    public async Task SpecialUpdateWithPoint(int i_X, int i_Y, int i_PlayerID)
    {
        await Clients.All.SendAsync("SpecialUpdateWithPointReceived", i_X, i_Y, i_PlayerID);
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

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await Clients.All.SendAsync("Disconnected", "an error occurred");
        await base.OnDisconnectedAsync(exception);
    }
}