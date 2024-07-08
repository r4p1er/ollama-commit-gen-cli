using System.CommandLine.Builder;
using LibGit2Sharp;

namespace OllamaCommitGen.Cli.Extensions;

public static class CommandLineBuilderExtensions
{
    public static CommandLineBuilder UseErrorHandling(this CommandLineBuilder builder)
    {
        builder.AddMiddleware(async (context, next) =>
        {
            try
            {
                await next(context);
            }
            catch (LibGit2SharpException e)
            {
                Console.WriteLine($"Git error: {e.Message}");
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error occured: {e.Message}");
            }
        });

        return builder;
    }
}