using System;

namespace KafkaConsumeConsole.Options
{
    public record TaskOptions
    {
        public long ConsumeWaitDurationMs { get; set; } = 30000;
        
        public TimeSpan ConsumeWaitDuration => TimeSpan.FromMilliseconds(this.ConsumeWaitDurationMs);
    }
}