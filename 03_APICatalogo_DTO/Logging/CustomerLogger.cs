namespace APICatalogo.Logging;

public class CustomerLogger : ILogger
{
    readonly string loggerName;

    readonly CustomLoggerProviderConfiguration loggerConfig;

    public CustomerLogger(string name, CustomLoggerProviderConfiguration config)
    {
        loggerName = name;
        loggerConfig = config;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel == loggerConfig.LogLevel;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return null;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
        Exception? exception, Func<TState, Exception?, string> formatter)
    {
        string message = $"{logLevel}: {eventId.Id} - {formatter(state, exception)}";
        WriteLog(message);
    }

    private void WriteLog(string mensagem)
    {
        string path = @"C:\Users\livia\OneDrive\Área de Trabalho\estudos-asp-net\APICatalogo\Log.txt";

        using (StreamWriter streamWriter = new StreamWriter(path, true))
        {
            try
            {
                streamWriter.WriteLine(mensagem);
                streamWriter.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
