using Microsoft.AspNetCore.SignalR.Client;
using Objects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects
{
    public class Player
    {
        public bool IsInitialized = false;
        //private static Player m_Instance = null;

        public int PlayerNumber { get; set; }


        // private static readonly object s_InstanceLock = new object();
        public Player()
        {
            DidPlayerPickAPlacement = false;
        }

        public bool DidPlayerPickAPlacement { get; set; } = false;

        public string Name { get; set; }

        public PlayerType PlayerType { get; set; }

        public string RoomCode { get; set; }
    }
}
