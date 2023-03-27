using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC_Client.Objects
{
    public class VisualUpdateSelectButtons
    {
        //public int m_Spot;
        //public string m_TextOnButton;
        //public bool m_DidClientSelect;
        public int spot;
        public string textOnButton;
        public bool didClientSelect;

        public VisualUpdateSelectButtons()
        {
        }
        public VisualUpdateSelectButtons(int i_Spot,string i_textOnButton,bool i_DidClientSelect)
        {
            didClientSelect = i_DidClientSelect;
            spot = i_Spot;
            textOnButton = i_textOnButton;
        }

        public void Set(int i_Spot, string i_textOnButton, bool i_DidClientSelect)
        {
            didClientSelect = i_DidClientSelect;
            spot = i_Spot;
            textOnButton = i_textOnButton;
        }


    }
}
