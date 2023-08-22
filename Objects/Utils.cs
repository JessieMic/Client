using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Objects
{
    public class Utils
    {     //// ;"http://localhost:5163";// "https://pocserver20230311140030.azurewebsites.net";
        public static string m_BaseAddress = " https://gameroomserverfinalproject.azurewebsites.net";// "https://serverrooms.azurewebsites.net";//"http://localhost:5163";// "https://localhost:7038";//"https://serverrooms.azurewebsites.net";//https://localhost:7038";//""https://gameroomserverfinalproject.azurewebsites.net";//"https://localhost:7038";
        public static string m_GameHubAddress = m_BaseAddress + "/GameHub";
        public static string m_BounceBallAddress = m_BaseAddress + "/bounceBallHub";
        public static string m_InGameHubAddress = m_BaseAddress + "/InGameHub";
    }
}
