# Serilog.Aggregating.Filter
Aggregation-based event filtering for [Serilog](https://serilog.net).

```csharp
const string expr = "@Level = 'Information' and AppId is not null and Items[?] like 'C%'";

Log.Logger = new LoggerConfiguration()
                .Enrich.WithProperty("AppId", 10)
                .Filter.UniqueOverSpan(expr, TimeSpan.FromSeconds(20))
                .WriteTo.Console()
                .CreateLogger();

// Printed
Log.Information("Cart contains {@Items}", new[] { "Tea", "Coffee" });

// Not printed (duplicate message)
Log.Information("Cart contains {@Items}", new[] { "Tea", "Coffee" });

//Printed (distinct message)
Log.Information("Cart contains {@Items}", new[] { "Peanuts", "Chocolate" });

// Printed (excluded from filter due to log level)
Log.Warning("Cart contains {@Items}", new[] { "Tea", "Coffee" });

Log.CloseAndFlush();
```

### Getting started

Install _Serilog.Aggregating.Filter_ from NuGet:

```powershell
Install-Package Serilog.Aggregating.Filter
```

Please see [Serilog.Filters.Expressions](https://github.com/serilog/serilog-filters-expressions) for filter syntax.

### JSON `appSettings.json` configuration

Using [_Serilog.Settings.Configuration_](https://github.com/serilog/serilog-settings-configuration):

```json
{
  "Serilog": {
    "Using": [
        "Serilog.Settings.Configuration",
        "Serilog.Aggregating.Filter"
    ],
    "Filter": [
      {
        "Name": "UniqueOverSpan",
        "Args": {
            "expression": "@Level = 'Information' and Items[?] like 'C%'",
            "span": "00:00:20"

        }
      }
    ]
```


