using System;
using Serilog.Aggregating.Filter;
using Serilog.Configuration;
using Serilog.Events;

// ReSharper disable once IdentifierTypo
// ReSharper disable once CheckNamespace
namespace Serilog
{
    public static class LoggerFilterConfigurationExtensions
    {
        public static LoggerConfiguration UniqueOverSpan(this LoggerFilterConfiguration configuration, Func<LogEvent, bool> inclusionPredicate, TimeSpan span)
        {
            return configuration.With(new UniqueOverSpanFilter(inclusionPredicate, span));
        }
    }
}
