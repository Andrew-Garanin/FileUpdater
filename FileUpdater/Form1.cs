using System;
using System.IO;
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

        private async void Form1_Shown(object sender, EventArgs e)
        {
            Updater updater;
            try
            {
                updater = new Updater(progressBar);
                BarSetting(updater.getFilesCout());
                updater.Updating();
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Asd!");
            }
            catch (ArgumentException)
            {
                MessageBox.Show("asdaw11!");
            }
            await Task.Delay(1000);
            Close();
        }

        private void BarSetting(int filesCount)
        {
            progressBar.Maximum = filesCount;
            progressBar.Minimum = 0;
            progressBar.Step = filesCount / 100;
            progressBar.Style = ProgressBarStyle.Continuous;
        }
    }
}
