using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicUnit.Logic.GamePageLogic.Games
{
    public class Sprite
    {
        string[] m_Pngs;
        public int m_CurrentIndex = 1;
        private int m_TimeBetweenFrames;
        private int m_AmountOfPng;

        public Sprite(string i_BasePng, int i_Miliseconds, int i_AmountOfPng,int i_AmountOfFrames)
        {
            m_Pngs = new string[i_AmountOfPng];
            m_AmountOfPng = i_AmountOfPng;
            for(int i = 1; i <= i_AmountOfPng; i++)
            {
                m_Pngs[i-1]=$"a{i_BasePng}{i}.png";
            }

            m_TimeBetweenFrames = i_Miliseconds / i_AmountOfFrames;
        }

        public string GetImageSource(double i_TimeThatPassed)
        {
            if(m_CurrentIndex != m_AmountOfPng)
            {
                if (m_TimeBetweenFrames * (m_CurrentIndex) < i_TimeThatPassed)
                {
                    m_CurrentIndex++;
                }
            }
            
            return m_Pngs[m_CurrentIndex-1];
        }
    }
}
