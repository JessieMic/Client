﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Enums.BoardEnum;

namespace LogicUnit.Logic.GamePageLogic.Games.BombIt
{
    public class BombItBoard
    {
        //public int[,] m_grid = // 18 x 45
        //    {
        //{0,0,0,0,0,0,2,0,0,0,2,0,0,2,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0} ,
        //{0,1,1,1,1,1,0,1,1,1,0,1,1,0,1,1,1,0,1,1,1,1,1,1,1,1,1,1,0,1,1,1,0,1,1,0,1,1,1,0,1,1,1,1,1} ,
        //{3,0,0,0,0,1,0,1,1,1,0,1,1,0,0,0,0,0,1,0,0,0,1,1,0,0,0,1,0,1,1,1,0,1,1,0,0,0,0,0,1,0,0,0,0} ,
        //{0,1,1,1,0,1,3,0,0,0,0,1,1,0,1,1,1,0,1,0,1,0,1,1,0,1,0,1,0,0,0,0,0,1,1,0,1,1,1,0,1,0,1,0,1} ,
        //{0,1,1,1,0,1,0,1,1,1,1,1,1,0,1,1,1,0,1,0,1,0,0,0,0,1,0,1,0,1,1,1,1,1,1,0,1,1,1,0,1,0,1,0,0} ,
        //{0,1,1,1,0,0,0,0,1,1,0,0,0,0,0,1,1,0,0,0,1,1,1,0,1,1,1,0,0,0,1,1,0,0,0,0,0,1,1,0,0,0,1,1,1} ,
        //{3,0,0,0,5,1,1,0,1,1,0,1,1,1,0,1,1,0,1,0,0,1,1,0,0,0,0,1,1,0,1,1,0,1,1,1,0,1,1,0,1,0,0,1,0} ,
        //{0,1,1,1,0,1,1,0,4,0,0,1,1,1,0,1,1,0,1,1,0,1,1,0,1,1,1,1,1,1,0,0,0,1,1,1,0,1,1,0,1,1,0,1,0} ,
        //{3,0,0,0,5,1,1,1,0,1,0,1,1,1,3,0,0,0,1,1,0,1,1,0,0,0,0,1,1,1,0,1,0,1,1,1,0,0,0,0,1,1,0,1,1} ,
        //{0,1,1,1,0,0,4,0,0,1,0,0,0,1,0,1,1,0,0,0,0,0,1,1,1,1,0,0,0,0,0,1,0,0,0,1,0,1,1,0,0,0,0,0,1} ,
        //{0,1,1,1,1,1,0,1,1,1,1,1,0,0,0,1,1,0,1,1,1,0,1,1,1,1,1,1,0,1,1,1,1,1,0,0,0,1,1,1,1,1,1,0,0} ,
        //{3,0,0,0,0,1,0,0,0,0,0,1,1,1,0,0,0,1,1,1,1,0,0,0,0,0,0,1,0,0,0,0,0,1,1,1,0,0,0,0,1,1,1,0,1} ,
        //{0,1,1,1,3,0,0,1,1,1,0,1,0,0,0,1,1,0,0,0,0,0,1,1,0,1,1,1,0,1,1,1,0,1,1,1,0,1,1,0,0,0,0,0,1} ,
        //{0,1,1,1,0,1,1,1,0,0,0,0,0,1,1,1,1,0,1,1,1,0,1,1,0,1,1,1,0,0,0,0,0,0,0,0,0,1,1,0,1,0,1,1,1} ,
        //{0,0,0,0,0,1,1,1,0,1,1,1,0,1,1,1,1,0,1,0,0,0,1,1,0,0,0,1,1,1,0,1,1,1,1,0,1,1,1,0,1,0,0,0,0} ,
        //{0,1,1,1,3,0,0,0,0,1,1,1,3,0,0,0,0,0,1,1,1,0,1,1,1,1,0,0,0,0,0,1,1,1,1,0,0,0,1,0,1,1,1,0,1} ,
        //{0,0,0,0,0,1,1,1,0,0,0,0,0,1,0,1,1,1,1,1,1,0,1,0,0,0,0,1,1,1,1,0,0,0,0,0,1,0,0,0,1,1,1,0,1} ,
        //{0,1,1,1,1,1,1,1,0,1,1,1,0,1,0,0,1,1,1,0,0,0,1,0,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,0,0,0,1} ,
        //    };

        //public int[,] m_grid = // 18 x 45
        //    {
        //{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0} ,
        //{2,1,1,1,1,1,0,1,1,1,0,1,1,0,1,1,1,0,1,0,1,1,1,1,0,1,1,1,0,1,1,1,0,1,1,0,1,1,1,0,1,0,1,1,1} ,
        //{3,0,0,0,0,1,0,1,1,1,2,1,1,0,0,0,0,0,1,0,0,2,1,1,0,0,0,0,0,1,1,1,0,1,1,0,0,0,0,2,1,0,0,0,0} ,
        //{0,1,1,1,0,1,3,0,0,0,0,1,1,0,1,1,1,0,1,1,1,0,1,1,1,1,1,1,0,0,0,0,2,1,1,0,1,1,1,0,1,1,1,0,1} ,//
        //{0,1,1,1,0,1,1,1,1,1,1,1,1,0,1,1,1,0,1,1,1,0,0,0,1,1,1,1,0,1,1,1,1,1,1,0,1,1,1,0,1,1,1,0,0} ,//
        //{0,1,1,1,0,0,0,0,1,1,0,0,0,0,0,1,1,0,0,0,1,1,1,0,1,1,1,1,0,1,1,1,0,0,0,0,0,1,1,0,0,0,1,1,1} ,//
        //{3,0,0,0,5,1,1,0,1,1,0,1,1,1,0,1,1,0,1,0,0,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,0,1,1,0,1,0,0,1,0} ,
        //{0,1,1,1,0,1,1,0,4,0,0,1,1,1,0,1,1,0,1,1,0,1,1,0,1,1,1,1,1,1,0,0,0,1,1,1,0,1,1,0,1,1,0,1,0} ,
        //{3,0,0,0,5,1,1,1,0,1,0,1,1,1,3,0,0,0,1,1,0,1,1,2,0,0,0,1,1,1,0,1,0,1,1,1,0,0,0,0,1,1,0,1,1} ,
        //{0,1,1,1,0,0,4,0,0,1,2,0,0,1,0,1,1,0,0,0,0,0,1,1,1,1,0,0,0,0,0,1,0,0,0,1,0,1,1,2,0,0,0,0,1} ,
        //{0,1,1,1,1,1,0,1,1,1,1,1,0,0,0,1,1,0,1,1,1,0,1,1,1,1,1,1,0,1,1,1,1,1,0,0,0,1,1,1,1,1,1,0,0} ,
        //{3,0,0,0,2,1,0,0,0,0,0,1,1,1,0,0,2,1,1,1,1,0,0,0,0,0,0,1,0,0,0,0,0,1,1,1,0,0,0,0,1,1,1,0,1} ,
        //{0,1,1,1,3,0,0,1,1,1,0,1,0,0,0,1,1,0,0,0,0,0,1,1,0,1,1,1,0,1,1,1,0,1,1,1,0,1,1,0,0,0,0,0,1} ,
        //{0,1,1,1,0,1,1,1,0,0,0,0,0,1,1,1,1,0,1,1,1,0,1,1,0,1,1,1,0,0,0,0,0,0,0,0,0,1,1,0,1,0,1,1,1} ,
        //{0,0,0,0,0,1,1,1,0,1,1,1,0,1,1,1,1,0,1,0,0,0,1,1,0,0,0,1,1,1,0,1,1,1,1,2,1,1,1,0,1,0,0,0,0} ,
        //{0,1,1,1,3,0,0,0,0,1,1,1,3,0,0,0,0,0,1,1,1,0,1,1,1,1,0,0,0,0,0,1,1,1,1,0,0,0,1,0,1,1,1,0,1} ,
        //{0,0,0,0,0,1,1,1,0,0,0,0,0,1,0,1,1,1,1,1,1,0,1,0,0,0,0,1,1,1,1,0,0,0,0,0,1,0,0,0,1,1,1,0,1} ,
        //{0,1,1,1,1,1,1,1,0,1,1,1,0,1,0,0,1,1,1,0,0,0,1,0,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,0,0,0,1} ,
        //    };

        public int[,] m_grid = // 42 x 23
            {
        {0,0,2,2,0,0,2,0,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0} ,
        {0,1,0,1,1,1,2,1,1,1,1,1,0,1,1,1,1,0,1,1,1,1,1} ,
        {0,1,0,2,1,0,0,0,0,1,0,0,2,2,1,0,0,0,1,2,0,0,1} ,
        {0,1,1,0,0,2,1,1,2,1,0,1,1,0,1,2,1,0,1,0,1,0,1} ,
        {2,1,1,0,1,1,1,1,2,1,2,1,1,0,1,2,1,0,1,0,1,0,0} ,
        {2,1,2,2,0,2,2,0,0,2,0,1,1,2,1,0,1,0,0,0,1,1,1} ,
        {2,1,0,1,1,1,1,1,2,1,1,1,0,2,0,0,1,0,1,0,0,1,1} ,
        {0,1,0,1,1,1,2,0,0,1,1,1,0,1,1,1,1,1,1,1,0,1,1} ,
        {0,0,2,0,2,2,0,1,2,0,2,0,0,2,2,0,0,0,0,1,0,1,1} ,
        {0,1,1,1,0,1,2,1,2,1,1,1,0,0,1,1,1,1,0,0,0,0,1} ,
        {0,1,1,1,2,1,0,1,0,0,2,1,2,1,1,1,1,1,0,1,1,0,1} ,
        {0,1,1,1,2,1,0,1,1,1,0,1,2,0,0,0,0,0,0,1,1,0,0} ,
        {0,1,1,1,0,1,0,1,0,0,0,1,0,1,1,1,1,1,0,0,0,0,1} ,
        {0,1,1,1,0,1,2,1,0,1,1,1,0,0,1,1,1,1,0,1,1,0,1} ,
        {0,0,2,0,2,0,0,1,0,2,0,0,2,0,0,0,0,0,0,0,0,0,1} ,
        {0,1,0,1,1,1,0,0,0,1,1,1,0,1,1,1,1,1,0,1,1,0,1} ,
        {0,1,0,1,1,1,1,1,2,0,0,1,0,0,0,0,1,0,2,1,1,0,1} ,
        {0,1,0,0,0,0,0,0,0,1,0,1,1,0,1,0,1,0,1,1,0,0,1} ,
        {0,1,1,0,1,1,1,1,0,1,0,1,1,0,1,0,1,0,1,0,0,0,0} ,
        {0,1,1,0,0,0,1,1,0,1,0,1,1,0,1,0,1,0,1,0,1,1,0} ,
        {0,1,0,0,1,0,0,0,0,1,0,0,0,0,1,0,0,0,1,0,1,1,0} ,
        {0,1,0,1,1,1,0,1,1,1,1,1,0,1,1,1,1,0,1,0,1,1,0} ,
        {0,1,0,0,1,0,0,0,0,1,0,0,0,0,1,0,0,0,1,0,0,0,0},
        {0,1,1,2,0,0,1,1,0,1,0,1,1,0,1,0,1,0,1,0,1,0,1} ,
        {0,1,1,0,1,1,1,1,0,1,0,1,1,0,1,0,1,0,1,0,1,0,0} ,
        {0,1,0,0,0,0,0,0,0,0,0,1,1,0,1,0,1,0,0,0,1,1,1} ,
        {0,1,0,1,1,1,1,1,0,1,1,1,2,0,0,0,1,0,1,0,0,1,1} ,
        {0,1,0,1,1,1,0,0,0,1,1,1,0,1,1,1,1,1,1,1,0,1,1} ,
        {0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,0,1,1} ,
        {0,1,1,1,0,1,0,1,0,1,1,1,0,0,1,1,1,1,0,0,0,0,1} ,
        {0,1,1,1,0,1,0,1,0,0,0,1,0,1,1,1,1,1,0,1,1,0,1} ,
        {0,1,1,1,0,1,0,1,1,1,0,1,0,0,0,0,0,0,0,1,1,0,0} ,
        {0,1,1,1,0,1,0,1,0,0,0,1,0,1,1,1,1,1,0,0,0,0,1} ,
        {0,1,1,1,0,1,0,1,0,1,1,1,0,0,1,1,1,1,0,1,1,0,1} ,
        {0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1} ,
        {0,1,0,1,1,1,0,0,0,1,1,1,0,1,1,1,1,1,0,1,1,0,1} ,
        {0,1,0,1,1,1,1,1,0,0,0,1,0,0,0,0,1,0,0,1,1,0,1} ,
        {0,1,0,0,0,0,0,0,0,1,0,1,1,0,1,0,1,0,1,1,0,0,1} ,
        {0,1,1,0,1,1,1,1,0,1,0,1,1,0,1,0,1,0,1,0,0,0,0} ,
        {0,1,1,0,0,0,1,1,0,1,0,1,1,0,1,0,1,0,1,0,1,1,0} ,
        {0,1,0,0,1,0,0,0,0,1,0,0,0,0,1,0,0,0,1,0,1,1,0} ,
        {0,1,0,1,1,1,0,1,1,1,1,1,0,1,1,1,1,0,1,0,1,1,0} ,
            };

        public int[,] BuildBoardMatrix(int i_Width, int i_Height)
        {
            int[,] mat = new int[i_Width, i_Height];

            for(int r = 0; r < i_Height; r++)
            {
                for(int c = 0; c < i_Width; c++)
                {
                    mat[c, r] = m_grid[r % 41, c % 22];

                    if((r == 0 || c == 0 || r == i_Height - 1 || c == i_Width - 1) && m_grid[r % 41, c % 22] == 1)
                    {
                        mat[c, r] = 0;
                    }
                }
            }

            return mat;
        }
    }
}