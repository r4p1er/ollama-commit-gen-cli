using System.CommandLine;
using System.CommandLine.Binding;
using Iso639;
using OllamaCommitGen.Cli.Models;
using OllamaCommitGen.Cli.Utils;
using OllamaCommitGen.Domain.Abstractions;
using OllamaCommitGen.Domain.Services;
using OllamaCommitGen.Infrastructure.Services;
using OllamaSharp.Models;

namespace OllamaCommitGen.Cli.Binders;

public class CommitGenBinder(PrimaryOptions primaryOptions, ModelOptions modelOptions) : BinderBase<ICommitGenService>
{
    protected override ICommitGenService GetBoundValue(BindingContext bindingContext)
    {
        var git = new GitService(Directory.GetCurrentDirectory());

        var configPath = bindingContext.ParseResult.GetValueForOption(primaryOptions.ConfigOption);
        AppConfig? appConfig = null;
        if (configPath != null) appConfig = ConfigLoader.LoadAppConfig(configPath);

        var uri = ResolveOptionValue(bindingContext, primaryOptions.OriginOption, appConfig?.Origin);
        var model = ResolveOptionValue(bindingContext, primaryOptions.ModelOption, appConfig?.Model);
        var langStr = ResolveOptionValue(bindingContext, primaryOptions.LangOption, appConfig?.Lang);
        var keepalive = ResolveOptionValue(bindingContext, primaryOptions.KeepAliveOption, appConfig?.KeepAlive);
        var noStream = ResolveOptionValue(bindingContext, primaryOptions.NoStreamOption, appConfig?.NoStream);

        var lang = Language.FromPart3(langStr);
        if (lang == null) throw new ArgumentException("Provided lang is not an ISO-639-3 valid code");

        var ollama = new OllamaService(uri);

        ollama.Request.Model = model;
        ollama.Request.System +=
            $"The language of your response must correspond this ISO-639-3 code: {lang.Part3}.";
        ollama.Request.KeepAlive = keepalive;
        ollama.Request.Stream = !noStream;

        var miroStat = ResolveOptionValue(bindingContext, modelOptions.MiroStatOption, appConfig?.MiroStat);
        var miroStatEta = ResolveOptionValue(bindingContext, modelOptions.MiroStatEtaOption, appConfig?.MiroStatEta);
        var miroStatTau = ResolveOptionValue(bindingContext, modelOptions.MiroStatTauOption, appConfig?.MiroStatTau);
        var numCtx = ResolveOptionValue(bindingContext, modelOptions.NumCtxOption, appConfig?.NumCtx);
        var repeatLastN = ResolveOptionValue(bindingContext, modelOptions.RepeatLastNOption, appConfig?.RepeatLastN);
        var repeatPenalty = ResolveOptionValue(bindingContext, modelOptions.RepeatPenaltyOption, appConfig?.RepeatPenalty);
        var temperature = ResolveOptionValue(bindingContext, modelOptions.TemperatureOption, appConfig?.Temperature);
        var seed = ResolveOptionValue(bindingContext, modelOptions.SeedOption, appConfig?.Seed);
#pragma warning disable CS8634 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'class' constraint.
        var stop = ResolveOptionValue(bindingContext, modelOptions.StopOption, appConfig?.Stop);
#pragma warning restore CS8634
        var tfsZ = ResolveOptionValue(bindingContext, modelOptions.TfsZOption, appConfig?.TfsZ);
        var numPredict = ResolveOptionValue(bindingContext, modelOptions.NumPredictOption, appConfig?.NumPredict);
        var topK = ResolveOptionValue(bindingContext, modelOptions.TopKOption, appConfig?.TopK);
        var topP = ResolveOptionValue(bindingContext, modelOptions.TopPOption, appConfig?.TopP);

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
            Stop = stop,
            TfsZ = tfsZ,
            NumPredict = numPredict,
            TopK = topK,
            TopP = topP
        };

        return new CommitGenService(git, ollama);
    }

    private T ResolveOptionValue<T>(BindingContext ctx, Option<T> opt, T? configVal) where T : struct
    {
        var optResult = ctx.ParseResult.FindResultFor(opt);
        var optValue = ctx.ParseResult.GetValueForOption(opt)!;

        if (optResult != null && !optResult.IsImplicit)
            return optValue;
        if (configVal.HasValue)
            return configVal.Value;

        return optValue;
    }

    private T ResolveOptionValue<T>(BindingContext ctx, Option<T> opt, T? configVal) where T : class
    {
        var optResult = ctx.ParseResult.FindResultFor(opt);
        var optValue = ctx.ParseResult.GetValueForOption(opt)!;

        if (optResult != null && !optResult.IsImplicit)
            return optValue;
        if (configVal != null)
            return configVal;

        return optValue;
    }
}