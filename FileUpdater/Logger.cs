

namespace FileUpdater
{
    class Logger
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static void WriteInfo(string info)
        {
            logger.Info(info);
        }

        public static void WriteError(string error)
        {
            logger.Error(error);
        }
    }
}
