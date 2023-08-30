namespace LogicUnit;

public class ServerAddressManager
{
    public string BaseAddress { get; init; }

    public string GameHubAddress
    {
        get => GameHubAddress;
        init => GameHubAddress = BaseAddress + "/GameHub";
    }

    public string InGameHubAddress
    {
        get => InGameHubAddress;
        init => InGameHubAddress = BaseAddress + "/InGameHub";
    }
}