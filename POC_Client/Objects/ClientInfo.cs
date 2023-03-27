using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC_Client.Objects
{
    public class ClientInfo
    {
        public bool isInitialized = false;
        private static ClientInfo m_Instance = null;
        private bool m_DidClientPickAPlacement;
        private string m_Name;
        private int m_ButtonThatClientPicked;

        private static readonly object s_InstanceLock = new object();
        private ClientInfo()
        {
            m_DidClientPickAPlacement = false;
        }

        public static ClientInfo Instance
        {
            get
            {
                lock (s_InstanceLock)
                {
                    if (m_Instance == null)
                    {
                        m_Instance = new ClientInfo();
                    }
                }
                return m_Instance;
            }
        }

        public bool DidClientPickAPlacement
        {
            get
            {
                return m_DidClientPickAPlacement;
            }
            set
            {
                m_DidClientPickAPlacement = value;
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

        public int ButtonThatClientPicked
        {
            get
            {
                return m_ButtonThatClientPicked;
            }
            set
            {
                m_ButtonThatClientPicked = value;
            }
        }
    }
}
