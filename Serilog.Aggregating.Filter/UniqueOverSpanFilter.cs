using System;
using System.Runtime.Caching;
using Serilog.Core;
using Serilog.Events;

// ReSharper disable once IdentifierTypo
namespace Serilog.Aggregating.Filter
{
    public class UniqueOverSpanFilter : ILogEventFilter
    {
        private static readonly MemoryCache Cache;
        private readonly Func<LogEvent, bool> _isEnabled;
        private readonly TimeSpan _span;

        static UniqueOverSpanFilter()
        {
            Cache = new MemoryCache("UniqueLogEntries");
        }

        public UniqueOverSpanFilter(Func<LogEvent, bool> isEnabled, TimeSpan span)
        {
            _isEnabled = isEnabled ?? throw new ArgumentNullException(nameof(isEnabled));
            _span = span;
        }

        public bool IsEnabled(LogEvent @event)
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            if (_isEnabled(@event))
            {
                var key = @event
                            .MessageTemplate
                            .Render(@event.Properties)
                            .GetHashCode()
                            .ToString();

                if (Cache.Contains(key))
                {
                    return false;
                }

                Cache
                    .Add
                    (
                        key,
                        key, // We're not really caching anything
                        new CacheItemPolicy
                        {
                            AbsoluteExpiration = new DateTimeOffset(DateTime.UtcNow.Add(_span))
                        }
                    );
            }

            return true;
        }
    }
}
