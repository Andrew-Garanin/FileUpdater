using System;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;


namespace FileUpdater
{
    class Updater
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static IniFile MyIni = new IniFile(@"..\..\data\Folders.ini");
        private static string folderToUpdate = MyIni.Read("FolderToUpdate");
        private static string updatedFolder = MyIni.Read("UpdatedFolder");
        private static string[] updatedFiles = Directory.GetFiles(updatedFolder);
        private int filesCount = updatedFiles.Length;
        private ProgressBar progressBar;

        public Updater(ProgressBar progressBar)
        {
            this.progressBar = progressBar;
        }

        public int getFilesCout()
        {
            return filesCount;
        }

        private bool isApplication(string fileName)
        {
            if (Path.GetExtension(fileName).Equals(".exe"))
                return true;
            return false;
        }

        private bool isProcessExist(string processName)
        {
            if (Process.GetProcesses().Any(p => p.ProcessName == processName))
                return true;
            return false;
        }

        private string generateTempFileName(string fileName)
        {
            string fileExtension = Path.GetExtension(fileName);
            return String.Format(@"{0}" + fileExtension, System.Guid.NewGuid());
        }

        private void closeProcess(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            bool wasClosed = false;
            foreach (Process process in processes)
            {
                if (process.CloseMainWindow())
                {
                    wasClosed = true;
                    Logger.Info("The " + processName + " application was closed.");
                    break;
                }
            }
            if (!wasClosed)
                Logger.Error("Faild to close the " + processName + " application.");
        }

        private void faildToUpdateFileMsgBox(string fileName)
        {
            MessageBox.Show("Failed to update the " + fileName + " file.",
                               "Updating files",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
        }

        public void Updating()
        {
            if (filesCount == 0)
            {
                MessageBox.Show("Folder with updated files is empty.",
                                "Updating files",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            for (int i = 0; i < filesCount; i++)
            {
                string fileName = Path.GetFileName(updatedFiles[i]);
                string fileToUpdatePath = Path.Combine(folderToUpdate, fileName);
                string updatedFilePath = Path.Combine(updatedFolder, fileName);

                if (File.Exists(fileToUpdatePath))
                {
                    if (isApplication(fileName))// Если это приложение
                    {
                        string processName = Path.GetFileNameWithoutExtension(fileName);
                        if (isProcessExist(processName))
                            closeProcess(processName);
                    }

                    DateTime updatedFileLastWriteTime = File.GetLastWriteTime(updatedFilePath);
                    DateTime fileToUpdateLastWriteTime = File.GetLastWriteTime(fileToUpdatePath);

                    if (updatedFileLastWriteTime.CompareTo(fileToUpdateLastWriteTime) > 0) // файл требуется обновить
                    {
                        string tempFilePath = Path.Combine(folderToUpdate, generateTempFileName(fileName));
                        while (File.Exists(tempFilePath))
                            tempFilePath = Path.Combine(folderToUpdate, generateTempFileName(fileName));// Генерируем новое имя до тех пор, пока оно не будет уникальным

                        File.Copy(fileToUpdatePath, tempFilePath); // Резервная копия файла в папке для обновления
                        try
                        {
                            File.Delete(fileToUpdatePath);
                        }
                        catch (IOException)
                        {
                            File.Delete(tempFilePath);
                            faildToUpdateFileMsgBox(fileName);
                            Logger.Error("Failed to update the " + fileName + " file.");
                            continue;
                        }

                        try
                        {
                            File.Copy(updatedFilePath, fileToUpdatePath);
                        }
                        catch (IOException)
                        {
                            File.Move(tempFilePath, fileToUpdatePath);
                            faildToUpdateFileMsgBox(fileName);
                            Logger.Error("Failed to update the " + fileName + " file.");
                            continue;
                        }

                        File.Delete(tempFilePath);
                        Logger.Info("File " + fileName + " is up to date.");
                    }
                }
                else
                {
                    File.Copy(updatedFilePath, fileToUpdatePath);
                    Logger.Info("File " + fileName + " added to the FolderToUpdate folder.");
                }
                progressBar.Value++;
            }
        }
    }
}
