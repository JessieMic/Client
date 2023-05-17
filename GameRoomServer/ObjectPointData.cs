using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicUnit.Logic.GamePageLogic
{
    public class ObjectPointData
    {
        public NetPeer Peer { get; init; }
        public int m_Column;
        public int m_Row;
        public int m_Object;

        public ObjectPointData(NetPeer i_Peer)
        {
            Peer = i_Peer;
        }
        public void set(int i_Column, int i_Row, int i_Object)
        {
            m_Column = i_Column;
            m_Row = i_Row;
            m_Object = i_Object;
        }
    }
}
