//using System.Threading;
//using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
//using static Java.Util.Jar.Attributes;
//using Microsoft.Maui.Controls;
//using static Java.Util.Jar.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using POC_Client.Objects;

namespace POC_Client.Logic
{
    public delegate void Notify();
    public class ScreenPlacementSelectingLogic
    {
        public event EventHandler<VisualUpdateSelectButtons> UpdateSelectButton;
        public event Notify ReceivedPlayerAmount;
        public event Notify GameIsStarting;
        private readonly HubConnection r_ConnectionToServer;
        private int m_AmountOfPlayers;
        private int[] m_ButtonsThatPlayersPicked;
        ClientInfo m_ClientInfo = ClientInfo.Instance;
        private int m_AmountOfPlayerThatAreConnected;
        
        public ScreenPlacementSelectingLogic()
        {

            r_ConnectionToServer = new HubConnectionBuilder()
                .WithUrl(Utils.m_GameHubAddress)
                .Build();

            r_ConnectionToServer.On<int>
            ("GetAmountOfPlayers", (i_AmountOfPlayers) =>
                {
                    m_AmountOfPlayers = i_AmountOfPlayers;
                    OnReceivedAmountOfPlayers();
                });

            r_ConnectionToServer.On<string[]>("RecieveScreenUpdate", (i_ButtonsThatAreOccupied) =>
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                        {
                            int buttonNumber = 0;
                            VisualUpdateSelectButtons visualUpdate = new();

                            foreach (string element in i_ButtonsThatAreOccupied)
                            {
                                if (i_ButtonsThatAreOccupied[buttonNumber] != String.Empty
                                    && i_ButtonsThatAreOccupied[buttonNumber] != null)
                                {
                                    visualUpdate.Set(buttonNumber, i_ButtonsThatAreOccupied[buttonNumber],true);
                                    OnUpdateButton(visualUpdate);
                                }
                                buttonNumber++;
                            }
                        });
                });

            r_ConnectionToServer.On<string, int>("DeSelectPlacementUpdateReceived", (i_NameOfClientThatDeselected, i_Spot) =>
            {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        VisualUpdateSelectButtons visualUpdate = new(i_Spot, (1 + i_Spot).ToString(), false);

                        if (m_ClientInfo.Name == i_NameOfClientThatDeselected) //(Name.Equals(nameOfClientThatDeselected))
                        {
                            m_ClientInfo.ButtonThatClientPicked = 0;
                            m_ClientInfo.DidClientPickAPlacement = false;
                        }

                        OnUpdateButton(visualUpdate);
                        m_AmountOfPlayerThatAreConnected--;
                    });
            });

            r_ConnectionToServer.On("StartGame", () =>
            {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        OnEnterGameRoom();
                    });
            });

            r_ConnectionToServer.On<string, int>
                ("PlacementUpdateRecevied", (i_NameOfClientThatGotASpot, i_Spot) =>
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                            {
                                VisualUpdateSelectButtons visualUpdate = new(i_Spot,i_NameOfClientThatGotASpot,true);

                                OnUpdateButton(visualUpdate);
                                m_AmountOfPlayerThatAreConnected++;

                                if (m_ClientInfo.Name == i_NameOfClientThatGotASpot)
                                {
                                    m_ClientInfo.ButtonThatClientPicked = 1 + i_Spot;
                                    m_ClientInfo.DidClientPickAPlacement = true;
                                }

                                if ( m_AmountOfPlayerThatAreConnected== m_AmountOfPlayers)
                                {
                                    r_ConnectionToServer.InvokeAsync("GameIsAboutToStart");
                                }
                            });
                    });

            Task.Run(() =>
            {
                Application.Current.Dispatcher.Dispatch(async () =>
                    {
                        await r_ConnectionToServer.StartAsync();
                        await r_ConnectionToServer.InvokeAsync("GetAmountOfPlayers");
                    });
            });

        }

        public void OnUpdateButton(VisualUpdateSelectButtons i_VisualUpdate)
        {
            UpdateSelectButton?.Invoke(this,i_VisualUpdate);
        }

        public void OnReceivedAmountOfPlayers()
        {
            ReceivedPlayerAmount?.Invoke();
        }

        protected virtual void OnEnterGameRoom()
        {
            GameIsStarting?.Invoke();
        }
        public int AmountOfPlayers
        {
            get { return m_AmountOfPlayers; }
            set { m_AmountOfPlayers = value; }
        }


        public int AmountOfPlayerThatAreConnected
        {
            get { return m_AmountOfPlayerThatAreConnected; }
            set { m_AmountOfPlayerThatAreConnected = value; }
        }

        public bool AreAllTheUsersReady()
        {
            bool result = false;

            if (m_AmountOfPlayerThatAreConnected == m_AmountOfPlayers && m_ClientInfo.ButtonThatClientPicked == 1)
            {
                result = true;
            }

            return result;
        }

        public int GetButtonRowValue(int i_ButtonNumber)
        {
            int returnValue = 2;

            if (i_ButtonNumber == 1)
            {
                returnValue = 1;
            }
            else if(AmountOfPlayers == 2)
            {
                returnValue = 2;
            }
            else if (m_AmountOfPlayers <= 4)
            {
                if (i_ButtonNumber < 3)
                {
                    returnValue = 1;
                }
            }
            else if (i_ButtonNumber < 4)
            {
                returnValue = 1;
            }

            return returnValue;
        }

        public int GetButtonColumnValue(int i_ButtonNumber)
        {
            int returnValue = 1;

            if (i_ButtonNumber == 2)
            {
                if (m_AmountOfPlayers > 2)
                {
                    returnValue = 2;
                }
            }
            else if (i_ButtonNumber == 3)
            {
                if (m_AmountOfPlayers > 4)
                {
                    returnValue = 3;
                }
            }
            else if (i_ButtonNumber == 4)
            {
                if (AmountOfPlayers == 4)
                {
                    returnValue = 2;
                }
            }
            else if (i_ButtonNumber == 5)
            {
                returnValue = 2;
            }
            else if (i_ButtonNumber == 6)
            {
                returnValue = 3;
            }

            return returnValue;
        }

        public async Task GetScreenUpdate()
        {
            await r_ConnectionToServer.InvokeCoreAsync("RequestScreenUpdate", args: new[]
                {r_ConnectionToServer.ConnectionId});
        }

        public async Task TryPickAScreenSpot(string i_TextOnButton)
        {
            await r_ConnectionToServer.InvokeCoreAsync("TryPickAScreenSpot", args: new[]
                {m_ClientInfo.Name,i_TextOnButton});
        }

        public async Task TryToDeselectScreenSpot(string i_TextOnButton)
        {
            await r_ConnectionToServer.InvokeCoreAsync("TryToDeselectScreenSpot", args: new[]
                {m_ClientInfo.Name,m_ClientInfo.ButtonThatClientPicked.ToString(),i_TextOnButton});
        }
    }
}

