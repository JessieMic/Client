using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects
{
    public class VisualUpdateSelectButtons
    {
        public int spot;
        public string textOnButton;
        public bool didPlayerSelect;

        public VisualUpdateSelectButtons() { }
        public VisualUpdateSelectButtons(int i_Spot, string i_textOnButton, bool i_DidPlayerSelect)
        {
            didPlayerSelect = i_DidPlayerSelect;
            spot = i_Spot;
            textOnButton = i_textOnButton;
        }

        public void Set(int i_Spot, string i_textOnButton, bool i_DidPlayerSelect)
        {
            didPlayerSelect = i_DidPlayerSelect;
            spot = i_Spot;
            textOnButton = i_textOnButton;
        }
    }
}
