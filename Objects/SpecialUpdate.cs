using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects
{
    public struct SpecialUpdate
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Player_ID { get; set; }
        public int Update { get; set; }

        public SpecialUpdate(int i_Update, int i_Player_ID)
        {
            Update = i_Update;
            Player_ID = i_Player_ID;
        }
        public SpecialUpdate(int i_X,int i_Y, int i_Player_ID)
        {
            X = i_X;
            Y = i_Y;
            Player_ID = i_Player_ID;
        }
    }
}
