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

        public Button GetButton()
        {
            return m_Button;
        }

        public ButtonImage()
        {
            m_Button.BorderColor = Colors.Transparent;
            m_Button.Background = Brush.Transparent;
            m_Button.ZIndex = 0;
            m_Image.ZIndex = -1;
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

        public double FontSize
        {
            set
            {
                m_Button.FontSize = value;
            }
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

