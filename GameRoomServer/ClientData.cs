using LiteNetLib;

namespace GameRoomServer;

public class ClientData
{
    public NetPeer Peer { get; init; }

    public int PlayerNumber { get; set; }
    public int X { get; set; }

    public int Y { get; set; }
    public int Button { get; set; }

    public ClientData(NetPeer i_Peer, int i_Button = 0)
    {
        Peer = i_Peer;
        Button = i_Button;
    }
}