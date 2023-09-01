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
        private int m_Blink = 0;
        public Explosion()
        {
            MonitorForCollision = true;
            IsVisable = false;
            IsCollisionEnabled = false;
            this.Initialize(eScreenObjectType.Image, 2, "explosion4.png",new Point(0,0), true,
                m_GameInformation.PointValuesToAddToScreen);
        }

        //public override void Update(double i_TimeElapsed)
        //{
        //    double timePassed = m_GameInformation.RealWorldStopwatch.Elapsed.TotalMilliseconds - m_CherryTimeStart;

        //    if (timePassed > 4500 && timePassed < 7000)
        //    {
        //        if (m_Blink % 5 == 0)
        //        {
        //            if (ImageSource == $"pacman_ghost_{ObjectNumber}_berry.png")
        //            {
        //                ImageSource = $"pacman_ghost_{ObjectNumber}.png";
        //            }
        //            else
        //            {
        //                ImageSource = $"pacman_ghost_{ObjectNumber}_berry.png";
        //            }
        //        }
        //        m_Blink++;
        //    }
        //    else if (timePassed > 7000)
        //    {
        //        ImageSource = $"pacman_ghost_{ObjectNumber}.png";
        //    }

        //    OnUpdate();
        //}

        public void Ignite(Point i_Point)
        {
            MoveToPointInGrided(i_Point);
            ChangeState(true);
        }

        public void ChangeState(bool i_IsIgnited)
        {
            IsVisable = i_IsIgnited;
            IsCollisionEnabled = i_IsIgnited;
            OnUpdate();
        }
    }
}
