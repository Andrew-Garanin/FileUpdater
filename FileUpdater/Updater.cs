using System;
using System.IO;
using System.Windows.Forms;


namespace FileUpdater
{
    class Updater
    {
        private IniFile iniFile;
        private string folderToUpdate;
        private string updatedFolder;
        private string[] updatedFiles;
        private int filesCount;
        private IProgressBar progressBar;

        public Updater(IProgressBar progressBar)
        {
            this.iniFile = new IniFile(@"..\..\configuration\FoldersConfiguration.ini");
            try
            {
                this.folderToUpdate = iniFile.Read("FolderToUpdate");
                this.updatedFolder = iniFile.Read("UpdatedFolder");
                this.updatedFiles = Directory.GetFiles(updatedFolder);
            }
            catch (DirectoryNotFoundException)
            {
                throw;
            }
            catch(ArgumentException)
            {
                throw;
            }
            this.filesCount = updatedFiles.Length;
            this.progressBar = progressBar;
            this.progressBar.Inicialize(this.filesCount);
        }

        private bool IsApplication(string fileName)
        {
            if (Path.GetExtension(fileName).Equals(".exe"))
                return true;
            return false;
        }

        private string GenerateTempFileName(string fileName)
        {
            string fileExtension = Path.GetExtension(fileName);
            return $"{System.Guid.NewGuid()}{fileExtension}";
        }

        private string GetUniqueFilePath(string fileName)
        {
            string tempFilePath = Path.Combine(folderToUpdate, GenerateTempFileName(fileName));
            while (File.Exists(tempFilePath)) // Генерируем новое имя до тех пор, пока оно не будет уникальным
                tempFilePath = Path.Combine(folderToUpdate, GenerateTempFileName(fileName));
            return tempFilePath;
        }

        private void FailedToUpdateFileMsgBox(string fileName)
        {
            MessageBox.Show($"Failed to update the {fileName} file.",
                            "Updating files",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
        }

        private void UpdateFile(string fileName)
        {
            string fileToUpdatePath = Path.Combine(folderToUpdate, fileName);
            string updatedFilePath = Path.Combine(updatedFolder, fileName);

            if (File.Exists(fileToUpdatePath))
            {
                if (IsApplication(fileName))
                {
                    string processName = Path.GetFileNameWithoutExtension(fileName);
                    if (ProcessManager.IsProcessExist(processName))
                        ProcessManager.CloseProcess(processName);
                }

                DateTime updatedFileLastWriteTime = File.GetLastWriteTime(updatedFilePath);
                DateTime fileToUpdateLastWriteTime = File.GetLastWriteTime(fileToUpdatePath);

                if (updatedFileLastWriteTime.CompareTo(fileToUpdateLastWriteTime) > 0) // файл требуется обновить
                {
                    string tempFilePath = GetUniqueFilePath(fileName);
                    File.Copy(fileToUpdatePath, tempFilePath); // Резервная копия файла в папке для обновления
                    try
                    {
                        File.Delete(fileToUpdatePath);
                    }
                    catch (IOException)
                    {
                        File.Delete(tempFilePath);
                        FailedToUpdateFileMsgBox(fileName);
                        Logger.WriteError($"Failed to update the {fileName} file.");
                        return;
                    }

                    try
                    {
                        File.Copy(updatedFilePath, fileToUpdatePath);
                    }
                    catch (IOException)
                    {
                        File.Move(tempFilePath, fileToUpdatePath);
                        FailedToUpdateFileMsgBox(fileName);
                        Logger.WriteError($"Failed to update the {fileName} file.");
                        return;
                    }

                    File.Delete(tempFilePath);
                    Logger.WriteInfo($"File {fileName} is up to date.");
                }
            }
            else
            {
                File.Copy(updatedFilePath, fileToUpdatePath);
                Logger.WriteInfo($"File {fileName} added to the FolderToUpdate folder.");
            }
        }

        public void UpdateFiles()
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
                UpdateFile(fileName);
                progressBar.Increment();
            }
        }
    }
}
