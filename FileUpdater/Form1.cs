using System;
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
            Updater updater = new Updater(progressBar);
            BarSetting(updater.getFilesCout());
            updater.Updating();
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
