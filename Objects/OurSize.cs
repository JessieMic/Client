//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Objects
//{

//    public struct SizeInPixelsD
//    {
//        public int Width = 0;
//        public int Height = 0;

//        public SizeInPixelsD() { }

//        public SizeInPixelsD(int i_Width, int i_Height)
//        {
//            Width = i_Width;
//            Height = i_Height;
//        }

//        public SizeInPixelsD SetAndGetSize(int i_Width, int i_Height)
//        {
//            Width = i_Width;
//            Height = i_Height;

//            return this;
//        }

//        public void SetSize(int i_Width, int i_Height)
//        {
//            Width = i_Width;
//            Height = i_Height;
//        }

//        public static bool operator ==(SizeInPixelsD i_P1, SizeInPixelsD i_P2)
//        {
//            return i_P1.Width == i_P2.Width && i_P1.Height == i_P2.Height;
//        }

//        public static bool operator !=(SizeInPixelsD i_P1, SizeInPixelsD i_P2)
//        {
//            return i_P1.Width != i_P2.Width || i_P1.Height != i_P2.Height;
//        }

//        public int[] Getint()
//        {
//            int[] result = new int[2];
//            result[0] = Width;
//            result[1] = Height;

//            return result;
//        }
//    }
//}
