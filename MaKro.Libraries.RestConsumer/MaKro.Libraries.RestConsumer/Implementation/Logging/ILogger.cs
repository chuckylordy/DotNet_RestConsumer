using System;

namespace MaKro.Libraries.RestConsumer.Implementation.Logging
{
    public interface ILogger
    {
        LogStage LocalLogStage { get; }

        void Log(Exception aiException, LogStage aiStage);

        void Log(string aiMessage, LogStage aiStage);
    }
}
