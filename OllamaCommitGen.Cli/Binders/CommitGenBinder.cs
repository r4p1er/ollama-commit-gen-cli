using System.CommandLine.Binding;
using Iso639;
using OllamaCommitGen.Cli.Models;
using OllamaCommitGen.Domain.Interfaces;
using OllamaCommitGen.Domain.Services;
using OllamaCommitGen.Infrastructure.Services;
using OllamaCommitGen.OllamaSharp.Models;

namespace OllamaCommitGen.Cli.Binders;

public class CommitGenBinder(PrimaryOptions primaryOptions, ModelOptions modelOptions) : BinderBase<ICommitGenService>
{
    protected override ICommitGenService GetBoundValue(BindingContext bindingContext)
    {
        var git = new GitService(Directory.GetCurrentDirectory());

        var uri = bindingContext.ParseResult.GetValueForOption(primaryOptions.OriginOption)!;
        var model = bindingContext.ParseResult.GetValueForOption(primaryOptions.ModelOption)!;
        var langStr = bindingContext.ParseResult.GetValueForOption(primaryOptions.LangOption)!;
        var lang = Language.FromPart3(langStr);
        var keepalive = bindingContext.ParseResult.GetValueForOption(primaryOptions.KeepAliveOption)!;
        var noStream = bindingContext.ParseResult.GetValueForOption(primaryOptions.NoStreamOption)!;

        if (lang == null) throw new ArgumentException("Provided lang is not an ISO-639-3 valid code");

        var ollama = new OllamaService(uri);

        ollama.Request.Model = model;
        ollama.Request.System +=
            $"The language of your response must correspond this ISO-639-3 code: {lang.Part3}.";
        ollama.Request.KeepAlive = keepalive;
        ollama.Request.Stream = !noStream;

        var miroStat = bindingContext.ParseResult.GetValueForOption(modelOptions.MiroStatOption)!;
        var miroStatEta = bindingContext.ParseResult.GetValueForOption(modelOptions.MiroStatEtaOption)!;
        var miroStatTau = bindingContext.ParseResult.GetValueForOption(modelOptions.MiroStatTauOption)!;
        var numCtx = bindingContext.ParseResult.GetValueForOption(modelOptions.NumCtxOption)!;
        var repeatLastN = bindingContext.ParseResult.GetValueForOption(modelOptions.RepeatLastNOption)!;
        var repeatPenalty = bindingContext.ParseResult.GetValueForOption(modelOptions.RepeatPenaltyOption)!;
        var temperature = bindingContext.ParseResult.GetValueForOption(modelOptions.TemperatureOption)!;
        var seed = bindingContext.ParseResult.GetValueForOption(modelOptions.SeedOption)!;
        var stop = bindingContext.ParseResult.GetValueForOption(modelOptions.StopOption)!;
        var tfsZ = bindingContext.ParseResult.GetValueForOption(modelOptions.TfsZOption)!;
        var numPredict = bindingContext.ParseResult.GetValueForOption(modelOptions.NumPredictOption)!;
        var topK = bindingContext.ParseResult.GetValueForOption(modelOptions.TopKOption)!;
        var topP = bindingContext.ParseResult.GetValueForOption(modelOptions.TopPOption)!;

        ollama.Request.Options = new RequestOptions()
        {
            MiroStat = miroStat,
            MiroStatEta = miroStatEta,
            MiroStatTau = miroStatTau,
            NumCtx = numCtx,
            RepeatLastN = repeatLastN,
            RepeatPenalty = repeatPenalty,
            Temperature = temperature,
            Seed = seed,
            Stop = stop == null || stop.Count == 0 ? null : string.Join("|", stop),
            TfsZ = tfsZ,
            NumPredict = numPredict,
            TopK = topK,
            TopP = topP
        };

        return new CommitGenService(git, ollama);
    }
}