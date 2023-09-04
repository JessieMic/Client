namespace LogicUnit.Logic.GamePageLogic;

public class PlayerData
{
    public PlayerData(int i_PlayerNumber)
    {
        PlayerNumber = i_PlayerNumber;
    }

    public PlayerData(int i_PlayerNumber, int i_button, Point i_Point)
    {
        PlayerNumber = i_PlayerNumber;
    }
    public Objects.Point PlayerPointData { get; set; }
    public int PlayerNumber { get; init; }
    public int Button { get; set; }
    public bool IsNewButton { get; set; }
}