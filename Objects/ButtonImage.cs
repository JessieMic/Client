using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects
{
    public class ButtonImage : Image
    {
        public Button m_Button = new Button();
        private string m_SourcePressed;

        public ButtonImage()
        {
            m_Button.BorderColor = Colors.Transparent;
            m_Button.Background = Brush.Transparent;
            m_Button.ZIndex = 0;
            m_Image.ZIndex = -1;
        }

        public Button GetButton()
        {
            m_SourcePressed = "pressed" + m_Source;
            return m_Button;
        }

        public void SetButtonImage(GameObject i_GameObject)
        {
            SetImage(i_GameObject);

            m_Button.TranslationX = i_GameObject.m_PointsOnScreen[0].m_Column;
            m_Button.TranslationY = i_GameObject.m_PointsOnScreen[0].m_Row;
            m_Button.ClassId = i_GameObject.m_ButtonType.ToString();
            m_Button.ZIndex = -1;
            m_Button.Rotation = i_GameObject.m_Rotatation[0];
            if (i_GameObject.m_OurSize.m_Width != 0)
            {
                m_Button.WidthRequest = i_GameObject.m_OurSize.m_Width;
                m_Button.HeightRequest = i_GameObject.m_OurSize.m_Height;
            }

            Text = i_GameObject.m_text;
        }

        public void IsButtonPressed(bool i_IsButtonPressed)
        {
            if(i_IsButtonPressed)
            {
                m_Image.Source = m_SourcePressed;
            }
            else
            {
                m_Image.Source = m_Source;
            }
        }

        public string Text
        {
            set
            {
                m_Button.Text = value;
                m_Button.FontSize = m_Button.HeightRequest * 0.3;
                m_Button.FontAutoScalingEnabled = true;
            }
        }

        public bool IsEnabled
        {
            set
            {
                m_Button.IsEnabled = value;
            }
        }

        public double FontSize
        {
            get
            {
                return m_Button.FontSize;
            }
            set
            {
                m_Button.FontSize = value;
            }
        }

        public void SetDefualtFontSize()
        {
            m_Button.FontSize = m_Button.HeightRequest * 0.3;
        }

        override public double HeightRequest
        {
            set
            {
                m_Button.HeightRequest = value;
                m_Image.HeightRequest = value;
            }
        }
        override public bool IsVisible
        {
            set
            {
                m_Button.IsVisible = value;
                m_Image.IsVisible = value;
            }
        }

        override public string ClassId
        {
            set
            {
                m_Button.ClassId = value;
                m_Image.ClassId = value;
            }
        }

        override public double WidthRequest
        {
            set
            {
                m_Button.WidthRequest = value;
                m_Image.WidthRequest = value;
            }
        }

        override public double TranslationX
        {
            set
            {
                m_Button.TranslationX = value;
                m_Image.TranslationX = value;
            }
        }

        override public double TranslationY
        {
            set
            {
                m_Button.TranslationY = value;
                m_Image.TranslationY = value;
            }
        }

        override public int ZIndex
        {
            set
            {
                m_Button.ZIndex = value;
                m_Image.ZIndex = value-1;
            }
        }

        override public double Rotation
        {
            set
            {
                m_Button.Rotation = value;
                m_Image.Rotation = value;
            }
        }

        override public Microsoft.Maui.Controls.LayoutOptions HorizontalOptions
        {
            set
            {
                m_Button.HorizontalOptions = value;
                m_Image.HorizontalOptions = value;
            }
        }

        override public Microsoft.Maui.Controls.LayoutOptions VerticalOptions
        {
            set
            {
                m_Button.VerticalOptions = value;
                m_Image.VerticalOptions = value;
            }
        }
    }
}

