using LiteNetLib;

namespace GameRoomServer;

public class ClientData
{
    public NetPeer Peer { get; init; }

    public int Button { get; set; }

    public ClientData(NetPeer i_Peer, int i_Button = -1)
    {
        Peer = i_Peer;
        Button = i_Button;
    }
}