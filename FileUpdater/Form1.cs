using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileUpdater
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void Updating()
        {
            // Поменять на относительный путь
            IniFile MyIni = new IniFile(@"D:\Projects\.Net Projects\FileUpdater\FileUpdater\FileUpdater\data\Folders.ini");

            String folderToUpdate = MyIni.Read("FolderToUpdate");
            String folderUpdated = MyIni.Read("UpdatedFolder");


            String[] filesToUpdate = Directory.GetFiles(folderToUpdate);
            String[] filesUpdated = Directory.GetFiles(folderUpdated);

            progressBar1.Maximum = filesUpdated.Length;
            progressBar1.Minimum = 0;
            progressBar1.Step = filesUpdated.Length / 100;
            progressBar1.Style = ProgressBarStyle.Continuous;

            for (int i = 0; i < filesUpdated.Length; i++)
            {
                String fileName = Path.GetFileName(filesUpdated[i]);

                if (File.Exists(Path.Combine(folderToUpdate, fileName)))
                {
                    String processName = fileName;
                    var processExists = Process.GetProcesses().Any(p => p.ProcessName == processName);
                    if (processExists)
                    {
                        Process[] workers = Process.GetProcessesByName(fileName);
                        foreach (Process worker in workers)
                        {
                            worker.Kill();
                            worker.WaitForExit();
                            worker.Dispose();
                        }
                        continue;
                    }

                    DateTime dt1 = File.GetLastWriteTime(Path.Combine(folderUpdated, fileName));
                    DateTime dt2 = File.GetLastWriteTime(Path.Combine(folderToUpdate, fileName));
                    if (dt1 == dt2)
                        MessageBox.Show("date is teh same!");
                    else if (dt1.CompareTo(dt2) > 0) // dt1 позже чем dt2
                    {
                        MessageBox.Show("date are different!");
                        var result = File.ReadAllLines(Path.Combine(folderUpdated, fileName)).Select(m => m.Substring(0, Math.Min(38, m.Length)));
                        File.WriteAllLines(Path.Combine(folderToUpdate, fileName), result);
                    }
                }
                else
                {
                    File.Copy(Path.Combine(folderUpdated, fileName), Path.Combine(folderToUpdate, fileName));
                }
                progressBar1.Value++;    
            } 
        }

        private async void Form1_Shown(object sender, EventArgs e)
        {
            Updating();
            await Task.Delay(1000);
            Close();
        }
    }
}
