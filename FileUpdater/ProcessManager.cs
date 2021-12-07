using System.Diagnostics;


namespace FileUpdater
{
    class ProcessManager
    {
        public static bool IsProcessExist(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            if (processes.Length!=0)
                return true;
            return false;
        }

        public static void CloseProcess(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            bool wasClosed = false;
            foreach (Process process in processes)
            {
                if (process.CloseMainWindow())
                {
                    wasClosed = true;
                    Logger.WriteInfo("The " + processName + " application was closed.");
                    break;
                }
            }
            if (!wasClosed)
                Logger.WriteError("Failed to close the " + processName + " application.");
        }
    }
}
