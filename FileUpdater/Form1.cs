using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileUpdater
{
    public partial class Form1 : Form
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Shown(object sender, EventArgs e)
        {
            Updating();
            await Task.Delay(1000);
            Close();
        }

        private void Updating()
        {
            IniFile MyIni = new IniFile(@"..\..\data\Folders.ini");

            String folderToUpdate = MyIni.Read("FolderToUpdate");
            String updatedFolder = MyIni.Read("UpdatedFolder");

            String[] filesToUpdate = Directory.GetFiles(folderToUpdate);
            String[] updatedFiles = Directory.GetFiles(updatedFolder);

            // Настройка прогресс бара
            progressBar1.Maximum = updatedFiles.Length;
            progressBar1.Minimum = 0;
            progressBar1.Step = updatedFiles.Length / 100;
            progressBar1.Style = ProgressBarStyle.Continuous;

            if (updatedFiles.Length == 0)
            {
                MessageBox.Show("Folder with updated files is empty.",
                                "Updating files",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            for (int i = 0; i < updatedFiles.Length; i++)
            {
                String fileName = Path.GetFileName(updatedFiles[i]);

                if (File.Exists(Path.Combine(folderToUpdate, fileName)))
                {
                    String processName = Path.GetFileNameWithoutExtension(fileName);
                    var processExists = Process.GetProcesses().Any(p => p.ProcessName == processName);
                    if (processExists)
                    {
                        Process[] workers = Process.GetProcessesByName(processName);
                        foreach (Process worker in workers)
                        {
                            worker.Kill();
                            worker.WaitForExit();
                            worker.Dispose();
                            Logger.Info("Closed application " + worker);
                        }
                        continue;
                    }

                    DateTime dt1 = File.GetLastWriteTime(Path.Combine(updatedFolder, fileName));
                    DateTime dt2 = File.GetLastWriteTime(Path.Combine(folderToUpdate, fileName));

                    if (dt1.CompareTo(dt2) > 0) // dt1 позже чем dt2
                    {
                        var result = File.ReadAllLines(Path.Combine(updatedFolder, fileName)).Select(m => m.Substring(0, Math.Min(38, m.Length)));
                        File.WriteAllLines(Path.Combine(folderToUpdate, fileName), result);
                        Logger.Info("File " + fileName + " is up to date.");
                    }
                }
                else
                {
                    File.Copy(Path.Combine(updatedFolder, fileName), Path.Combine(folderToUpdate, fileName));
                    Logger.Info("File " + fileName + " added to the FolderToUpdate folder.");
                }
                progressBar1.Value++;
            }
        }
    }
}
