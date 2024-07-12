using System.CommandLine;

namespace OllamaCommitGen.Cli.Models;

public class ModelOptions
{
    public Option<int> MiroStatOption { get; set; }
    
    public Option<float> MiroStatEtaOption { get; set; }
    
    public Option<float> MiroStatTauOption { get; set; }
    
    public Option<int> NumCtxOption { get; set; }
    
    public Option<int> RepeatLastNOption { get; set; }
    
    public Option<float> RepeatPenaltyOption { get; set; }
    
    public Option<float> TemperatureOption { get; set; }
    
    public Option<int> SeedOption { get; set; }
    
    public Option<List<string>> StopOption { get; set; }
    
    public Option<float> TfsZOption { get; set; }
    
    public Option<int> NumPredictOption { get; set; }
    
    public Option<int> TopKOption { get; set; }
    
    public Option<float> TopPOption { get; set; }
}