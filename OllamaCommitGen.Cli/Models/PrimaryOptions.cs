using System.CommandLine;

namespace OllamaCommitGen.Cli.Models;

public class PrimaryOptions
{
    public Option<string> OriginOption { get; set; }
    
    public Option<string> ModelOption { get; set; }

    public Option<string> LangOption { get; set; }
    
    public Option<string> KeepAliveOption { get; set; }
    
    public Option<bool> NoStreamOption { get; set; }

    public Option<string> ConfigOption { get; set; }
}