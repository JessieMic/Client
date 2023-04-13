using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Enums;

namespace Objects
{
    public struct ScreenObject
    {
        public Point m_Point; 
        public eScreenObjectType m_ScreenObjectType;
        public string m_Text;
        public eButton m_KindOfButton;
        public string m_ImageSource;
        public Size m_Size;

       public ScreenObject(
           eScreenObjectType i_Type,
           eButton? i_Button,
           Point i_Point,
           Size i_Size,
           string i_ImageSource,
           string? i_Text)
       {

           m_Text = i_Text;
           m_ImageSource = i_ImageSource;
           m_Point = i_Point;
           m_Size = i_Size;
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
