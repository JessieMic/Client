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
        private readonly List<NetPeer> r_Clients = new List<NetPeer>();
        private readonly Dictionary<NetPeer, Tuple<int, int>> r_ClientsData = new Dictionary<NetPeer, Tuple<int, int>>();
        private readonly Timer r_Timer = new System.Timers.Timer(15);

        private int m_TimerCounts = 0;
        public LiteNetServer(int i_Port)
        {
            r_NetManager.Start(i_Port);
            sr_NetListener.ConnectionRequestEvent += onConnectionRequest;
            sr_NetListener.PeerConnectedEvent += onPeerConnected;
            sr_NetListener.PeerDisconnectedEvent += onPeerDisconnected;
            sr_NetListener.NetworkReceiveEvent += onNetworkReceive;

            r_Timer.Elapsed += onTimerElapsed;
        }

        private void onTimerElapsed(object? i_Sender, ElapsedEventArgs i_E)
        {
            r_NetManager.SendToAll(NetDataWriter.FromString($"heyyyaa we have {r_Clients.Count} timer is {m_TimerCounts}"), DeliveryMethod.ReliableOrdered);
        }

        private byte[] clientsDataInBytes()
        {
            //convert dictionary to byte array
            throw new NotImplementedException();
        }

        private void onNetworkReceive(NetPeer i_Peer, NetPacketReader i_Reader, byte i_Channel, DeliveryMethod i_Deliverymethod)
        {
            throw new NotImplementedException();
        }

        private void onPeerDisconnected(NetPeer i_Peer, DisconnectInfo i_Disconnectinfo)
        {
            throw new NotImplementedException();
        }

        private void onPeerConnected(NetPeer i_Peer)
        {

        }

        private void onConnectionRequest(ConnectionRequest i_Request)
        {
            if (r_Clients.Count == 0)
            {
                r_Timer.Start();
            }
            r_Clients.Add(i_Request.Accept());
        }
    }
}
