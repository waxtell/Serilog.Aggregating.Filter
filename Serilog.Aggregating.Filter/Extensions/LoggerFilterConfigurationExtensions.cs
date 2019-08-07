using System;
using Serilog.Aggregating.Filter;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Filters.Expressions;

// ReSharper disable once IdentifierTypo
// ReSharper disable once CheckNamespace
namespace Serilog
{
    public static class LoggerFilterConfigurationExtensions
    {
        public static LoggerConfiguration UniqueOverSpan(this LoggerFilterConfiguration configuration, Func<LogEvent, bool> inclusionPredicate, TimeSpan span)
        {
            return 
                configuration
                    .With(new UniqueOverSpanFilter(inclusionPredicate, span));
        }

        public static LoggerConfiguration UniqueOverSpan(this LoggerFilterConfiguration loggerFilterConfiguration, string expression, TimeSpan span)
        {
            if (loggerFilterConfiguration == null)
            {
                throw new ArgumentNullException(nameof(loggerFilterConfiguration));
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            var compiled = FilterLanguage.CreateFilter(expression);

            return
                loggerFilterConfiguration
                    .UniqueOverSpan
                    (
                        e => true.Equals(compiled(e)),
                        span
                    );
        }
    }
}
