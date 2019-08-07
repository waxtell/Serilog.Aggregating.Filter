using System;
using Serilog;
using Serilog.Events;

namespace SampleApp9000
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                            .Enrich.WithProperty("AppId", 10)
                            .Filter
                            .UniqueOverSpan
                            (
                                @event => @event.Level == LogEventLevel.Information && @event.MessageTemplate.Text.StartsWith("Cart contains"),
                                TimeSpan.FromSeconds(25)
                            )
                            .WriteTo.Console()
                            .CreateLogger();

            Log.Information("Cart contains {@Items}", new[] { "Tea", "Coffee" });
            Log.Information("Cart contains {@Items}", new[] { "Tea", "Coffee" });
            Log.Information("Cart contains {@Items}", new[] { "Tea", "Coffee" });
            Log.Information("Cart contains {@Items}", new[] { "Tea", "Coffee" });
            Log.Warning("Cart contains {@Items}", new[] { "Tea", "Coffee" });
            Log.Information("Cart contains {@Items}", new[] { "Apricots" });
            Log.Information("Cart contains {@Items}", new[] { "Peanuts", "Chocolate" });

            Log.CloseAndFlush();

            // [12:38:54 INF] Cart contains["Tea", "Coffee"]            <-- Only the first of the four identical entries is logged
            // [12:38:54 WRN] Cart contains["Tea", "Coffee"]            <-- Does not meet the filter for inclusion (log level != Information)
            // [12:38:54 INF] Cart contains["Apricots"]                 <-- Distinct message so entry is logged
            // [12:38:54 INF] Cart contains["Peanuts", "Chocolate"]     <-- Distinct message so entry is logged
        }
    }
}
