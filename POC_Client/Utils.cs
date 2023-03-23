using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC_Client
{
    public class Utils
    {     
        public static string m_BaseAddress = "http://localhost:5163";// "https://pocserver20230311140030.azurewebsites.net";
        public static string m_GameHubAddress = m_BaseAddress + "/gamehub";
        public static string m_BounceBallAddress = m_BaseAddress + "/bounceBallHub";
    }
}
