using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects
{
    public class Image
    {
        public Microsoft.Maui.Controls.Image m_Image = new Microsoft.Maui.Controls.Image();

        public Microsoft.Maui.Controls.Image GetImage()
        {
            return m_Image;
        }

        virtual public double HeightRequest
        {
            set
            {
                m_Image.HeightRequest = value;
            }
        }

        virtual public string ClassId
        {
            set
            {
                m_Image.ClassId = value;
            }
        }

        virtual public double WidthRequest
        {
            set
            {
                m_Image.WidthRequest = value;
            }
        }

         virtual public double TranslationX
        {
            set
            {
                m_Image.TranslationX = value;
            }
        }

        virtual public double TranslationY
        {
            set
            {
                m_Image.TranslationY = value;
            }
        }
        
        virtual public int ZIndex
        {
            set
            {
                m_Image.ZIndex = value;
            }
        }

        public Microsoft.Maui.Aspect Aspect
        {
            set
            {
                m_Image.Aspect = value;
            }
        }

        virtual public double Rotation
        {
            set
            {
                m_Image.Rotation = value;
            }
        }

        public string Source
        {
            set
            {
                m_Image.Source = value;
            }
        }

        virtual public bool IsVisible
        {
            set
            {
                m_Image.IsVisible = value;
            }
        }

        virtual public Microsoft.Maui.Controls.LayoutOptions HorizontalOptions
        {
            set
            {
                m_Image.HorizontalOptions = value;
            }
        }

        virtual public Microsoft.Maui.Controls.LayoutOptions VerticalOptions
        {
            set
            {
                m_Image.VerticalOptions = value;
            }
        }
    }
}
