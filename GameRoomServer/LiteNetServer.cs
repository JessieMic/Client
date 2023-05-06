using System.Timers;
using LiteNetLib;
using LiteNetLib.Utils;
using Timer = System.Timers.Timer;

namespace GameRoomServer
{
    public class LiteNetServer
    {
        private static readonly EventBasedNetListener sr_NetListener = new EventBasedNetListener();
        private readonly NetManager r_NetManager = new NetManager(sr_NetListener);
        private readonly List<ClientData> r_Clients = new List<ClientData>();
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

        public void Run()
        {
            while(true)
            {
                r_NetManager.PollEvents();
                updateClients();
                Thread.Sleep(15);
            }
        }

        private void updateClients()
        {
            foreach(ClientData client in r_Clients)
            {
                NetDataWriter writer = new();
                writer.Put(client.Button);
                client.Peer.Send(writer, DeliveryMethod.Unreliable);
            }
        }


        private void onNetworkReceive(NetPeer i_Peer, NetPacketReader i_Reader, byte i_Channel, DeliveryMethod i_Deliverymethod)
        {
            ClientData clientData = r_Clients.Find(client => client.Peer == i_Peer);
            clientData.Button = i_Reader.GetInt();

            i_Reader.Recycle();
        }

        private void onPeerDisconnected(NetPeer i_Peer, DisconnectInfo i_Disconnectinfo)
        {
            throw new NotImplementedException();
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
