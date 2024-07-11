using System.CommandLine;
using System.CommandLine.Binding;
using Iso639;
using OllamaCommitGen.Domain.Interfaces;
using OllamaCommitGen.Domain.Services;
using OllamaCommitGen.Infrastructure.Services;

namespace OllamaCommitGen.Cli.Binders;

public class CommitGenBinder(
    Option<string> originOption,
    Option<string> modelOption,
    Option<string> langOption,
    Option<string> keepaliveOption,
    Option<bool> noStreamOption)
    : BinderBase<ICommitGenService>
{
    protected override ICommitGenService GetBoundValue(BindingContext bindingContext)
    {
        var git = new GitService(Directory.GetCurrentDirectory());

        var uri = bindingContext.ParseResult.GetValueForOption(originOption)!;
        var model = bindingContext.ParseResult.GetValueForOption(modelOption)!;
        var langStr = bindingContext.ParseResult.GetValueForOption(langOption)!;
        var lang = Language.FromPart3(langStr);
        var keepalive = bindingContext.ParseResult.GetValueForOption(keepaliveOption)!;
        var noStream = bindingContext.ParseResult.GetValueForOption(noStreamOption)!;

        if (lang == null) throw new ArgumentException("Provided lang is not an ISO-639-3 valid code");

        var ollama = new OllamaService(uri);

        ollama.Request.Model = model;
        ollama.Request.System +=
            $"The language of your response must correspond this ISO-639-3 code: {lang.Part3}.";
        ollama.Request.KeepAlive = keepalive;
        ollama.Request.Stream = !noStream;

        return new CommitGenService(git, ollama);
    }
}