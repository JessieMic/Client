namespace LogicUnit;

public class ServerAddressManager
{
    private readonly string r_BaseAddress;
    public string GameHubAddress { get; }
    public string InGameHubAddress { get; }

    public ServerAddressManager(string i_BaseAddress)
    {
        r_BaseAddress = i_BaseAddress;
        GameHubAddress = i_BaseAddress + "/GameHub";
        InGameHubAddress = i_BaseAddress + "/InGameHub";
    }
}