using System.CommandLine;

namespace OllamaCommitGen.Cli;

public static class CommandExtensions
{
    public static Option<T> AddOption<T>(this Command command, string name, string description, string alias,
        ArgumentArity arity, T? defaultValue = default)
    {
        var option = new Option<T>(name, description)
        {
            Arity = arity,
            IsRequired = false
        };
        option.AddAlias(alias);
        
        if (defaultValue != null) option.SetDefaultValue(defaultValue);

        command.AddOption(option);

        return option;
    }
}