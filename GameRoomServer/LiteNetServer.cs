using System.Timers;
using LiteNetLib;
using LiteNetLib.Utils;
using LogicUnit.Logic.GamePageLogic;
using Timer = System.Timers.Timer;


namespace GameRoomServer
{
    public class LiteNetServer
    {
        private static readonly EventBasedNetListener sr_NetListener = new EventBasedNetListener();
        private readonly NetManager r_NetManager = new NetManager(sr_NetListener);
        private readonly List<ClientData> r_Clients = new List<ClientData>();
        private readonly ObjectPointData r_ObjectPointData;
        private readonly ILogger<LiteNetServer> r_Logger;
        //private readonly Timer r_Timer = new System.Timers.Timer(15);

        private int m_TimerCounts = 0;

        public LiteNetServer(int i_Port)
        {
            r_NetManager.Start(i_Port);
            sr_NetListener.ConnectionRequestEvent += onConnectionRequest;
            sr_NetListener.PeerConnectedEvent += onPeerConnected;
            sr_NetListener.PeerDisconnectedEvent += onPeerDisconnected;
            sr_NetListener.NetworkReceiveEvent += onNetworkReceive;

            //r_Timer.Elapsed += onTimerElapsed;
        }

        public async Task Run()
        {
            while (true)
            {
                var time = DateTime.Now.Millisecond;
                r_NetManager.PollEvents();
                if (r_Clients.Count > 0)
                {
                    updateClients();
                }
                Thread.Sleep(5);
                //await Task.Delay(10);
                //Console.WriteLine($"sent: {r_Clients.ToString()}");
            }
        }

        private void updateClients()
        {
            NetDataWriter writer = new();
            foreach (ClientData data in r_Clients)
            {
                writer.Put(data.PlayerNumber);
                writer.Put(data.Button);
            }

            foreach (ClientData client in r_Clients)
            {
                client.Peer.Send(writer, DeliveryMethod.ReliableOrdered);
            }

        }

        private void updateClientsWithObjectPoint()
        {
            NetDataWriter writer = new();
            
            writer.Put(r_ObjectPointData.m_Column);
            writer.Put(r_ObjectPointData.m_Row);
            writer.Put(r_ObjectPointData.m_Object);
            
            foreach (ClientData client in r_Clients)
            {
                client.Peer.Send(writer, DeliveryMethod.ReliableOrdered);
            }
        }


        private void onNetworkReceive(NetPeer i_Peer, NetPacketReader i_Reader, byte i_Channel, DeliveryMethod i_Deliverymethod)
        {
            if(r_Clients.Exists(client => client.Peer == i_Peer))
            {
                ClientData clientData = r_Clients.Find(client => client.Peer == i_Peer);
                if(clientData != null)
                {
                    clientData.PlayerNumber = i_Reader.GetInt();
                    clientData.Button = i_Reader.GetInt();
                }
            }

            i_Reader.Recycle();
        }

        private void onPeerDisconnected(NetPeer i_Peer, DisconnectInfo i_Disconnectinfo)
        {

        }

        private void onPeerConnected(NetPeer i_Peer)
        {
            r_Clients.Add(new ClientData(i_Peer));
        }

        private void onConnectionRequest(ConnectionRequest i_Request)
        {
            i_Request.AcceptIfKey("myKey");
        }
    }
}
