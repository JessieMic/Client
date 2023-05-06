using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.Extensions.Logging;
namespace LogicUnit.Logic.GamePageLogic

{
    public class LiteNetClient
    {
        private static readonly EventBasedNetListener sr_Listener = new EventBasedNetListener();
        private readonly NetManager r_NetManager = new NetManager(sr_Listener);
        private static object s_Lock = new object();
        private static LiteNetClient s_Instance = null;
        public event Action ReceivedData;
        public List<PlayerData> PlayersData { get; set; }

        public int PlayerNumber { get; set; }
        //public int NumberOfPlayers { get; set; }

        public static LiteNetClient Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    lock (s_Lock)
                    {
                        s_Instance ??= new LiteNetClient();
                    }
                }

                return s_Instance;
            }
        }
        //private readonly ILogger<LiteNetClient> r_Logger;

        private LiteNetClient(/*ILogger<LiteNetClient> i_Logger*/)
        {
            //r_Logger = i_Logger;
            r_NetManager.Start();
            r_NetManager.Connect("127.0.0.1", 5555, "myKey");
            sr_Listener.NetworkReceiveEvent += OnReceive;
        }

        public void Init(int i_NumberOfPlayers)
        {
            PlayersData = new List<PlayerData>(i_NumberOfPlayers);

            for (int i = 0; i < i_NumberOfPlayers; i++)
            {
                PlayersData.Add(new PlayerData(i));
            }

            Console.WriteLine($"Client started");
        }


        public void Send(int i_PlayerNumber, int i_Button)
        {
            NetDataWriter writer = new NetDataWriter();
            writer.Put(i_PlayerNumber);
            writer.Put(i_Button);
            Task.Run(() =>
                {
                    r_NetManager.FirstPeer.Send(writer, DeliveryMethod.Unreliable);
                });
            //r_NetManager.FirstPeer.Send(writer, DeliveryMethod.Unreliable);
            //r_Logger.LogInformation($"Sent: {i_PlayerNumber}");
        }

        private void OnReceive(NetPeer i_Peer, NetPacketReader i_Reader, byte i_Channel, DeliveryMethod i_Deliverymethod)
        {
            Console.WriteLine($"Received:");
            foreach(PlayerData t in PlayersData)
            {
                PlayersData.ElementAt(i_Reader.GetInt()).Button = i_Reader.GetInt();
            }
            ReceivedData?.Invoke();
            i_Reader.Recycle();
        }

        public void Run()
        {
            new Thread(update).Start();
        }

        private void update()
        {
            while (true)
            {
                r_NetManager.PollEvents();
                Thread.Sleep(100);
            }
        }

    }
}
