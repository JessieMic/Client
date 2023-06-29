namespace DTOs
{
    public struct SizeDTO
    {
        public int m_Width = 0;
        public int m_Height = 0;

        public SizeDTO() { }

        public SizeDTO(int i_Width, int i_Height)
        {
            m_Width = i_Width;
            m_Height = i_Height;
        }

        public SizeDTO SetAndGetSize(int i_Width, int i_Height)
        {
            m_Width = i_Width;
            m_Height = i_Height;

            return this;
        }

        public void SetSize(int i_Width, int i_Height)
        {
            m_Width = i_Width;
            m_Height = i_Height;
        }

        public static bool operator ==(SizeDTO i_P1, SizeDTO i_P2)
        {
            return i_P1.m_Width == i_P2.m_Width && i_P1.m_Height == i_P2.m_Height;
        }

        public static bool operator !=(SizeDTO i_P1, SizeDTO i_P2)
        {
            return i_P1.m_Width != i_P2.m_Width || i_P1.m_Height != i_P2.m_Height;
        }

        public int[] Getint()
        {
            int[] result = new int[2];
            result[0] = m_Width;
            result[1] = m_Height;

            return result;
        }
    }
}