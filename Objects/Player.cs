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
        public bool isInitialized = false;
        //private static Player m_Instance = null;
        private bool m_DidPlayerPickAPlacement = false;
        private string m_Name;
        public int PlayerNumber { get; set; }
        private PlayerType m_PlayerType;
        private string m_RoomCade;


       // private static readonly object s_InstanceLock = new object();
        public Player()
        {
            m_DidPlayerPickAPlacement = false;
        }

        //public static Player Instance
        //{
        //    get
        //    {
        //        lock (s_InstanceLock)
        //        {
        //            if (m_Instance == null)
        //            {
        //                m_Instance = new Player();
        //            }
        //        }
        //        return m_Instance;
        //    }
        //}

        public bool DidPlayerPickAPlacement
        {
            get
            {
                return m_DidPlayerPickAPlacement;
            }
            set
            {
                m_DidPlayerPickAPlacement = value;
            }
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }

        public PlayerType PlayerType
        {
            get
            {
                return m_PlayerType;
            }
            set
            {
                m_PlayerType = value;
            }
        }

        public string RoomCode
        {
            get
            {
                return m_RoomCade;
            }
            set
            {
                m_RoomCade = value;
            }
        }
    }
}
