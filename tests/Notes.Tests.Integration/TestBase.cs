namespace Notes.Tests.Integration;

public abstract class TestBase
{
    protected static readonly CustomWebApplicationFactory Factory = new();
    protected readonly HttpClient Client;

    protected TestBase()
    {
        Client = Factory.CreateClient();
    }
}