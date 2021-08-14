using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MessageBrokers.Kafka
{
    internal class KafkaMessageCollection
    {
        private readonly ILogger<KafkaMessageCollection> _logger;
        private readonly Dictionary<IMessage, ConsumeResult<string, string>> _results = new();

        public KafkaMessageCollection(ILogger<KafkaMessageCollection> logger)
        {
            this._logger = logger;
        }

        public void Add<TMessage>(TMessage message, ConsumeResult<string, string> consumeResult) where TMessage : IMessage
        {
            if (this._results.ContainsKey(message))
            {
                this._logger.LogInformation("Message already registered: {Message}", message);
                return;
            }
            this._results.Add(message, consumeResult);
        }

        public bool TryGet<TMessage>(TMessage message, [NotNullWhen(true)] out ConsumeResult<string, string>? consumeResult) where TMessage : IMessage
            => this._results.TryGetValue(message, out consumeResult);
    }
}