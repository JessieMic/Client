using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POC_Client.Objects.Enums;

namespace POC_Client.Objects
{
    public struct ScreenObject
    {
        public int m_Row;
        public int m_Column;
        public eScreenObjectType m_ScreenObjectType;
        public string m_Text;
        public eButton m_KindOfButton;
        public string m_ImageSource;
        public int m_Width;
        public int m_Height;

       public ScreenObject(
           eScreenObjectType i_Type,
           eButton? i_Button,
           int i_Column,
           int i_Row,
           int i_Width,
           int i_Height,
           string i_ImageSource,
           string? i_Text)
       {

           m_Text = i_Text;
           m_ImageSource = i_ImageSource;
           m_Column = i_Column;
           m_Width = i_Width;
           m_Height = i_Height;
           m_Row = i_Row;
           m_ScreenObjectType = i_Type;

           if(i_Button != null)
           {
               m_KindOfButton = (eButton)i_Button;
           }

           if(i_Text != null)
           {
               m_Text = i_Text;
           }
       }
    }
}
