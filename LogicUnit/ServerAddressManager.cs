namespace LogicUnit;

public class ServerAddressManager
{
    private static ServerAddressManager? s_Instance = null;

    public string GameHubAddress { get; private set; }
    public string InGameHubAddress { get; private set; }

    private static readonly object sr_Lock = new();

    private ServerAddressManager()
    {
    }

    public void SetAddresses(string i_BaseAddress)
    {
        GameHubAddress = i_BaseAddress + "/GameHub";

        InGameHubAddress = i_BaseAddress + "/InGameHub";
    }

    public static ServerAddressManager? Instance
    {
        get
        {
            lock(sr_Lock)
            {
                return s_Instance ??= new ServerAddressManager();
            }
        }
    }
}