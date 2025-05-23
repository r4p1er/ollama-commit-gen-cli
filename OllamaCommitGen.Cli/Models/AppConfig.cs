namespace OllamaCommitGen.Cli.Models
{
    public class AppConfig
    {
        public string? Origin { get; set; } = null!;

        public string? Model { get; set; } = null!;

        public string? Lang { get; set; } = null!;

        public string? KeepAlive { get; set; } = null!;

        public bool? NoStream { get; set; }

        public string? Example { get; set; }

        public string? ExampleDescription { get; set; }

        public int? MiroStat { get; set; }

        public float? MiroStatEta { get; set; }

        public float? MiroStatTau { get; set; }

        public int? NumCtx { get; set; }

        public int? RepeatLastN { get; set; }

        public float? RepeatPenalty { get; set; }

        public float? Temperature { get; set; }

        public int? Seed { get; set; }

        public string[]? Stop { get; set; }

        public float? TfsZ { get; set; }

        public int? NumPredict { get; set; }

        public int? TopK { get; set; }

        public float? TopP { get; set; }
    }
}
