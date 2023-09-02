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
        protected double m_Density;
        protected string m_Source;

        public Image()
        {
            m_Density = GameInformation.Instance.ScreenDensity;
            if(GameInformation.Instance.Player.PlayerNumber == 1)
            {
                m_Density = 3 / 2.625;
                
               //m_Density = 2.625;
            }
            else
            {
                m_Density = 1;
            }

            //if (m_Density != 1)
            //{
            //    m_Density /= 2.615;
            //}
        }
        public void SetImage(GameObject i_GameObject)
        {
            m_Image.IsAnimationPlaying = false;
            m_Image.TranslationX = (int)(i_GameObject.PointOnScreen.Column/ m_Density);
            m_Image.TranslationY = (int)(i_GameObject.PointOnScreen.Row);
            m_Image.Aspect = Aspect.AspectFill;
            m_Image.Source = i_GameObject.ImageSource;
            m_Image.ClassId = i_GameObject.ImageSource;
            m_Image.ZIndex = i_GameObject.ZIndex;
            m_Image.Rotation = i_GameObject.Rotatation;
            m_Image.IsVisible = i_GameObject.IsVisable;
            if (i_GameObject.Size.Width != 0)
            {
                m_Image.WidthRequest = (i_GameObject.Size.Width / m_Density);
                m_Image.HeightRequest = (int)(i_GameObject.Size.Height);
                if (i_GameObject.ImageSource != "bombitbackground.png")
                {
                    m_Image.Aspect = Aspect.Fill;
                }
            }
        }

        public void Update(GameObject i_GameObject)
        {
            m_Image.IsAnimationPlaying = true;
            m_Image.TranslationX = (int)(i_GameObject.PointOnScreen.Column / m_Density);
            m_Image.TranslationY = (int)(i_GameObject.PointOnScreen.Row );
            m_Image.IsVisible = i_GameObject.IsVisable;
            m_Image.ScaleX = i_GameObject.ScaleX;
            m_Image.ScaleY = i_GameObject.ScaleY;
            if (m_Image.ClassId != i_GameObject.ImageSource)
            {
                m_Image.Source = m_Image.ClassId = i_GameObject.ImageSource;
            }
            m_Image.Rotation = i_GameObject.Rotatation;
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

        public void FadeTo(int i_Opacity, uint i_Length)
        {
            m_Image.FadeTo(i_Opacity, i_Length, null);
        }
    }
}
