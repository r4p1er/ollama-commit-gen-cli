using System.CommandLine;
using OllamaCommitGen.Cli.Models;

namespace OllamaCommitGen.Cli.Extensions;

public static class CommandExtensions
{
    public static Option<T> AddOption<T>(this Command command, string name, string description, string? alias,
        ArgumentArity arity, T? defaultValue = default)
    {
        var option = new Option<T>(name, description)
        {
            Arity = arity,
            IsRequired = false
        };
        
        if (alias != null) option.AddAlias(alias);
        
        if (defaultValue != null) option.SetDefaultValue(defaultValue);

        command.AddOption(option);

        return option;
    }

    public static PrimaryOptions AddPrimaryOptions(this Command command)
    {
        var originOption = command.AddOption<string>(
            name: "--origin",
            description: "Specifies a custom remote Ollama host",
            alias: "-o",
            arity: ArgumentArity.ExactlyOne,
            defaultValue: "http://localhost:11434"
        );

        var modelOption = command.AddOption<string>(
            name: "--model",
            description: "Specifies an Ollama model to be used",
            alias: "-m",
            arity: ArgumentArity.ExactlyOne,
            defaultValue: "codellama"
        );

        var langOption = command.AddOption<string>(
            name: "--lang",
            description: "Specifies a language (ISO-639-3) to be used for generating commit message",
            alias: "-l",
            arity: ArgumentArity.ExactlyOne,
            defaultValue: "eng"
        );

        var keepaliveOption = command.AddOption<string>(
            name: "--keepalive",
            description: "Specifies how long the model will stay loaded into memory following the request",
            alias: "-k",
            arity: ArgumentArity.ExactlyOne,
            defaultValue: "5m"
        );

        var noStreamOption = command.AddOption<bool>(
            name: "--no-stream",
            description: "Disables real time generating the commit message",
            alias: "-s",
            arity: ArgumentArity.ZeroOrOne,
            defaultValue: false
        );

        var configOption = command.AddOption<string>(
            name: "--config",
            description: "Specifies configuration file. JSON format is supported",
            alias: "-c",
            arity: ArgumentArity.ExactlyOne,
            defaultValue: null
        );

        return new PrimaryOptions()
        {
            OriginOption = originOption,
            ModelOption = modelOption,
            LangOption = langOption,
            KeepAliveOption = keepaliveOption,
            NoStreamOption = noStreamOption,
            ConfigOption = configOption
        };
    }

    public static ModelOptions AddModelOptions(this Command command)
    {
        var miroStatOption = command.AddOption<int>(
            name: "--mirostat",
            description: "Enable Mirostat sampling for controlling perplexity",
            alias: null,
            arity: ArgumentArity.ExactlyOne,
            defaultValue: 0
        );
        
        var miroStatEtaOption = command.AddOption<float>(
            name: "--mirostat-eta",
            description: "Influences how quickly the algorithm responds to feedback from the generated text. A lower learning rate will result in slower adjustments, while a higher learning rate will make the algorithm more responsive",
            alias: null,
            arity: ArgumentArity.ExactlyOne,
            defaultValue: 0.1f
        );
        
        var miroStatTauOption = command.AddOption<float>(
            name: "--mirostat-tau",
            description: "Controls the balance between coherence and diversity of the output. A lower value will result in more focused and coherent text",
            alias: null,
            arity: ArgumentArity.ExactlyOne,
            defaultValue: 5.0f
        );
        
        var numCtxOption = command.AddOption<int>(
            name: "--num-ctx",
            description: "Sets the size of the context window used to generate the next token",
            alias: null,
            arity: ArgumentArity.ExactlyOne,
            defaultValue: 2048
        );
        
        var repeatLastNOption = command.AddOption<int>(
            name: "--repeat-last-n",
            description: "Sets how far back for the model to look back to prevent repetition",
            alias: null,
            arity: ArgumentArity.ExactlyOne,
            defaultValue: 64
        );
        
        var repeatPenaltyOption = command.AddOption<float>(
            name: "--repeat-penalty",
            description: "Sets how strongly to penalize repetitions. A higher value (e.g., 1.5) will penalize repetitions more strongly, while a lower value (e.g., 0.9) will be more lenient",
            alias: null,
            arity: ArgumentArity.ExactlyOne,
            defaultValue: 1.1f
        );
        
        var temperatureOption = command.AddOption<float>(
            name: "--temperature",
            description: "The temperature of the model. Increasing the temperature will make the model answer more creatively",
            alias: null,
            arity: ArgumentArity.ExactlyOne,
            defaultValue: 0.8f
        );
        
        var seedOption = command.AddOption<int>(
            name: "--seed",
            description: "Sets the random number seed to use for generation. Setting this to a specific number will make the model generate the same text for the same prompt",
            alias: null,
            arity: ArgumentArity.ExactlyOne,
            defaultValue: 0
        );
        
        var stopOption = command.AddOption<string[]?>(
            name: "--stop",
            description: "Sets the stop sequences to use. When this pattern is encountered the LLM will stop generating text and return",
            alias: null,
            arity: ArgumentArity.OneOrMore,
            defaultValue: null
        );
        
        var tfsZOption = command.AddOption<float>(
            name: "--tfs-z",
            description: "Tail free sampling is used to reduce the impact of less probable tokens from the output. A higher value (e.g., 2.0) will reduce the impact more, while a value of 1.0 disables this setting",
            alias: null,
            arity: ArgumentArity.ExactlyOne,
            defaultValue: 1.0f
        );
        
        var numPredictOption = command.AddOption<int>(
            name: "--num-predict",
            description: "Maximum number of tokens to predict when generating text",
            alias: null,
            arity: ArgumentArity.ExactlyOne,
            defaultValue: 128
        );
        
        var topKOption = command.AddOption<int>(
            name: "--top-k",
            description: "Reduces the probability of generating nonsense. A higher value (e.g. 100) will give more diverse answers, while a lower value (e.g. 10) will be more conservative",
            alias: null,
            arity: ArgumentArity.ExactlyOne,
            defaultValue: 40
        );
        
        var topPOption = command.AddOption<float>(
            name: "--top-p",
            description: "Works together with top-k. A higher value (e.g., 0.95) will lead to more diverse text, while a lower value (e.g., 0.5) will generate more focused and conservative text",
            alias: null,
            arity: ArgumentArity.ExactlyOne,
            defaultValue: 0.9f
        );

        return new ModelOptions()
        {
            MiroStatOption = miroStatOption,
            MiroStatEtaOption = miroStatEtaOption,
            MiroStatTauOption = miroStatTauOption,
            NumCtxOption = numCtxOption,
            RepeatLastNOption = repeatLastNOption,
            RepeatPenaltyOption = repeatPenaltyOption,
            TemperatureOption = temperatureOption,
            SeedOption = seedOption,
            StopOption = stopOption,
            TfsZOption = tfsZOption,
            NumPredictOption = numPredictOption,
            TopKOption = topKOption,
            TopPOption = topPOption
        };
    }
}