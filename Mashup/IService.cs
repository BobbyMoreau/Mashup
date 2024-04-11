namespace Mashup
{
    public interface IService
    {
        HttpClient HttpClient { get; }
        string BaseUrl { get; }
        string? EndUrl { get; }

    }
}
