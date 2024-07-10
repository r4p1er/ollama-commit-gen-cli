namespace OllamaCommitGen.Domain.DataObjects;

public class StreamResponseArrivedArgs(string response, bool done) : EventArgs
{
    public string Response { get; set; } = response;

    public bool Done { get; set; } = done;
}