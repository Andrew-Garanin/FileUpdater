using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace FileUpdater
{
    public partial class ProgressBarForm : Form, IProgressBar
    {
        public ProgressBarForm()
        {
            InitializeComponent();
        }

        private async void ProgressBarForm_Shown(object sender, EventArgs e)
        {
            Updater updater;
            try
            {
                updater = new Updater(this);
                updater.UpdateFiles();
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Directory not found.",
                                "Updating files",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Directory path not specified.",
                                "Updating files",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            await Task.Delay(1000);
            Close();
        }

        public void Inicialize(int filesCount)
        {
            progressBar.Maximum = filesCount;
            progressBar.Minimum = 0;
            progressBar.Step = filesCount / 100;
            progressBar.Style = ProgressBarStyle.Continuous;
        }

        public void Increment()
        {
            progressBar.Value++;
        }
    }
}
