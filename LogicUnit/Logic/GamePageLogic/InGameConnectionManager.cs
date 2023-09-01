using Microsoft.AspNetCore.SignalR.Client;
using Objects;
using Objects.Enums;

namespace LogicUnit.Logic.GamePageLogic;

public class InGameConnectionManager
{
    public readonly HubConnection r_ConnectionToServer;
    public Action<string> ServerError;
    public Notify DisposeEvents;
    public GameInformation GameInfo { get; } = GameInformation.Instance;
    public Queue<SpecialUpdate> SpecialEventQueue { get; }= new Queue<SpecialUpdate>();
    public Queue<SpecialUpdate> SpecialEventWithPointQueue { get; }= new Queue<SpecialUpdate>();
    public eGameStatus GameStatus { get; set; } = eGameStatus.Running;

    private readonly object r_Lock = new();
    public InGameConnectionManager()
    {
        r_ConnectionToServer = new HubConnectionBuilder()
                    .WithUrl(ServerAddressManager.Instance!.InGameHubAddress)
                    .Build();

        r_ConnectionToServer.Reconnecting += (sender) =>
        {
            //DisposeEvents?.Invoke();
            ServerError?.Invoke("Trying to reconnect");
            return Task.CompletedTask;
        };
        

        r_ConnectionToServer.On<int, int>("SpecialUpdateReceived", (int i_WhatHappened, int i_Player) =>
        {
            lock (r_Lock)
            {
                if (GameInfo.Player.PlayerNumber == 1)
                {
                    System.Diagnostics.Debug.WriteLine($"----  Player = {i_Player}---- what {i_WhatHappened} ----  ");
                }
                SpecialEventQueue.Enqueue(new SpecialUpdate(i_WhatHappened, i_Player));
            }
        });

        r_ConnectionToServer.On<int, int, int>("SpecialUpdateWithPointReceived", (i_X, i_Y, i_Player) =>
        {
            lock (r_Lock)
            {
                SpecialEventWithPointQueue.Enqueue(new SpecialUpdate(i_X, i_Y, i_Player));
            }
        });

        r_ConnectionToServer.On<string>("Disconnected", (i_Message) =>
        {
            // r_ConnectionToServer.StopAsync();
            GameStatus = eGameStatus.Exited;
            //DisposeEvents.Invoke();
            ServerError?.Invoke(i_Message);
        });
    }

}