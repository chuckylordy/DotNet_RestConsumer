namespace MaKro.Libraries.RestConsumer.Implementation.Logging
{
    /// <summary>
    /// Logggingstages (must be ordered by their importance where the first stage is the less, the last stage the most important).
    /// </summary>
    public enum LogStage
    {
        DEBUG,
        INFORMATION,
        WARNING,
        ERROR
    }
}
