
namespace FileUpdater
{
    partial class ProgressBarForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.labelUpdating = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 53);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(442, 16);
            this.progressBar.TabIndex = 2;
            // 
            // labelUpdating
            // 
            this.labelUpdating.AutoSize = true;
            this.labelUpdating.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelUpdating.Location = new System.Drawing.Point(186, 19);
            this.labelUpdating.Name = "labelUpdating";
            this.labelUpdating.Size = new System.Drawing.Size(97, 22);
            this.labelUpdating.TabIndex = 3;
            this.labelUpdating.Text = "Updating...";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 97);
            this.Controls.Add(this.labelUpdating);
            this.Controls.Add(this.progressBar);
            this.Name = "Form1";
            this.Text = "Updating Files";
            this.Shown += new System.EventHandler(this.ProgressBarForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label labelUpdating;
    }
}

