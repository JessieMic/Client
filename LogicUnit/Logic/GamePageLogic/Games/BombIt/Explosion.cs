using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicUnit.Logic.GamePageLogic.Games.Pacman;
using Objects;
using Objects.Enums;
using Point = Objects.Point;

namespace LogicUnit.Logic.GamePageLogic.Games.BombIt
{
    internal class Explosion : GameObject
    {
        private Sprite m_Sprite;

        public Explosion(int i_Number)
        {
            MonitorForCollision = true;
            IsVisable = false;
            IsCollisionEnabled = false;
            this.Initialize(eScreenObjectType.Image, i_Number, "a1explosion.png", new Point(0,0), true,
                m_GameInformation.PointValuesToAddToScreen);
            m_Sprite = new Sprite("explosion", 500, 4,10);
        }

        public override void Update(double i_TimeElapsed)
        {
            ImageSource = m_Sprite.GetImageSource(i_TimeElapsed-2500);
            OnUpdate();
        }

        public void Ignite(Point i_Point)
        {
            MoveToPointInGrided(i_Point);
            ChangeState(true);
        }

        public void ChangeState(bool i_IsIgnited)
        {
            IsVisable = i_IsIgnited;
            IsCollisionEnabled = i_IsIgnited;
            if(!i_IsIgnited)
            {
                m_Sprite.m_CurrentIndex = 1;
            }

            OnUpdate();
        }
    }
}
