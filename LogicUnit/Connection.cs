using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicUnit
{
    internal class Connection
    {
        //public string Name { get; set; }

        public event Action<string> PlayerNamePicked;
        public event Action<string> GameCodeInserted;


        private void insertRoomCode(string i_Code)
        {
            
        }

        private void generateRoomCodeFromServer()
        {
            // request the generated code from server
        }
    }
}
