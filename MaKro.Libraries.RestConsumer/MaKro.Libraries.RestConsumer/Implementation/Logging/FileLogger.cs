using System;
using System.IO;

namespace MaKro.Libraries.RestConsumer.Implementation.Logging
{
    public class FileLogger : ILogger
    {
        public LogStage LocalLogStage
        {
            get; private set;
        }

        public FileLogger(LogStage aiStage)
        {
            LocalLogStage = aiStage;
        }

        /// <summary>
        /// Logging the message.
        /// </summary>
        /// <param name="aiMessage"></param>
        /// <param name="aiStage"></param>
        public void Log(string aiMessage, LogStage aiStage)
        {
            WriteLog(new Exception(aiMessage), aiStage);
        }

        /// <summary>
        /// Logging the Exception.
        /// </summary>
        /// <param name="aiException"></param>
        /// <param name="aiStage"></param>
        public void Log(Exception aiException, LogStage aiStage)
        {
            WriteLog(aiException, aiStage);
        }

        #region private get/set for FileLogging.
        private string LogPath
        {
            get
            {
                return Path.GetTempPath() + "\\scanservice_" + TodayDateAsString + ".log";
            }
        }

        private string NowAsString
        {
            get
            {
                return DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss");
            }
        }

        private string TodayDateAsString
        {
            get
            {
                return DateTime.Now.ToString("yyyy_MM_dd");
            }
        }
        #endregion

        /// <summary>
        /// Writes the logs to the FilePath.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="aiStage"></param>
        private void WriteLog(Exception e, LogStage aiStage)
        {
            if(aiStage >= LocalLogStage)
            {
                if(e.InnerException != null && e.StackTrace != null)
                File.AppendAllText(LogPath, NowAsString + " ( " + aiStage.ToString() +" ) - Message: " + e.Message + " (Inner Exception: " + e.InnerException +" )\r\nStacktrace: " + e.StackTrace+"\r\n");
                else if(e.InnerException != null)
                    File.AppendAllText(LogPath, NowAsString + " ( " + aiStage.ToString() + " ) - Message: " + e.Message + " (Inner Exception: " + e.InnerException + " )\r\n");
                else if(e.StackTrace != null)
                    File.AppendAllText(LogPath, NowAsString + " ( " + aiStage.ToString() + " ) - Message: " + e.Message + "\r\nStacktrace: " + e.StackTrace + "\r\n");
                else
                    File.AppendAllText(LogPath, NowAsString + " ( " + aiStage.ToString() + " ) - Message: " + e.Message + "\r\n");
            }
        } 
    }
}
