using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Enums;

namespace Objects
{
    public class Image
    {
        public Microsoft.Maui.Controls.Image m_Image = new Microsoft.Maui.Controls.Image();
        protected string m_Source;

        public void SetImage(GameObject i_GameObject)
        {
            m_Image.TranslationX = i_GameObject.m_PointsOnScreen[0].m_Column;
            m_Image.TranslationY = i_GameObject.m_PointsOnScreen[0].m_Row;
            m_Image.Aspect = Aspect.AspectFill;
            m_Image.Source = i_GameObject.m_ImageSources[0];
            m_Image.ClassId = i_GameObject.m_ImageSources[0];
            m_Image.ZIndex = -1;
            m_Image.Rotation = i_GameObject.m_Rotatation[0];
            if (i_GameObject.m_OurSize.m_Width != 0)
            {
                m_Image.WidthRequest = i_GameObject.m_OurSize.m_Width;
                m_Image.HeightRequest = i_GameObject.m_OurSize.m_Height;
                if (i_GameObject.m_ImageSources[0] != "snakebackground.png")
                {
                    m_Image.Aspect = Aspect.Fill;
                }
            }
        }

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
                m_Source = value;
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
